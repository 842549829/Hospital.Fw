using Hospital.Fw.Application.Contract;

namespace Hospital.Fw.Application.Tests.AppService;

public interface ITestAppService : IBaseAppService 
{
    void Test();
}

public class TestAppService: BaseAppService, ITestAppService
{
    public void Test() 
    {
        Console.Write("Test");
    }
}
