using Hospital.Fw.Application;
using Hospital.Fw.Application.Contract;

namespace Hospital.Fw.Test.TestServices;

public interface ITestAppService : IBaseAppService
{
    Task<string> GetAsync();
}

public class TestAppService : BaseAppService, ITestAppService
{
    public Task<string> GetAsync()
    {
        var logger = ServiceProvider.GetRequiredService<ILogger<ITestAppService>>();
        logger.LogInformation("Hello World!");
        return Task.FromResult("Hello World!");
    }
}