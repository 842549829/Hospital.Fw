using Castle.DynamicProxy;
using Hospital.Fw.Domain.Shared.Core.DynamicProxy;

namespace Hospital.Fw.Interceptor.DynamicProxy;

public class CastleFwMethodInvocationAdapter(
    IInvocation invocation,
    IInvocationProceedInfo proceedInfo,
    Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    : CastleFwMethodInvocationAdapterBase(invocation), IFwMethodInvocation
{
    protected IInvocationProceedInfo ProceedInfo { get; } = proceedInfo;

    protected Func<IInvocation, IInvocationProceedInfo, Task> Proceed { get; } = proceed;

    public override async Task ProceedAsync()
    {
        await Proceed(Invocation, ProceedInfo);
    }
}