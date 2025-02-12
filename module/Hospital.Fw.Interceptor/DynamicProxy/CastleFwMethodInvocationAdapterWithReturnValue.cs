using Castle.DynamicProxy;
using Hospital.Fw.Domain.Shared.Core.DynamicProxy;

namespace Hospital.Fw.Interceptor.DynamicProxy;

public class CastleFwMethodInvocationAdapterWithReturnValue<TResult>(
    IInvocation invocation,
    IInvocationProceedInfo proceedInfo,
    Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    : CastleFwMethodInvocationAdapterBase(invocation), IFwMethodInvocation
{
    protected IInvocationProceedInfo ProceedInfo { get; } = proceedInfo;

    protected Func<IInvocation, IInvocationProceedInfo, Task<TResult>> Proceed { get; } = proceed;

    public override async Task ProceedAsync()
    {
        ReturnValue = (await Proceed(Invocation, ProceedInfo))!;
    }
}