using Hospital.Fw.Domain.Shared.Custom.Caching;
using Hospital.Fw.Domain.Shared.Custom.Caching.Users;

namespace Hospital.Fw.HttpApi.Custom.Security;

/// <summary>
/// SessionUserExtensions
/// </summary>
public static class SessionUserExtensions
{
    /// <summary>
    /// AddSessionUser(原生注入方式)
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="setupAction">setupAction</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddSessionUser(this IServiceCollection services, Action<SessionRedisCacheOptions> setupAction)
    {
        services.AddSessionRedisCache(setupAction);
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, HttpContextCurrentUserAccessor>();
        services.AddScoped<ISessionUserAccessor, SessionUserAccessor>();
        return services;
    }
}