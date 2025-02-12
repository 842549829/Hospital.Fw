using Castle.DynamicProxy;
using Hospital.Fw.Domain.Shared.Core.DynamicProxy;

namespace Hospital.Fw.Interceptor.DynamicProxy;

public class CastleAsyncFwInterceptorAdapter(IEnumerable<IFwInterceptor> interceptor) : AsyncInterceptorBase
{
    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
    {
        foreach (var item in interceptor)
        {
            await item.InterceptAsync(
                new CastleFwMethodInvocationAdapter(invocation, proceedInfo, proceed)
            );
        }
    }

    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
    {
        var adapter = new CastleFwMethodInvocationAdapterWithReturnValue<TResult>(invocation, proceedInfo, proceed);
        foreach (var item in interceptor)
        {
            await item.InterceptAsync(
                adapter
            );
        }
        return (TResult)adapter.ReturnValue;
    }
}