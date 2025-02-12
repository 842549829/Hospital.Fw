using System.Reflection;
using Autofac;
using Hospital.Fw.Domain.Shared.Core;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Hospital.Fw.Domain.Shared.Core.DynamicProxy;
using Module = Autofac.Module;

namespace Hospital.Fw.Interceptor.DynamicProxy;

public class DynamicProxyAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterDynamicProxy(builder);
    }

    public static ContainerBuilder RegisterDynamicProxy(ContainerBuilder builder)
    {
        // 获取当前程序集或指定程序集
        var assemblies = GetAssemblies();

        // 注入同步拦截器
        builder.RegisterAssemblyTypes(assemblies)
            .Where(a => typeof(IInterceptor).IsAssignableFrom(a));

        // 注入异步拦截器
        builder.RegisterAssemblyTypes(assemblies)
            .Where(a => typeof(IAsyncInterceptor).IsAssignableFrom(a))
            .As<IAsyncInterceptor>()
            .InstancePerLifetimeScope();

        // 注入元拦截器接口
        builder.RegisterAssemblyTypes(assemblies)
            .Where(a => typeof(IFwInterceptor).IsAssignableFrom(a))
            .AsImplementedInterfaces();

        // 注入集成IDynamicProxyEnabled的方法的拦截器
        builder.RegisterAssemblyTypes(assemblies)
            .Where(a => typeof(IDynamicProxyEnabled).IsAssignableFrom(a)
                        && a is { IsClass: true, IsAbstract: false })
            .As(t => t.GetInterfaces())
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(FwAsyncDeterminationInterceptor));

        return builder;
    }

    private static Assembly[] GetAssemblies()
    {
        var assemblies = LoadAssemblies.AssembliesStartingWith;
        return assemblies.ToArray();
    }
}