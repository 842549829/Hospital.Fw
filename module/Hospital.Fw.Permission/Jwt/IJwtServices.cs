using Hospital.Fw.Domain.Shared.Core.Autofac;
using System.Security.Claims;

namespace Hospital.Fw.Permission.Jwt;

/// <summary>
/// Jwt服务
/// </summary>
public interface IJwtServices : ISingletonDependency
{
    /// <summary>
    /// 创建Token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns>token</returns>
    string GenerateAccessToken(User user);

    /// <summary>
    /// 解析Token
    /// </summary>
    /// <param name="token">token</param>
    /// <returns>ClaimsPrincipal</returns>
    ClaimsPrincipal? ValidateAccessToken(string token);

    /// <summary>
    /// 生成刷新Token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns>刷新Token</returns>
    string GenerateRefreshToken(User user);

    /// <summary>
    /// 验证 Refresh Token (也是一个 JWT 验证)
    /// </summary>
    /// <param name="refreshToken">刷新token</param>
    /// <returns>ClaimsPrincipal 或 null（验证失败）</returns>
    ClaimsPrincipal? ValidateRefreshToken(string refreshToken);
}