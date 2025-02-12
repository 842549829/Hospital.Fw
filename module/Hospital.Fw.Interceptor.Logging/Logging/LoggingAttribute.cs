using Microsoft.Extensions.Logging;

namespace Hospital.Fw.Interceptor.Logging.Logging;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class LoggingAttribute(string methodName) : Attribute
{
    public string Name { get; set; } = methodName;

    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    public bool IsDisabled { get; set; }
}