using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hospital.Fw.BackgroundJobs;
using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.BackgroundJobs.Implementations;
using Hospital.Fw.Domain.Shared.Constant;
using Hospital.Fw.Domain.Shared.Core;
using Hospital.Fw.HttpApi.Autofac;
using Hospital.Fw.HttpApi.Custom.Security;
using Hospital.Fw.HttpApi.Filter;
using Hospital.Fw.HttpApi.Middleware;
using Hospital.Fw.Mo.Logging.AopLog;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using SqlSugar;
using System.Reflection;
using Template.Application;
using Template.Application.Jobs;
using Template.Domain.Jobs.Entities;
using Template.Domain.Test.Entities;
using Template.SqlSugarCore;

Log.Logger = new LoggerConfiguration()
    .CreateLogger();
try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console();
    });


    LoadAssemblies.Configure(options =>
    {
        List<string> supportPackageNamePrefixs = options.SupportPackageNamePrefixs.ToList();
        supportPackageNamePrefixs.Add("Template");
        options.SupportPackageNamePrefixs = supportPackageNamePrefixs.ToArray();
    });

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<AutofacModule>();
    });

    builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

    builder.Services.AddTransactionFilter();

    TypeAdapterConfig.GlobalSettings.Scan(LoadAssemblies.AssembliesStartingWith.ToArray());
    builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
    builder.Services.AddScoped<IMapper, ServiceMapper>();

    builder.Services.AddControllers()
        .AddJsonOptions(jsonOptions =>
        {
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Template.Host", Version = "v1" });
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

    builder.Services.AddInventoryFluentValidation();

    builder.Services.AddSessionUser(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("SessionRedisConnection");
    });

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

    app.UseSessionAuthentication();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using IServiceScope scope = app.Services.CreateScope();
        ISqlSugarClient db = scope.ServiceProvider.GetRequiredService<ISqlSugarClient>();
        SqlSugarProvider jobDb = db.AsTenant().GetConnection(SqlSugarCoreDbConst.Job);
        jobDb.CodeFirst.InitTables(typeof(JobTask), typeof(Test));
    }

    app.MapControllers();

    app.Lifetime.ApplicationStarted.Register(StartCallback);

    app.Lifetime.ApplicationStopping.Register(StopCallback);

    app.Run();

    async void StartCallback()
    {
        try
        {
            if (app.Services.GetRequiredService<IOptions<BackgroundJobOptions>>().Value.IsJobExecutionEnabled)
            {
                await app.Services.AddBackgroundWorkerAsync<IBackgroundJobWorker>();
            }
            await app.Services.GetRequiredService<IBackgroundWorkerManager>().StartAsync();
        }
        catch (Exception exception)
        {

            Log.Fatal(exception, "启动后台任务异常!");
        }
    }

    async void StopCallback()
    {
        try
        {
            await app.Services.GetRequiredService<IBackgroundWorkerManager>().StopAsync();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "停止后台任务异常!");
        }
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