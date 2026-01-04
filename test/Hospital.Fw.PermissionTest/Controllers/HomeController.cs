using System.IdentityModel.Tokens.Jwt;
using Hospital.Fw.Permission.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Fw.PermissionTest.Controllers;

public class RefreshTokenRequest
{
    /// <summary>
    /// RefreshToken
    /// </summary>
    public required string RefreshToken { get; set; }
}

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
        var token = jwtServices.GenerateAccessToken(new User("1", "admin", "管理员", "tid", "oid"));
        return token;
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshTokenRequest request)
    {
        // 1. 验证提供的 Refresh Token
        var principal = jwtServices.ValidateRefreshToken(request.RefreshToken);

        if (principal == null)
        {
            // Refresh Token 无效或已过期
            return Unauthorized("Invalid refresh token.");
        }

        // 2. 从验证通过的 Token 中提取用户信息
        var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            // Token 结构异常
            return Unauthorized("Invalid refresh token payload.");
        }

        // 3. (可选) 这里可以再次验证用户是否仍然有效 (例如，检查用户是否被禁用)
        // if (!IsUserStillValid(userId))
        // {
        //     return Unauthorized("User account is no longer valid.");
        // }

        // 4. 生成新的 Access Token 和新的 Refresh Token
        // 注意：这里可以决定是否生成新的 Refresh Token，或者沿用旧的。
        // 方案一中，我们生成新的 Refresh Token，旧的自然过期。
        var user = new User("1", "admin", "管理员", "tid", "oid");
        var newAccessToken = jwtServices.GenerateAccessToken(user); // 假设角色信息也从 principal 或数据库获取
        var newRefreshToken = jwtServices.GenerateRefreshToken(user);

        // 5. 返回新的 Token
        return Ok(new
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenType = "Bearer",
            ExpiresIn = 15 * 60 // Access Token 过期秒数 (15分钟)
        });
    }
}