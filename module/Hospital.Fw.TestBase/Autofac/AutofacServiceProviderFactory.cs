using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Fw.TestBase.Autofac;

/// <summary>
/// AutofacServiceProviderFactory
/// </summary>
public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    /// <summary>
    /// ContainerBuilder
    /// </summary>
    private readonly ContainerBuilder _builder;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="builder">builder</param>
    public AutofacServiceProviderFactory(ContainerBuilder builder)
    {
        _builder = builder;
    }

    /// <summary>
    /// Creates a container builder from an <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
    /// </summary>
    /// <param name="services">The collection of services</param>
    /// <returns>A container builder that can be used to create an <see cref="T:System.IServiceProvider" />.</returns>
    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        _builder.Populate(services);

        return _builder;
    }

    /// <summary>
    /// CreateServiceProvider
    /// </summary>
    /// <param name="containerBuilder">containerBuilder</param>
    /// <returns>IServiceProvider</returns>
    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        return new AutofacServiceProvider(containerBuilder.Build());
    }
}
