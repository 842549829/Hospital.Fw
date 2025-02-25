using System.Diagnostics;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Hospital.Fw.Domain.Shared.Custom.Caching;

public class DistributedSessionCache(
    IOptions<SessionRedisCacheOptions> configuration,
    ILogger<DistributedSessionCache> logger)
    : IDistributedSessionCache
{
    private const long SugarNotPresent = -1;
    private const string SugarUserinfoKey = "USERINFO_KEY";
    //private const string SugarAuthorizeCore = "AUTHORIZE_CORE";
    private const string SugarSessionTimeout = "SessionTimeout";
    private const string SugarApplicationName = "Guigug.His.App.Container";

    private volatile IDatabase? _cache;
    private bool _disposed;
    private readonly ILogger _logger = logger;
    private readonly SemaphoreSlim _connectionLock = new(initialCount: 1, maxCount: 1);

    private long _lastConnectTicks = DateTimeOffset.UtcNow.Ticks;
    private long _firstErrorTimeTicks;
    private long _previousErrorTimeTicks;

    private readonly TimeSpan _reconnectMinInterval = TimeSpan.FromSeconds(60);
    private readonly TimeSpan _reconnectErrorThreshold = TimeSpan.FromSeconds(30);

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        ReleaseConnection(Interlocked.Exchange(ref _cache, null));
    }

    public byte[]? Get(string sessionId)
    {
        return GetAndRefresh(sessionId);
    }

    private byte[]? GetAndRefresh(string sessionId)
    {
        var cache = Connect();

        var key1 = $"{{{SugarApplicationName}_{sessionId}}}_Data";
        var key2 = $"{{{SugarApplicationName}_{sessionId}}}_Internal";

        RedisValue results1;
        RedisValue results2;
        TimeSpan? ttl;
        try
        {
            ttl = cache.KeyTimeToLive(key1);
            results1 = cache.HashGet(key1, SugarUserinfoKey);
            results2 = cache.HashGet(key2, SugarSessionTimeout);
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }

        if (ttl != null)
        {
            var ttlTick = (long?)results2;
            if (ttlTick.HasValue)
            {
                MapMetadata(results2, out var absExpr);
                if (absExpr.HasValue)
                {
                    Refresh(cache, key1, absExpr.Value);
                    Refresh(cache, key2, absExpr.Value);
                }
            }
        }
        return results1;
    }

    private static void MapMetadata(RedisValue results, out TimeSpan? absoluteExpiration)
    {
        absoluteExpiration = null;
        var absoluteExpirationTicks = (long?)results;
        if (absoluteExpirationTicks.HasValue && absoluteExpirationTicks.Value != SugarNotPresent)
        {
            var timeSpan = TimeSpan.FromSeconds(absoluteExpirationTicks.Value);
            absoluteExpiration = timeSpan;
        }
    }

    private void Refresh(IDatabase cache, string key, TimeSpan expr)
    {
        try
        {
            cache.KeyExpire(key, expr);
        }
        catch (Exception ex)
        {
            OnRedisError(ex, cache);
            throw;
        }
    }

    private IDatabase Connect()
    {
        CheckDisposed();
        var cache = _cache;
        if (cache is not null)
        {
            Debug.Assert(_cache is not null);
            return cache;
        }

        _connectionLock.Wait();
        try
        {
            cache = _cache;
            if (cache is null)
            {
                var connectionString = configuration.Value.Configuration;
                if (connectionString == null)
                {
                    throw new ArgumentNullException(nameof(connectionString));
                }
                
                var options = ConfigurationOptions.Parse(connectionString);
                options.AbortOnConnectFail = false;
                options.ConnectRetry = 5; // 设置重试次数
                options.ConnectTimeout = 5000; // 设置超时时间 (ms)
                IConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);
                //IConnectionMultiplexer connection = ConnectionMultiplexer.Connect(connectionString);

                PrepareConnection(connection);
                cache = _cache = connection.GetDatabase();
            }
            Debug.Assert(_cache is not null);
            return cache;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    private void PrepareConnection(IConnectionMultiplexer connection)
    {
        WriteTimeTicks(ref _lastConnectTicks, DateTimeOffset.UtcNow);
        TryRegisterProfiler(connection);
        TryAddSuffix(connection);
    }

    private static void WriteTimeTicks(ref long field, DateTimeOffset value)
    {
        var ticks = value == DateTimeOffset.MinValue ? 0L : value.UtcTicks;
        Volatile.Write(ref field, ticks); // avoid torn values
    }

    private static DateTimeOffset ReadTimeTicks(ref long field)
    {
        var ticks = Volatile.Read(ref field); // avoid torn values
        return ticks == 0 ? DateTimeOffset.MinValue : new DateTimeOffset(ticks, TimeSpan.Zero);
    }

    private void TryRegisterProfiler(IConnectionMultiplexer connection)
    {
        _ = connection ?? throw new InvalidOperationException($"{nameof(connection)} cannot be null.");
    }

    private void TryAddSuffix(IConnectionMultiplexer connection)
    {
        try
        {
            connection.AddLibraryNameSuffix("aspnet");
            connection.AddLibraryNameSuffix("DC");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TryAddSuffix");
        }
    }

    private void CheckDisposed()
    {
        ObjectDisposedThrowHelper.ThrowIf(_disposed, this);
    }

    private static void ReleaseConnection(IDatabase? cache)
    {
        var connection = cache?.Multiplexer;
        if (connection is not null)
        {
            try
            {
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

    private void OnRedisError(Exception exception, IDatabase cache)
    {
        if ((exception is RedisConnectionException or SocketException))
        {
            var utcNow = DateTimeOffset.UtcNow;
            var previousConnectTime = ReadTimeTicks(ref _lastConnectTicks);
            TimeSpan elapsedSinceLastReconnect = utcNow - previousConnectTime;

            // We want to limit how often we perform this top-level reconnect, so we check how long it's been since our last attempt.
            if (elapsedSinceLastReconnect < _reconnectMinInterval)
            {
                return;
            }

            var firstErrorTime = ReadTimeTicks(ref _firstErrorTimeTicks);
            if (firstErrorTime == DateTimeOffset.MinValue)
            {
                // note: order/timing here (between the two fields) is not critical
                WriteTimeTicks(ref _firstErrorTimeTicks, utcNow);
                WriteTimeTicks(ref _previousErrorTimeTicks, utcNow);
                return;
            }

            TimeSpan elapsedSinceFirstError = utcNow - firstErrorTime;
            TimeSpan elapsedSinceMostRecentError = utcNow - ReadTimeTicks(ref _previousErrorTimeTicks);

            bool shouldReconnect =
                    elapsedSinceFirstError >= _reconnectErrorThreshold // Make sure we gave the multiplexer enough time to reconnect on its own if it could.
                    && elapsedSinceMostRecentError <= _reconnectErrorThreshold; // Make sure we aren't working on stale data (e.g. if there was a gap in errors, don't reconnect yet).

            // Update the previousErrorTime timestamp to be now (e.g. this reconnect request).
            WriteTimeTicks(ref _previousErrorTimeTicks, utcNow);

            if (!shouldReconnect)
            {
                return;
            }

            WriteTimeTicks(ref _firstErrorTimeTicks, DateTimeOffset.MinValue);
            WriteTimeTicks(ref _previousErrorTimeTicks, DateTimeOffset.MinValue);

            // wipe the shared field, but *only* if it is still the cache we were
            // thinking about (once it is null, the next caller will reconnect)
            var tmp = Interlocked.CompareExchange(ref _cache, null, cache);
            if (ReferenceEquals(tmp, cache))
            {
                ReleaseConnection(tmp);
            }
        }
    }
}