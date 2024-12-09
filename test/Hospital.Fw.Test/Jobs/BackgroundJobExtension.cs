using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Test.Jobs.Eto;
using Hospital.Fw.Test.Jobs.Tasks;

namespace Hospital.Fw.Test.Jobs;

public static class BackgroundJobExtension
{
    public static void AddBackgroundJobTasks(this IServiceCollection services)
    {
        // 配置任务持久化层
        services.AddTransient<IBackgroundJobStore, BackgroundJobStore>();

        // 添加任务
        services.AddTransient<UserInitJobs>();
        services.AddTransient<UserCreateJobs>();
        services.AddTransient<UserUpdateJobs>();
        services.AddTransient<UserDeleteJobs>();
        services.Configure<BackgroundJobOptions>(options =>
        {
            options.AddJob<UserInitJobs>();
            options.AddJob<UserCreateJobs>();
            options.AddJob<UserUpdateJobs>();
            options.AddJob<UserDeleteJobs>();
        });
    }
}