using Hospital.Fw.Test.TestServices;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Fw.Test.Controllers;

[ApiController]
[Route("/api/home")]
public class HomeController(ILogger<HomeController> logger, ITestAppService testAppService) : ControllerBase
{
    [HttpGet]
    public async Task<string> Get()
    {
        logger.LogInformation("Get");


        var d1 = await testAppService.GetAsync2((new List<string> { "111", "x" }, "drferfe "));

        return await testAppService.GetAsync();
    }
}