using Hospital.Fw.Permission.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Fw.PermissionTest.Controllers;

[ApiController]
[Route("/api/home")]
public class HomeController(IJwtServices jwtServices) : ControllerBase
{

    [HttpGet("home")]
    [Authorize("A1")]

    public string Get1()
    {
        return "home";
    }

    [HttpGet("login")]
    public string Login()
    {
        var token = jwtServices.CreateToken(new User("1", "admin", "管理员"));
        return token;
    }
}