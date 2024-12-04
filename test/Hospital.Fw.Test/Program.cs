using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hospital.Fw.Application;
using Hospital.Fw.BackgroundJobs;
using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.BackgroundJobs.Implementations;
using Hospital.Fw.Domain.Shared.Core;
using Hospital.Fw.HttpApi.Autofac;
using Hospital.Fw.HttpApi.Filter;
using Hospital.Fw.HttpApi.Middleware;
using Hospital.Fw.Test.SqlSugarCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;

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
    var builder = WebApplication.CreateBuilder(args);

    LoadAssemblies.AssembliesStartingWith = 
    [
        Assembly.Load("Hospital.Fw.Application"),
        Assembly.Load("Hospital.Fw.Application.Contract"),
        Assembly.Load("Hospital.Fw.Domain"),
        Assembly.Load("Hospital.Fw.Domain.Shared"),
        Assembly.Load("Hospital.Fw.HttpApi"),
        Assembly.Load("Hospital.Fw.SqlSugarCore"),
        Assembly.Load("Hospital.Fw.Test")
    ];

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacModule>();
    });

    // 替换控制器的替换规则(目的:使用Autofac的特性注入)
    builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

    builder.Services.AddTransactionFilter();

    builder.Services.AddAutoMapper(LoadAssemblies.AssembliesStartingWith);

    // Add services to the container.
    builder.Host.UseSerilog();

    builder.Services.AddControllers()
        .AddJsonOptions(jsonOptions =>
        {
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null; // 保持属性名不变
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        // 设置标题和描述
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hospital.Fw.Test", Version = "v1" });
        var assemblies = LoadAssemblies.AssembliesStartingWith;
        foreach (var assembly in assemblies)
        {
            // 获取 XML 文件路径
            var xmlFile = $"{assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            // 检查 XML 文件是否存在并包含到 Swagger 文档中
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        }
    });

    builder.Services.AddFluentValidation();

    builder.Services.AddSqlSugar(builder.Configuration);

    // 添加后台任务服务
    builder.Services.AddBackgroundJobs(builder.Configuration);

    var app = builder.Build();

    var serviceProvider = app.Services;

    app.UseExceptionHandling();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    // 添加后台任务
    app.Lifetime.ApplicationStarted.Register(StartCallback);

    // 停止服务
    app.Lifetime.ApplicationStopping.Register(StopCallback);

    app.Run();

    // 启动服务
    async void StartCallback()
    {
        if (serviceProvider.GetRequiredService<IOptions<BackgroundJobOptions>>().Value.IsJobExecutionEnabled)
        {
            await serviceProvider.AddBackgroundWorkerAsync<IBackgroundJobWorker>();
        }
        await serviceProvider.GetRequiredService<IBackgroundWorkerManager>().StartAsync();
    }

    // 停止服务
    async void StopCallback()
    {
        await serviceProvider.GetRequiredService<IBackgroundWorkerManager>().StopAsync();
    }
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