# Hospital.Fw

## 简介

Hospital.Fw 是一个专为医疗行业设计的轻量级、模块化的 .NET 应用程序框架。它提供了一套完整的基础设施和工具，帮助开发者快速构建高质量、可维护的医疗信息系统。

## 功能特性

- **模块化架构**：基于模块化设计，便于扩展和维护
- **依赖注入**：集成 Autofac 实现依赖注入，简化组件管理
- **日志系统**：集成 Serilog 提供全面的日志记录功能
- **后台任务**：内置后台任务处理系统，支持定时任务和异步处理
- **权限管理**：提供灵活的权限控制和认证授权机制
- **数据访问**：集成 SqlSugar ORM，简化数据库操作
- **API 文档**：自动生成 Swagger API 文档
- **异常处理**：统一的异常处理机制
- **拦截器**：支持 AOP 编程，提供日志、序列号等拦截器
- **会话管理**：提供用户会话管理功能

## 项目结构

```
├── custom/                     # 自定义扩展模块
│   ├── Hospital.Fw.Domain.Shared.Custom/  # 自定义领域共享模块
│   └── Hospital.Fw.HttpApi.Custom/        # 自定义 HTTP API 模块
├── module/                     # 功能模块
│   ├── Hospital.Fw.BackgroundJobs/        # 后台任务模块
│   ├── Hospital.Fw.Interceptor/           # 拦截器基础模块
│   ├── Hospital.Fw.Interceptor.Logging/   # 日志拦截器模块
│   ├── Hospital.Fw.Interceptor.Sequence/  # 序列号拦截器模块
│   ├── Hospital.Fw.Mo.Logging/            # 日志模块
│   ├── Hospital.Fw.Permission/            # 权限模块
│   └── Hospital.Fw.TestBase/              # 测试基础模块
├── src/                        # 核心源代码
│   ├── Hospital.Fw.Application/           # 应用层
│   ├── Hospital.Fw.Application.Contract/   # 应用层契约
│   ├── Hospital.Fw.Domain/                # 领域层
│   ├── Hospital.Fw.Domain.Shared/         # 领域共享层
│   ├── Hospital.Fw.HttpApi/               # HTTP API 层
│   └── Hospital.Fw.SqlSugarCore/          # 数据访问层
├── test/                       # 测试项目
│   ├── Hospital.Fw.Application.Tests/     # 应用层测试
│   ├── Hospital.Fw.PermissionTest/        # 权限测试
│   └── Hospital.Fw.Test/                  # 综合测试
└── samples/                    # 示例项目
    └── src/                    # 示例源代码
        ├── Template.Application/          # 示例应用层
        ├── Template.Application.Contract/  # 示例应用层契约
        ├── Template.Domain/               # 示例领域层
        ├── Template.Domain.Shared/        # 示例领域共享层
        ├── Template.Host/                 # 示例宿主应用
        ├── Template.HttpApi/              # 示例 HTTP API 层
        └── Template.SqlSugarCore/         # 示例数据访问层
```

## SLNX 解决方案文件

本项目使用了 .NET 9 引入的新型 SLNX 解决方案文件格式，相比传统的 SLN 文件，SLNX 具有以下优势：

### SLNX 文件说明

SLNX 是一种基于 XML 的新型解决方案文件格式，用于替代传统的 SLN 文件。它采用了更简洁、更易读的 XML 结构，使解决方案文件更易于理解和维护。

```xml
<Solution>
  <Folder Name="/01-Solution Items/">
    <File Path="README.md" />
    <!-- 其他解决方案项 -->
  </Folder>
  <Folder Name="/02-src/">
    <Project Path="src/Hospital.Fw.Application/Hospital.Fw.Application.csproj" />
    <!-- 其他项目引用 -->
  </Folder>
  <!-- 其他文件夹 -->
</Solution>
```

### 为什么从 SLN 升级到 SLNX

1. **更简洁的格式**：SLNX 使用 XML 格式，结构清晰，易于阅读和理解
2. **更好的版本控制兼容性**：减少了合并冲突，特别是在多人协作的大型项目中
3. **更易于手动编辑**：相比 SLN 文件中的 GUID 引用，SLNX 使用直观的路径引用
4. **更好的工具支持**：支持 .NET CLI 和主流 IDE（如 Visual Studio、Rider）
5. **更少的冗余信息**：移除了不必要的元数据，使文件更加精简

### 使用要求

- .NET 9.0.200 SDK 或更高版本
- 支持 SLNX 的 IDE（如 Visual Studio 2022 预览版）

### 兼容性说明

虽然 SLNX 是 .NET 9 引入的新格式，但它与现有工具链保持了良好的兼容性。如果您使用的工具尚不支持 SLNX，您仍然可以使用传统的 SLN 格式。

## 集中管理 NuGet 包版本

本项目使用 .NET 的 `ManagePackageVersionsCentrally` 功能来集中管理所有 NuGet 包的版本，这种方式有以下优势：

### 集中版本管理说明

在项目根目录的 `Directory.Packages.props` 文件中，我们启用了集中版本管理并定义了所有 NuGet 包的版本：

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  
  <ItemGroup>
    <!-- 共享或不随框架变化的包 -->
    <PackageVersion Include="Autofac.Extras.DynamicProxy" Version="7.1.0" />
    <PackageVersion Include="FluentValidation.DependencyInjectionExtensions" Version="12.0.0" />
    <!-- 其他包版本定义 -->
  </ItemGroup>
  
  <!-- 针对特定 .NET 版本的包 -->
  <ItemGroup Condition="'$(TargetFramework)' == 'net8.0'">
    <PackageVersion Include="Microsoft.Extensions.Configuration.Abstractions" Version="9.0.8" />
    <!-- 其他 .NET 8.0 特定包 -->
  </ItemGroup>
</Project>
```

### 使用方式

在项目文件（.csproj）中引用 NuGet 包时，无需指定版本号：

```xml
<ItemGroup>
  <PackageReference Include="Serilog.AspNetCore" />
  <PackageReference Include="Swashbuckle.AspNetCore" />
</ItemGroup>
```

### 集中管理的优势

1. **版本一致性**：确保整个解决方案中使用相同版本的包，避免版本冲突
2. **简化维护**：升级依赖包只需在一个地方修改版本号
3. **条件引用**：可以根据目标框架（如 net8.0、net9.0）指定不同的包版本
4. **更清晰的项目文件**：项目文件中不再包含版本信息，使其更加简洁
5. **更好的版本控制**：减少因包版本更新导致的合并冲突

## 安装与配置

### 前提条件

- .NET 8.0 或更高版本 
- 支持的数据库（如 SQL Server、MySQL 等）

### NuGet 包引用

```xml
<!-- 在项目中添加以下引用 -->
<ItemGroup>
  <ProjectReference Include="path/to/Hospital.Fw.HttpApi/Hospital.Fw.HttpApi.csproj" />
  <ProjectReference Include="path/to/Hospital.Fw.SqlSugarCore/Hospital.Fw.SqlSugarCore.csproj" />
</ItemGroup>
```

### 基本配置

在 `Program.cs` 中进行框架配置：

```csharp
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hospital.Fw.BackgroundJobs;
using Hospital.Fw.Domain.Shared.Core;
using Hospital.Fw.HttpApi.Autofac;
using Hospital.Fw.HttpApi.Filter;
using Hospital.Fw.HttpApi.Middleware;
using Serilog;

// 配置 Serilog 日志
Log.Logger = new LoggerConfiguration()
    .CreateLogger();

// 创建 Web 应用构建器
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 配置 Serilog
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

// 加载程序集
LoadAssemblies.AssembliesStartingWith =
[
    // 添加需要加载的程序集
    Assembly.Load("YourApplication.Application"),
    // ...
];

// 配置 Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<AutofacModule>();
});

// 添加事务过滤器
builder.Services.AddTransactionFilter();

// 添加控制器
builder.Services.AddControllers()
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// 配置 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 添加 SqlSugar
builder.Services.AddSqlSugar(builder.Configuration);

// 添加后台任务
builder.Services.AddBackgroundJobs(builder.Configuration);

// 构建应用
WebApplication app = builder.Build();

// 配置中间件
app.UseExceptionHandling();

// 配置 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 映射控制器
app.MapControllers();

// 运行应用
app.Run();
```

## 使用指南

### 依赖注入

根据所需的生命周期，实现相应的依赖注入接口以自动注册服务：

- `ITransientDependency`：瞬时生命周期（每次请求创建新实例）
- `ISingletonDependency`：单例生命周期（整个应用程序生命周期内共享一个实例）
- `IScopedDependency`：范围生命周期（在同一个请求/范围内共享一个实例）

```csharp
// 瞬时生命周期示例
public interface IMyTransientService : ITransientDependency
{
    void DoSomething();
}

public class MyTransientService : IMyTransientService
{
    public void DoSomething()
    {
        // 实现逻辑
    }
}

// 单例生命周期示例
public interface IMySingletonService : ISingletonDependency
{
    void DoSomething();
}

public class MySingletonService : IMySingletonService
{
    public void DoSomething()
    {
        // 实现逻辑
    }
}

// 范围生命周期示例
public interface IMyScopedService : IScopedDependency
{
    void DoSomething();
}

public class MyScopedService : IMyScopedService
{
    public void DoSomething()
    {
        // 实现逻辑
    }
}```

#### 属性注入

使用 `AutowiredAttribute` 特性可以实现属性注入，无需通过构造函数注入依赖：

```csharp
public class MyService : IMyTransientService
{
    // 使用 Autowired 特性进行属性注入
    [Autowired]
    public ILogger<MyService> Logger { get; set; }
    
    // 可以指定服务名称进行注入
    [Autowired("customServiceName")]
    public IAnotherService AnotherService { get; set; }
    
    public void DoSomething()
    {
        Logger.LogInformation("执行某些操作");
        AnotherService.Execute();
        // 实现逻辑
    }
}```

### 控制器开发

创建 API 控制器：

```csharp
[ApiController]
[Route("/api/my-controller")]
public class MyController : ControllerBase
{
    private readonly IMyService _myService;

    public MyController(IMyService myService)
    {
        _myService = myService;
    }

    [HttpGet("do-something")]
    public IActionResult DoSomething()
    {
        _myService.DoSomething();
        return Ok(new { success = true });
    }
}
```

### 后台任务

创建后台任务：

```csharp
public class MyBackgroundJob : BackgroundJob
{
    public override Task ExecuteAsync()
    {
        // 实现后台任务逻辑
        return Task.CompletedTask;
    }
}
```

注册后台任务：

```csharp
builder.Services.AddBackgroundJobs(builder.Configuration);
builder.Services.AddBackgroundJobTasks();
```

### 数据访问

使用 SqlSugar 进行数据访问：

```csharp
public interface IMyRepository : ITransientDependency
{
    Task<List<MyEntity>> GetAllAsync();
}

public class MyRepository : IMyRepository
{
    private readonly ISqlSugarClient _db;

    public MyRepository(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<List<MyEntity>> GetAllAsync()
    {
        return await _db.Queryable<MyEntity>().ToListAsync();
    }
}
```

## 模块指南

### Hospital.Fw.BackgroundJobs 后台任务模块

后台任务模块提供了一套完整的后台任务处理系统，支持定时任务和异步处理。

#### 核心组件

- `IBackgroundJob`：后台任务接口，所有后台任务都需要实现此接口
- `BackgroundJobExecuter`：后台任务执行器，负责执行后台任务
- `IBackgroundJobManager`：后台任务管理器，用于管理和调度后台任务
- `BackgroundJobWorker`：后台任务工作者，负责从存储中获取并执行任务
- `IBackgroundJobStore`：后台任务存储接口，用于存储和检索任务信息

#### 使用方式

1. 创建后台任务：

```csharp
public class MyBackgroundJob : : IAsyncBackgroundJob<UserInitEto>, ITransientDependency
{
    public async Task ExecuteAsync(JobExecutionContext context)
    {
        // 实现后台任务逻辑
        await Task.CompletedTask;
    }
}
```

2. 注册后台任务服务：

```csharp
builder.Services.AddBackgroundJobs(builder.Configuration);
```

3. 使用后台任务管理器：

```csharp
public class MyService : IMyService
{
    private readonly IBackgroundJobManager _backgroundJobManager;

    public MyService(IBackgroundJobManager backgroundJobManager)
    {
        _backgroundJobManager = backgroundJobManager;
    }

    public async Task DoSomethingAsync()
    {
        // 立即执行后台任务
        await _backgroundJobManager.EnqueueAsync(new MyJobArgs { Param = "value" });

        // 延迟执行后台任务
        await _backgroundJobManager.EnqueueAsync(
            new MyJobArgs { Param = "value" },
            delay: TimeSpan.FromMinutes(5)
        );

        // 设置任务优先级
        await _backgroundJobManager.EnqueueAsync(
            new MyJobArgs { Param = "value" },
            priority: BackgroundJobPriority.High
        );
    }
}
```

### Hospital.Fw.Interceptor AOP模块

拦截器模块提供了基于 Castle.DynamicProxy 的 AOP（面向切面编程）功能，允许在方法执行前后插入自定义逻辑。

#### 核心组件

- `IFwInterceptor`：拦截器接口，所有拦截器都需要实现此接口
- `FwAsyncDeterminationInterceptor`：异步拦截器，用于处理异步方法的拦截
- `DynamicProxyAutofacModule`：Autofac 模块，用于注册和配置拦截器

#### 使用方式

1. 创建拦截器：

```csharp
public class MyInterceptor : FwInterceptor
{
    public async Task InterceptAsync(IFwMethodInvocation invocation)
    {
        // 方法执行前的逻辑
        Console.WriteLine($"执行方法: {invocation.Method.Name}");

        // 执行原始方法
        await invocation.ProceedAsync();

        // 方法执行后的逻辑
        Console.WriteLine("方法执行完成");
    }
}
```

2. 标记需要拦截的服务：

```csharp
public interface IMyService : IDynamicProxyEnabled
{
    Task DoSomethingAsync();
}

[Intercept(typeof(MyInterceptor))]
public class MyService : IMyService
{
    public async Task DoSomethingAsync()
    {
        // 实现逻辑
        await Task.CompletedTask;
    }
}
```

### Hospital.Fw.Interceptor.Logging HTTP请求日志模块

日志拦截器模块提供了对 HTTP 请求的自动日志记录功能，可以记录请求的详细信息、执行时间等。

#### 核心组件

- `LoggingInterceptor`：日志拦截器，用于记录方法调用的日志
- `LoggingAttribute`：标记需要记录日志的方法或类
- `LoggingHelper`：日志辅助类，提供日志记录的辅助方法

#### 使用方式

```csharp
// 标记整个类的方法都需要记录日志
[Logging]
public class MyService : IMyService
{
    public void DoSomething()
    {
        // 实现逻辑
    }
}

// 或者只标记特定方法
public class AnotherService : IAnotherService
{
    [Logging]
    public void DoSomethingImportant()
    {
        // 实现逻辑
    }
}
```

### Hospital.Fw.Interceptor.Sequence 自增序列种子模块

序列拦截器模块提供了类似数据库自增列的功能，可以生成自增序列号，常用于生成业务编号、订单号等。

#### 核心组件

- `ISequenceManager`：序列管理器接口，用于获取下一个序列号
- `SequenceManager`：序列管理器实现，基于数据库序列生成序列号
- `BatchNumberManager`：批次号管理器，用于生成批次号

#### 使用方式

```csharp
public class MyService : IMyService
{
    private readonly ISequenceManager _sequenceManager;

    public MyService(ISequenceManager sequenceManager)
    {
        _sequenceManager = sequenceManager;
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        // 获取下一个序列号
        long nextSequence = await _sequenceManager.GetNextSequenceAsync("ORDER");

        // 格式化序列号（例如：添加前缀、补零等）
        string orderNumber = await _sequenceManager.PadNumberWithZerosAsync(nextSequence, 8, "ORD");
        // 结果示例：ORD00000123

        return orderNumber;
    }
}
```

### Hospital.Fw.Mo.Logging AOP日志模块

AOP日志模块基于 Rougamo 框架，提供了方法级别的日志记录功能，可以记录方法的输入参数、返回值、执行时间等信息。

#### 核心组件

- `LoggerAttribute`：标记需要记录日志的方法或类

#### 使用方式

```csharp
public class MyService : IMyService
{
    // 记录方法执行的日志，包括参数和返回值
    [Logger]
    public string ProcessData(string input)
    {
        // 实现逻辑
        return $"Processed: {input}";
    }

    // 自定义日志级别和事件ID
    [Logger(LogLevel = LogLevel.Warning, EventId = 100)]
    public void ImportantOperation()
    {
        // 实现逻辑
    }
}
```

### Hospital.Fw.Permission 权限模块

权限模块提供了基于 JWT 的身份验证和授权功能，支持角色和权限的细粒度控制。

#### 核心组件

- `IPermissionDefinitionManager`：权限定义管理器，用于管理系统中的权限定义
- `IJwtServices`：JWT 服务接口，用于创建和解析 JWT 令牌
- `PermissionRequirement`：权限要求，用于定义权限策略
- `User`：用户模型，包含用户信息、角色和权限

#### 使用方式

1. 配置权限服务：

```csharp
builder.Services.AddPermissions(options =>
{
    options.Issuer = "your-issuer";
    options.Audience = "your-audience";
    options.Secret = "your-secret-key";
    options.Expires = 7200; // 令牌过期时间（秒）
});
```

2. 创建和使用 JWT 令牌：

```csharp
public class AuthService : IAuthService
{
    private readonly IJwtServices _jwtServices;

    public AuthService(IJwtServices jwtServices)
    {
        _jwtServices = jwtServices;
    }

    public string Login(string username, string password)
    {
        // 验证用户凭据...

        // 创建用户信息
        var user = new User
        {
            Id = "1",
            UserName = username,
            NickName = "用户昵称",
            Roles = new[] { "admin", "user" },
            Permissions = new[] { "user.create", "user.edit" }
        };

        // 创建 JWT 令牌
        return _jwtServices.CreateToken(user);
    }

    public User ValidateToken(string token)
    {
        // 解析和验证 JWT 令牌
        return _jwtServices.ParseToken(token);
    }
}
```

3. 在控制器中使用权限：

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "Permission:user.view")]
    public IActionResult GetUsers()
    {
        // 只有拥有 user.view 权限的用户才能访问
        return Ok(new { message = "用户列表" });
    }
}
```

### Hospital.Fw.TestBase 单元测试模块

测试基础模块提供了一套用于单元测试的基础设施，简化了依赖注入、模拟对象等测试准备工作。

#### 核心组件

- `InterceptionTestBase`：拦截测试基类，用于测试拦截器
- `TestAutofacModule`：测试用 Autofac 模块，用于配置测试环境的依赖注入
- `AutofacServiceProviderFactory`：Autofac 服务提供者工厂，用于创建测试用服务提供者

#### 使用方式

```csharp
public class MyServiceTests : InterceptionTestBase
{
    private IMyService _myService;

    protected override void ConfigureServices(IServiceCollection services)
    {
        // 配置测试服务
        services.AddTransient<IMyService, MyService>();
        
        // 调用基类方法完成配置
        base.ConfigureServices(services);
    }

    protected override void ConfigureContainer(ContainerBuilder builder)
    {
        // 配置 Autofac 容器
        builder.RegisterType<MyInterceptor>();
        
        // 调用基类方法完成配置
        base.ConfigureContainer(builder);
    }

    [Fact]
    public async Task MyService_DoSomething_ShouldWork()
    {
        // 从服务提供者获取服务实例
        _myService = ServiceProvider.GetRequiredService<IMyService>();

        // 执行测试
        await _myService.DoSomethingAsync();

        // 断言测试结果
        // ...
    }
}
```

## API 参考

框架提供了丰富的 API，详细文档可通过 Swagger UI 查看（开发环境下访问 `/swagger`）。

## 贡献指南

欢迎贡献代码、报告问题或提出改进建议。请遵循以下步骤：

1. 克隆仓库 (`git clone https://gitee.com/notify/hospital.-fw.git`)
2. 创建特性分支 (`git checkout -b feature/amazing-feature`)
3. 提交更改 (`git commit -m 'Add some amazing feature'`)
4. 推送到分支 (`git push origin feature/amazing-feature`)
5. 在Gitee上创建合并请求

## 许可证

[MIT License](LICENSE)
