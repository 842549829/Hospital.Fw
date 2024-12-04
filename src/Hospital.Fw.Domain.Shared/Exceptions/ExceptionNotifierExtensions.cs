using Microsoft.Extensions.Logging;

namespace Hospital.Fw.Domain.Shared.Exceptions;

public static class ExceptionNotifierExtensions
{
    public static Task NotifyAsync(
        this IExceptionNotifier exceptionNotifier,
         Exception exception,
        LogLevel logLevel = LogLevel.Error,
        bool handled = true)
    {

        return exceptionNotifier.NotifyAsync(
            new ExceptionNotificationContext(
                exception,
                logLevel,
                handled
            )
        );
    }
}