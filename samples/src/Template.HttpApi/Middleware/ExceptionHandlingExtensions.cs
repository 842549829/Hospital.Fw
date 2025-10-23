using Microsoft.AspNetCore.Builder;

namespace Template.HttpApi.Middleware;

/// <summary>
/// 启用异常中间件扩展方法
/// </summary>
public static class ExceptionHandlingExtensions
{
    /// <summary>
    /// 启用认证中间件
    /// </summary>
    /// <param name="builder">builder</param>
    /// <returns>IApplicationBuilder</returns>
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}