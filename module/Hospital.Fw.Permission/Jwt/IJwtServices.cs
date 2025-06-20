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
    string CreateToken(User user);

    /// <summary>
    /// 解析Token
    /// </summary>
    /// <param name="token">token</param>
    /// <returns>ClaimsPrincipal</returns>
    ClaimsPrincipal? ParseToken(string token);
}