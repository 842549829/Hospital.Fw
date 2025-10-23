using System.Net;
using System.Text.Json;
using Hospital.Fw.Domain.Shared.Core;
using Hospital.Fw.Domain.Shared.Core.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Template.HttpApi.Middleware;

/// <summary>
/// 异常中间件
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="next">next</param>
    /// <param name="logger">logger</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// 执行中间件
    /// </summary>
    /// <param name="context">context</param>
    /// <returns>Task</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "错误路径:{path}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// 异常处理
    /// </summary>
    /// <param name="context">context</param>
    /// <param name="exception">exception</param>
    /// <returns>Task</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = null };
        context.Response.ContentType = "application/json";
        if (exception is CustomException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var result = new ErrorResult(exception.Message);
            return context.Response.WriteAsJsonAsync(result, jsonSerializerOptions);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var result = new ErrorResult("服务器内部错误");
            return context.Response.WriteAsJsonAsync(result, jsonSerializerOptions);
        }
    }
} 