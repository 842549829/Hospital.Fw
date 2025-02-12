namespace Hospital.Fw.Domain.Shared.Core.DynamicProxy;

public abstract class FwInterceptor : IFwInterceptor
{
    public abstract Task InterceptAsync(IFwMethodInvocation invocation);
}