
using Microsoft.Extensions.Logging;

namespace Hospital.Fw.Domain.Shared.Exceptions;

public class ExceptionNotificationContext(
    Exception exception,
    LogLevel logLevel,
    bool handled = true)
{
    /// <summary>
    /// The exception object.
    /// </summary>
    public Exception Exception { get; } = exception;

    public LogLevel LogLevel { get; } = logLevel;

    /// <summary>
    /// True, if it is handled.
    /// </summary>
    public bool Handled { get; } = handled;
}