using Hospital.Fw.Test.TestServices;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Fw.Test.Controllers;

[ApiController]
[Route("/api/home")]
public class HomeController(ILogger<HomeController> logger, ITestAppService testAppService) : ControllerBase
{
    [HttpGet]
    public Task<string> Get()
    {
        logger.LogInformation("Get");
        return testAppService.GetAsync();
    }
}