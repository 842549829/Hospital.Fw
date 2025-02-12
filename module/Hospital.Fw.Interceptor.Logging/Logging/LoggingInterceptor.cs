using Hospital.Fw.Domain.Shared.Core.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hospital.Fw.Interceptor.Logging.Logging;

public class LoggingInterceptor(IServiceScopeFactory serviceScopeFactory) : FwInterceptor
{
    public override async Task InterceptAsync(IFwMethodInvocation invocation)
    {
        if (!LoggingHelper.IsLoggingMethod(invocation.Method, out var loggingAttribute) || loggingAttribute == null)
        {
            await invocation.ProceedAsync();
            return;
        }

        using var scope = serviceScopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetService<ILogger<LoggingInterceptor>>();
        if (logger == null)
        {
            await invocation.ProceedAsync();
            return;
        }

        await invocation.ProceedAsync();

        logger.Log(loggingAttribute.LogLevel, "Name:{Name}\r\nArguments:{Arguments}\r\nReturnValue:{ReturnValue}", loggingAttribute.Name, invocation.Arguments, invocation.ReturnValue);
    }
}