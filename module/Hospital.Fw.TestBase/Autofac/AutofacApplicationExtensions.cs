using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Fw.TestBase.Autofac;

/// <summary>
/// Autofac 扩展
/// </summary>
public static class AutofacApplicationExtensions
{
    /// <summary>
    /// 注册UseAutofac
    /// </summary>
    /// <param name="services">services</param>
    public static void UseAutofac(this IServiceCollection services)
    {
        services.AddAutofacServiceProviderFactory();
    }

    /// <summary>
    /// AddAutofacServiceProviderFactory
    /// </summary>
    /// <param name="services">services</param>
    /// <returns>services</returns>
    public static IServiceCollection AddAutofacServiceProviderFactory(this IServiceCollection services)
    {
        return services.AddAutofacServiceProviderFactory(new ContainerBuilder());
    }

    /// <summary>
    /// AddAutofacServiceProviderFactory
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="containerBuilder">containerBuilder</param>
    /// <returns>services</returns>
    public static IServiceCollection AddAutofacServiceProviderFactory(this IServiceCollection services, ContainerBuilder containerBuilder)
    {
        var factory = new AutofacServiceProviderFactory(containerBuilder);

        services.AddSingleton((IServiceProviderFactory<ContainerBuilder>)factory);

        return services;
    }
}