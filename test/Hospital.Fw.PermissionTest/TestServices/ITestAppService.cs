using Hospital.Fw.Application;
using Hospital.Fw.Application.Contract;
using Hospital.Fw.Domain.Shared.Core.DynamicProxy;

namespace Hospital.Fw.PermissionTest.TestServices;

public interface ITestAppService : IBaseAppService
{
    Task<string> GetAsync();

    Task<(string, string)> GetAsync2((List<string>,string) a);
}

public class TestAppService : BaseAppService, ITestAppService
{
    public async Task<string> GetAsync()
    {
        var d = ServiceProvider.GetService<IEnumerator<IFwInterceptor>>();

        var logger = ServiceProvider.GetRequiredService<ILogger<ITestAppService>>();
        logger.LogInformation("Hello World!");

       

        return await Task.FromResult("Hello World!");
    }

    public Task<(string, string)> GetAsync2((List<string>, string) a)
    {
        return Task.FromResult((a.Item1.First(), a.Item2));
    }
}