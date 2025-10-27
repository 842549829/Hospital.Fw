using Hospital.Fw.Application.Tests.AppService;
using Hospital.Fw.TestBase.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Fw.Application.Tests
{
    public class UnitTest1 : InterceptionTestBase
    {
        protected override void InitAssemblies()
        {
        }


        [Fact]
        public void Test1()
        {
            ITestAppService testAppService = ServiceProvider.GetRequiredService<ITestAppService>();
            testAppService.Test();
            Assert.True(true);
        }
    }
}