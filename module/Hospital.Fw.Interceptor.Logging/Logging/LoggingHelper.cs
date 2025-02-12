using Hospital.Fw.Domain.Shared.Core;
using System.Reflection;

namespace Hospital.Fw.Interceptor.Logging.Logging;

public static class LoggingHelper
{
    public static bool IsLoggingType(TypeInfo implementationType)
    {
        if (HasLoggingAttribute(implementationType) || AnyMethodHasLoggingAttribute(implementationType))
        {
            return true;
        }

        if (typeof(ILoggingEnabled).GetTypeInfo().IsAssignableFrom(implementationType))
        {
            return true;
        }

        return false;
    }

    public static bool IsLoggingMethod(MethodInfo methodInfo, out LoggingAttribute? loggingAttribute)
    {
        Check.NotNull(methodInfo, nameof(methodInfo));

        var attrs = methodInfo.GetCustomAttributes(true).OfType<LoggingAttribute>().ToArray();
        if (attrs.Any())
        {
            loggingAttribute = attrs.First();
            return !loggingAttribute.IsDisabled;
        }

        if (methodInfo.DeclaringType != null)
        {
            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<LoggingAttribute>()
                .ToArray();
            if (attrs.Any())
            {
                loggingAttribute = attrs.First();
                return !loggingAttribute.IsDisabled;
            }

            if (typeof(LoggingAttribute).GetTypeInfo().IsAssignableFrom(methodInfo.DeclaringType))
            {
                loggingAttribute = null;
                return true;
            }
        }

        loggingAttribute = null;
        return false;
    }

    private static bool AnyMethodHasLoggingAttribute(TypeInfo implementationType)
    {
        return implementationType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(HasLoggingAttribute);
    }

    private static bool HasLoggingAttribute(MemberInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(LoggingAttribute), true);
    }
}