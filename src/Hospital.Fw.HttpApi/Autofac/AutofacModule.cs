using Autofac;
using Hospital.Fw.Domain.Shared.Core;
using Hospital.Fw.Domain.Shared.Core.Autofac;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Module = Autofac.Module;

namespace Hospital.Fw.HttpApi.Autofac;

/// <summary>
/// AutofacModule
/// </summary>
public class AutofacModule : Module
{
    /// <summary>
    /// 加载
    /// </summary>
    /// <param name="builder">builder</param>
    protected override void Load(ContainerBuilder builder)
    {
        // 获取当前程序集或指定程序集
        var assemblies = GetAssembliesStartingWith();

        // 自动注册相关类型服务
        RegisterType(builder, assemblies, typeof(IDependency));

        // 注册控制器
        RegisterController(builder, assemblies);
    }

    private static void RegisterController(ContainerBuilder builder, Assembly[] assemblies)
    {
        // 获取所有控制器类型并使用属性注入
        var controllerBaseType = typeof(ControllerBase);
        builder.RegisterAssemblyTypes(assemblies)
            .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            .PropertiesAutowired(new AutowiredPropertySelector());
    }

    private static void RegisterType(ContainerBuilder builder, Assembly[] assemblies, Type type)
    {
        foreach (var assembly in assemblies)
        {
            RegisterType(builder, assembly, type);
        }
    }

    private static void RegisterType(ContainerBuilder builder, Assembly assembly, Type type)
    {
        // 查找所有实现了 ITransientDependency 接口的类型
        var baseAppServiceTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && type.IsAssignableFrom(t));
        foreach (var serviceType in baseAppServiceTypes)
        {
            // 注册为 IBaseAppService
            RegisterType(builder, type, serviceType);

            // 找出该类型实现的所有接口
            var implementedInterfaces = serviceType.GetInterfaces();

            // 注册为所有实现的接口
            foreach (var interfaceType in implementedInterfaces)
            {
                if (interfaceType != type)
                {
                    RegisterType(builder, interfaceType, serviceType);
                }
            }
        }
    }

    private static void RegisterType(ContainerBuilder builder, Type type, Type serviceType)
    {
        if (typeof(ISingletonDependency).IsAssignableFrom(serviceType))
        {
            builder
                .RegisterType(serviceType)
                .As(type)
                .PropertiesAutowired(new AutowiredPropertySelector())
                .SingleInstance();
        }
        else if (typeof(IScopedDependency).IsAssignableFrom(serviceType))
        {
            builder
                .RegisterType(serviceType)
                .As(type)
                .PropertiesAutowired(new AutowiredPropertySelector())
                .InstancePerLifetimeScope();
        }
        else
        {
            builder
                .RegisterType(serviceType)
                .As(type)
                .PropertiesAutowired(new AutowiredPropertySelector())
                .InstancePerDependency();
        }
    }

    private static Assembly[] GetAssembliesStartingWith()
    {
        return LoadAssemblies.AssembliesStartingWith.ToArray();
    }
}