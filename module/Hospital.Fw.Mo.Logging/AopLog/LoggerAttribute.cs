using System.Diagnostics;
using Hospital.Fw.Domain.Shared.Core.Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rougamo;
using Rougamo.Context;

namespace Hospital.Fw.Mo.Logging.AopLog;

/// <summary>
/// 日志记录
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class)]
public class LoggerAttribute : MoAttribute, ITransientDependency
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    private static readonly AsyncLocal<IServiceProvider> ServiceProvider = new();

    /// <summary>
    /// set service provider
    /// </summary>
    /// <param name="serviceProvider">serviceProvider</param>
    public static void SetServiceProvider(IServiceProvider serviceProvider) => ServiceProvider.Value = serviceProvider;

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel LogLevel { get; }

    /// <summary>
    /// 是否记录返回值
    /// </summary>
    public bool LogReturnValue { get; }

    /// <summary>
    /// 是否记录参数
    /// </summary>
    public bool LogArguments { get; }

    /// <summary>
    /// 是否记录耗时
    /// </summary>
    public bool LogExecutionTime { get; }

    /// <summary>
    /// 日志事件Id
    /// </summary>
    public EventId EventId { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="eventId">日志事件Id</param>
    /// <param name="logLevel">日志级别</param>
    /// <param name="logReturnValue">是否记录返回值，默认为 true</param>
    /// <param name="logArguments">是否记录参数，默认为 true</param>
    /// <param name="logExecutionTime">是否记录耗时，默认为 true</param>
    public LoggerAttribute(
        EventId eventId,
        LogLevel logLevel = LogLevel.Debug,
        bool logReturnValue = true,
        bool logArguments = true,
        bool logExecutionTime = true
    )
    {
        EventId = eventId;
        LogLevel = logLevel;
        LogReturnValue = logReturnValue;
        LogArguments = logArguments;
        LogExecutionTime = logExecutionTime;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="logLevel">日志级别</param>
    /// <param name="logReturnValue">是否记录返回值，默认为 true</param>
    /// <param name="logArguments">是否记录参数，默认为 true</param>
    /// <param name="logExecutionTime">是否记录耗时，默认为 true</param>
    public LoggerAttribute(
        LogLevel logLevel = LogLevel.Debug,
        bool logReturnValue = true,
        bool logArguments = true,
        bool logExecutionTime = true
        ): this(new EventId(900001, nameof(LoggerAttribute)), logLevel, logReturnValue, logArguments, logExecutionTime)
    {
    }

    /// <summary>
    /// 计时器
    /// </summary>
    private Stopwatch? _stopwatch;

    /// <summary>
    /// 方法进入时
    /// </summary>
    /// <param name="context">context</param>
    public override void OnEntry(MethodContext context)
    {
        if (LogExecutionTime)
        {
            // 记录进入方法的时间
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }
    }

    /// <summary>
    /// 方法退出时
    /// </summary>
    /// <param name="context">context</param>
    public override void OnExit(MethodContext context)
    {
        if (ServiceProvider.Value == null)
        {
            return;
        }

        var log = ServiceProvider.Value.GetRequiredService<ILogger<LoggerAttribute>>();

        var arguments = LogArguments ? context.Arguments : null;
        var returnValue = LogReturnValue ? context.ReturnValue : null;
        var elapsedMilliseconds = LogExecutionTime && _stopwatch != null ? _stopwatch.ElapsedMilliseconds : -1;

        log.Log(LogLevel, EventId, null,
            "MethodName: {FullName}.{MethodName}\r\n" +
            "Arguments: {@Arguments}\r\n" +
            "ReturnedValue: {@ReturnedValue}\r\n" +
            "ExecutionTook: {ElapsedMilliseconds}ms",
            context.TargetType.FullName,
            context.Method.Name,
            arguments,
            returnValue,
            elapsedMilliseconds);
    }
}