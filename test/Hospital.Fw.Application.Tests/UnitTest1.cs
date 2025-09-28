using Hospital.Fw.Application.Tests.AppService;
using Hospital.Fw.Domain.Shared.Core;
using Hospital.Fw.TestBase.Tests;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hospital.Fw.Application.Tests
{
    public class UnitTest1 : InterceptionTestBase
    {
        public override void InitAssemblies()
        {
            LoadAssemblies.AssembliesStartingWith =
  [
      Assembly.Load("Hospital.Fw.Application"),
        Assembly.Load("Hospital.Fw.Application.Contract"),
        Assembly.Load("Hospital.Fw.Domain"),
        Assembly.Load("Hospital.Fw.Domain.Shared"),
        Assembly.Load("Hospital.Fw.TestBase"),
        Assembly.Load("Hospital.Fw.Application.Tests")
    ];
        }


        [Fact]
        public void Test1()
        {
            var testAppService = ServiceProvider.GetRequiredService<ITestAppService>();
            testAppService.Test();
            Assert.True(true);
        }
    }
}