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
using Hospital.Fw.Test.Jobs;
using Hospital.Fw.Test.SqlSugarCore;
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
    var builder = WebApplication.CreateBuilder(args);

    LoadAssemblies.AssembliesStartingWith = 
    [
        Assembly.Load("Hospital.Fw.Application"),
        Assembly.Load("Hospital.Fw.Application.Contract"),
        Assembly.Load("Hospital.Fw.Domain"),
        Assembly.Load("Hospital.Fw.Domain.Shared"),
        Assembly.Load("Hospital.Fw.HttpApi"),
        Assembly.Load("Hospital.Fw.SqlSugarCore"),
        Assembly.Load("Hospital.Fw.Test"),
        Assembly.Load("Hospital.Fw.Interceptor.Logging"), 
        Assembly.Load("Hospital.Fw.Interceptor"),
    ];

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacModule>();
        containerBuilder.RegisterModule<DynamicProxyAutofacModule>();
    });

    builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

    //builder.Services.AddTransactionFilter();

    builder.Services.AddAutoMapper(LoadAssemblies.AssembliesStartingWith);

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
        var assemblies = LoadAssemblies.AssembliesStartingWith;
        foreach (var assembly in assemblies)
        {
            var xmlFile = $"{assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        }
    });

    builder.Services.AddFluentValidation();

    builder.Services.AddSqlSugar(builder.Configuration);

    builder.Services.AddBackgroundJobs(builder.Configuration);
    builder.Services.AddBackgroundJobTasks();

    var app = builder.Build();

    var serviceProvider = app.Services;

    app.UseExceptionHandling();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
        var jobDb = db.AsTenant().GetConnection(SqlSugarCoreDbConst.Job);
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