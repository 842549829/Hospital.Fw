using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Fw.Domain.Shared.Custom.Caching
{
    public static class SessionRedisCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddSessionRedisCache(this IServiceCollection services, Action<SessionRedisCacheOptions> setupAction)
        {
            services.AddOptions();

            services.Configure(setupAction);
            services.Add(ServiceDescriptor.Singleton<IDistributedSessionCache, DistributedSessionCache>());

            return services;
        }
    }
}
