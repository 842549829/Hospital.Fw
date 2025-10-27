using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hospital.Fw.Application;
using Hospital.Fw.Domain.Shared.Core;
using Hospital.Fw.HttpApi.Autofac;
using Hospital.Fw.HttpApi.Middleware;
using Hospital.Fw.PermissionTest.Permissions;
using Hospital.Fw.PermissionTest.SqlSugarCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;

/*
 * dotnet pack -c release -o C:\Users\Administrator\Desktop\pack
 * dotnet nuget push *.nupkg -k EE90BD155433BB -s "http://192.168.5.245:8885/nuget" --skip-duplicate 
 */
Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File($"{AppContext.BaseDirectory}logs/logs.log", rollingInterval: RollingInterval.Day))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacModule>();
    });

    builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

    //builder.Services.AddTransactionFilter();

    // 获取你要扫描的程序集（比如 Application 程序集）
    // 自动注册所有实现 IRegister 的类
    TypeAdapterConfig.GlobalSettings.Scan(LoadAssemblies.AssembliesStartingWith.ToArray());
    builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
    builder.Services.AddScoped<IMapper, ServiceMapper>();

    // Add services to the container.
    builder.Host.UseSerilog();

    builder.Services.AddControllers()
        .AddJsonOptions(jsonOptions =>
        {
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hospital.Fw.Test", Version = "v1" });
        List<Assembly> assemblies = LoadAssemblies.AssembliesStartingWith;
        foreach (Assembly assembly in assemblies)
        {
            var xmlFile = $"{assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        }
    });

    builder.Services.AddFluentValidation(typeof(ApplicationExtensions));

    builder.Services.AddSqlSugar(builder.Configuration);

    builder.Services.AddPermissionsTest(builder.Configuration);

    WebApplication app = builder.Build();

    app.UseExceptionHandling();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // 开启认证
    app.UseAuthentication();

    // 授权中间件
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    if (ex is HostAbortedException)
    {
        throw;
    }

    Log.Fatal(ex, "Host terminated unexpectedly!");
}
finally
{
    Log.CloseAndFlush();
}