using Microsoft.AspNetCore.Http;

namespace Hospital.Fw.HttpApi.Middleware;

/// <summary>
/// 授权中间件
/// </summary>
public class AuthenticationMiddleware(RequestDelegate next)
{
    /// <summary>
    /// InvokeAsync
    /// </summary>
    /// <param name="context">context</param>
    /// <returns>Task</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var swaggers = new[] { "/swagger/v1/swagger.json", "/swagger/index.html" };
        if (swaggers.Any(d => d == context.Request.Path))
        {
            await next(context);
        }
        else
        {
            //var currentUser = context.RequestServices.GetRequiredService<ICurrentUser>();

            //// 检查用户是否已登录
            //if (!currentUser.SessionUserInfo.IsAuthenticated)
            //{
            //    // TODO 需要优化兼容以前的登录方式
            //    //context.Response.StatusCode = 401;
            //    //await context.Response.WriteAsync("用户未登录");
            //    //return;
            //}
            await next(context);
        }
    }
}