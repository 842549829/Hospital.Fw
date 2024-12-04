using Microsoft.AspNetCore.Builder;

namespace Hospital.Fw.HttpApi.Middleware;

/// <summary>
/// 认证扩展类
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// 启用认证中间件
    /// </summary>
    /// <param name="builder">builder</param>
    /// <returns>IApplicationBuilder</returns>
    public static IApplicationBuilder UseSessionAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthenticationMiddleware>();
    }
}