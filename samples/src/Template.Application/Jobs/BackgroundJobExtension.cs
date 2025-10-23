using Hospital.Fw.BackgroundJobs.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Jobs.Tasks;

namespace Template.Application.Jobs;

public static class BackgroundJobExtension
{
    public static void AddBackgroundJobTasks(this IServiceCollection services)
    {
        // 配置任务持久化层
        services.AddTransient<IBackgroundJobStore, BackgroundJobStore>();

        // 添加任务
        services.AddTransient<UserInitJobs>();
        services.Configure<BackgroundJobOptions>(options =>
        {
            options.AddJob<UserInitJobs>();
        });
    }
}