using Autofac;
using Hospital.Fw.TestBase.Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Fw.TestBase.Tests;

/// <summary>
/// 测试基类
/// </summary>
public abstract class InterceptionTestBase
{
    /// <summary>
    /// ServiceProvider
    /// </summary>
    protected IServiceProvider ServiceProvider { get; set; } = null!;

    /// <summary>
    /// 构造函数
    /// </summary>
    protected InterceptionTestBase()
    {
        InitAssemblies();
        Initialize();
    }

    /// <summary>
    /// 初始化程序集
    /// </summary>
    protected abstract void InitAssemblies();

    /// <summary>
    /// CreateServiceCollection
    /// </summary>
    /// <returns>IServiceCollection</returns>
    protected virtual IServiceCollection CreateServiceCollection()
    {
        return new ServiceCollection();
    }

    protected virtual void Initialize()
    {
        // 1. 创建服务集合
        var services = CreateServiceCollection();

        // 2. 使用 Autofac 工厂（注册 IServiceProviderFactory）
        services.UseAutofac();

        // 3. 获取工厂（用于创建 Autofac 容器）
        var factory = GetServiceProviderFactory(services);

        // 4. 【关键】先让子类注册服务！
        RegisterServices(services);

        // 5. 创建 ContainerBuilder，并将 IServiceCollection 内容迁移到 Autofac
        var containerBuilder = factory.CreateBuilder(services);

        // 6. 可选：注册 Autofac 模块（如 TestAutofacModule）
        containerBuilder.RegisterModule<TestAutofacModule>();

        // 7. 构建基于 Autofac 的 ServiceProvider
        var serviceProvider = factory.CreateServiceProvider(containerBuilder);

        // 8. 保存服务提供者
        ServiceProvider = serviceProvider;

        // 9. 初始化（此时可安全解析服务）
        OnServicesInitialized();
    }

    /// <summary>
    /// 获取 Autofac 的服务提供者工厂
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <returns>IServiceProviderFactory<ContainerBuilder></returns>
    private static IServiceProviderFactory<ContainerBuilder> GetServiceProviderFactory(IServiceCollection services)
    {
        // 注意：这里会触发一个临时 ServiceProvider，仅用于获取 factory
        return services.BuildServiceProvider()
                      .GetRequiredService<IServiceProviderFactory<ContainerBuilder>>();
    }

    /// <summary>
    /// 在此方法中注册你的服务（使用 services.Add...）
    /// 此方法在容器构建前调用，安全有效。
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    protected virtual void RegisterServices(IServiceCollection services)
    {
        // 示例：
        // services.AddSingleton<IMyService, MyService>();
        // services.AddScoped<IUserService, UserService>();
    }

    /// <summary>
    /// 容器构建完成后调用，可用于解析服务、触发初始化逻辑等。
    /// </summary>
    protected virtual void OnServicesInitialized()
    {
        // 此时可以安全解析服务
        // var myService = ServiceProvider.GetRequiredService<IMyService>();
    }
}
