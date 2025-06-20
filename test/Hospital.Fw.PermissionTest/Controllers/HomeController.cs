using Hospital.Fw.Permission.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Fw.PermissionTest.Controllers;

[ApiController]
[Route("/api/home")]
public class HomeController(IJwtServices jwtServices) : ControllerBase
{

    [HttpGet("home")]
    [Authorize()]

    public string Get1()
    {
        return "home";
    }

    [HttpGet("login")]
    public string Login()
    {
        var token = jwtServices.CreateToken(new User
        {

            Id = "1",
            UserName = "admin",
            NickName = "管理员",
            Avatar = "https://avatar.csdn.net/5/E/C/3_qq_32183901.jpg",
            Email = "admin@admin.com",
            Phone = "12345678901",
            Roles = new string[] { "A1", "A2" },
            Permissions = new string[] { "A1", "A2" }
        });
        return token;
    }
}