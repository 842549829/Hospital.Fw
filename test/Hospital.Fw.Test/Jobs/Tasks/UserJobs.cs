using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Domain.Shared.Core.Autofac;
using Hospital.Fw.Test.Jobs.Eto;
using Hospital.Fw.Test.TestServices;

namespace Hospital.Fw.Test.Jobs.Tasks;

public class UserInitJobs(ITestAppService testAppService, ILogger<UserInitJobs> logger) : IAsyncBackgroundJob<UserInitEto>, ITransientDependency
{
    public Task ExecuteAsync(UserInitEto args)
    {
        logger.LogInformation("UserInitJobs is running...args:{args}", args);
        return testAppService.GetAsync();
    }
}

public class UserCreateJobs(ITestAppService testAppService, ILogger<UserCreateJobs> logger) : IAsyncBackgroundJob<UserCreateEto>, ITransientDependency
{
    public Task ExecuteAsync(UserCreateEto args)
    {
        logger.LogInformation("UserCreateJobs is running...args:{args}", args);
        return testAppService.GetAsync();
    }
}

public class UserUpdateJobs(ITestAppService testAppService, ILogger<UserUpdateEto> logger) : IAsyncBackgroundJob<UserUpdateEto>, ITransientDependency
{
    public Task ExecuteAsync(UserUpdateEto args)
    {
        logger.LogInformation("UserUpdateJobs is running...args:{args}", args);
        return testAppService.GetAsync();
    }
}

public class UserDeleteJobs(ITestAppService testAppService, ILogger<UserInitJobs> logger) : IAsyncBackgroundJob<UserDeleteEto>, ITransientDependency
{
    public Task ExecuteAsync(UserDeleteEto args)
    {
        logger.LogInformation("UserDeleteJobs is running...args:{args}", args);
        return testAppService.GetAsync();
    }
}