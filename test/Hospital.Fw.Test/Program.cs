using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hospital.Fw.Application;
using Hospital.Fw.BackgroundJobs;
using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.BackgroundJobs.Implementations;
using Hospital.Fw.Domain.Shared.Constant;
using Hospital.Fw.Domain.Shared.Core;
using Hospital.Fw.HttpApi.Autofac;
using Hospital.Fw.HttpApi.Middleware;
using Hospital.Fw.Interceptor.DynamicProxy;
using Hospital.Fw.Mo.Logging.AopLog;
using Hospital.Fw.Test.Jobs;
using Hospital.Fw.Test.SqlSugarCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using SqlSugar;
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
        containerBuilder.RegisterModule<DynamicProxyAutofacModule>();
    });

    builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

    //builder.Services.AddTransactionFilter();

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
            string xmlFile = $"{assembly.GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        }
    });

    builder.Services.AddFluentValidation(typeof(ApplicationExtensions));

    builder.Services.AddSqlSugar(builder.Configuration);

    builder.Services.AddBackgroundJobs(builder.Configuration);
    builder.Services.AddBackgroundJobTasks();

    WebApplication app = builder.Build();

    IServiceProvider serviceProvider = app.Services;


    app.UseExceptionHandling();

    // 添加日志记录器
    app.Use(async (context, next) =>
    {
        LoggerAttribute.SetServiceProvider(context.RequestServices);
        await next();
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using IServiceScope scope = app.Services.CreateScope();
        ISqlSugarClient db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
        SqlSugarProvider jobDb = db.AsTenant().GetConnection(SqlSugarCoreDbConst.Job);
        jobDb.CodeFirst.InitTables(typeof(JobTask));
    }

    app.MapControllers();

    app.Lifetime.ApplicationStarted.Register(StartCallback);

    app.Lifetime.ApplicationStopping.Register(StopCallback);

    app.Run();

    async void StartCallback()
    {
        if (serviceProvider.GetRequiredService<IOptions<BackgroundJobOptions>>().Value.IsJobExecutionEnabled)
        {
            await serviceProvider.AddBackgroundWorkerAsync<IBackgroundJobWorker>();
        }
        await serviceProvider.GetRequiredService<IBackgroundWorkerManager>().StartAsync();
    }

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