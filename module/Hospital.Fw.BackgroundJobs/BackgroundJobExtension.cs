using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.BackgroundJobs.Implementations;
using Hospital.Fw.BackgroundJobs.Threading;
using Hospital.Fw.Domain.Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Fw.BackgroundJobs;

public static class BackgroundJobExtension
{
    public static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SystemTextJsonSerializerOptions>(_ => { });
        services.Configure<BackgroundJobOptions>(_ => { });
        services.Configure<BackgroundJobWorkerOptions>(options =>
        {
            configuration.GetSection("BackgroundJobWorker").Bind(options);
        });

        services.AddTransient<AbpAsyncTimer>();
        services.AddTransient<IJsonSerializer, SystemTextJsonSerializer>();
        services.AddTransient<IBackgroundJobSerializer, JsonBackgroundJobSerializer>();
        services.AddTransient<IBackgroundJobStore, InMemoryBackgroundJobStore>();
        services.AddTransient<IBackgroundJobExecuter, BackgroundJobExecuter>();
        services.AddTransient<IBackgroundJobManager, DefaultBackgroundJobManager>();
        services.AddTransient<IExceptionNotifier, TaskExceptionNotifier>();
        services.AddSingleton<IBackgroundJobWorker, BackgroundJobWorker>();
        services.AddSingleton<IBackgroundWorkerManager, BackgroundWorkerManager>();
    }
}