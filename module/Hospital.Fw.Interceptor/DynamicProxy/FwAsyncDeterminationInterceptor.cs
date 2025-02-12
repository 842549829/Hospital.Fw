using Castle.DynamicProxy;
using Hospital.Fw.Domain.Shared.Core.DynamicProxy;

namespace Hospital.Fw.Interceptor.DynamicProxy;

public class FwAsyncDeterminationInterceptor(IEnumerable<IFwInterceptor> interceptor)
    : AsyncDeterminationInterceptor(new CastleAsyncFwInterceptorAdapter(interceptor));
