using System.Reflection;

namespace Hospital.Fw.Domain.Shared.Core.DynamicProxy;

public interface IFwMethodInvocation
{
    object[] Arguments { get; }

    IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }

    Type[] GenericArguments { get; }

    object TargetObject { get; }

    MethodInfo Method { get; }

    object ReturnValue { get; set; }

    Task ProceedAsync();
}