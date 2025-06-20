using Serilog;
using Serilog.Events;

namespace Hospital.Fw.PermissionTest;

public static class LoggerExtensions
{
    public static void ConfigureLogLevelFile(this LoggerConfiguration config)
    {
        BaseConfigureLogLevelFile(config, LogEventLevel.Verbose, $"{AppContext.BaseDirectory}logs/base/verbose/logs.log");
        BaseConfigureLogLevelFile(config, LogEventLevel.Debug, $"{AppContext.BaseDirectory}logs/base/debug/logs.log");
        BaseConfigureLogLevelFile(config, LogEventLevel.Information, $"{AppContext.BaseDirectory}logs/base/information/logs.log");
        BaseConfigureLogLevelFile(config, LogEventLevel.Warning, $"{AppContext.BaseDirectory}logs/base/warning/logs.log");
        BaseConfigureLogLevelFile(config, LogEventLevel.Error, $"{AppContext.BaseDirectory}logs/base/error/logs.log");
        BaseConfigureLogLevelFile(config, LogEventLevel.Fatal, $"{AppContext.BaseDirectory}logs/base/fatal/logs.log");

        NetWorkConfigureLogLevelFile(config, LogEventLevel.Verbose, $"{AppContext.BaseDirectory}logs/network/verbose/logs.log");
        NetWorkConfigureLogLevelFile(config, LogEventLevel.Debug, $"{AppContext.BaseDirectory}logs/network/debug/logs.log");
        NetWorkConfigureLogLevelFile(config, LogEventLevel.Information, $"{AppContext.BaseDirectory}logs/network/information/logs.log");
        NetWorkConfigureLogLevelFile(config, LogEventLevel.Warning, $"{AppContext.BaseDirectory}logs/network/warning/logs.log");
        NetWorkConfigureLogLevelFile(config, LogEventLevel.Error, $"{AppContext.BaseDirectory}logs/network/error/logs.log");
        NetWorkConfigureLogLevelFile(config, LogEventLevel.Fatal, $"{AppContext.BaseDirectory}logs/network/fatal/logs.log");
    }

    public static void NetWorkConfigureLogLevelFile(this LoggerConfiguration config, LogEventLevel level, string filePath)
    {
        //config.WriteTo.Async(a => a.Logger(lc => lc.Filter.ByIncludingOnly(e => e.Level == level && (!e.Properties.ContainsKey(SystemConst.SourceContext) || (e.Properties.ContainsKey(SystemConst.SourceContext) && e.Properties[SystemConst.SourceContext].ToString() == $"\"{typeof(HttpRequestHelper).FullName}\"")))
        //    .WriteTo.File(path: filePath, restrictedToMinimumLevel: LogEventLevel.Verbose, rollingInterval: RollingInterval.Hour)));
        config.WriteTo.Async(c =>
            c.File($"{AppContext.BaseDirectory}logs/logs.log", rollingInterval: RollingInterval.Hour));
    }

    public static void BaseConfigureLogLevelFile(this LoggerConfiguration config, LogEventLevel level, string filePath)
    {
        //config.WriteTo.Async(a => a.Logger(lc => lc.Filter.ByIncludingOnly(e => e.Level == level && (!e.Properties.ContainsKey(SystemConst.SourceContext) || (e.Properties.ContainsKey(SystemConst.SourceContext) && e.Properties[SystemConst.SourceContext].ToString() != $"\"{typeof(HttpRequestHelper).FullName}\"")))
        //    .WriteTo.File(path: filePath, restrictedToMinimumLevel: LogEventLevel.Verbose, rollingInterval: RollingInterval.Hour)));
    }
}