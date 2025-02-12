using Hospital.Fw.Application;
using Hospital.Fw.Application.Contract;
using Hospital.Fw.Domain.Shared.Core.DynamicProxy;
using Hospital.Fw.Interceptor.Logging.Logging;

namespace Hospital.Fw.Test.TestServices;

public interface ITestAppService : IBaseAppService, IDynamicProxyEnabled
{
    Task<string> GetAsync();

    Task<(string, string)> GetAsync2((List<string>,string) a);
}

public class TestAppService : BaseAppService, ITestAppService
{
    public TestAppService()
    {

      
    }

    [Logging("Hospital.Fw.Test.TestServices.GetAsync")]
    public async Task<string> GetAsync()
    {
        var d = ServiceProvider.GetService<IEnumerator<IFwInterceptor>>();

        var logger = ServiceProvider.GetRequiredService<ILogger<ITestAppService>>();
        logger.LogInformation("Hello World!");

       

        return await Task.FromResult("Hello World!");
    }

    [Logging("Hospital.Fw.Test.TestServices.GetAsync2")]
    public Task<(string, string)> GetAsync2((List<string>, string) a)
    {
        return Task.FromResult((a.Item1.First(), a.Item2));
    }
}