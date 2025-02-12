namespace Hospital.Fw.Domain.Shared.Core.DynamicProxy;

public interface IFwInterceptor
{
    Task InterceptAsync(IFwMethodInvocation invocation);
}