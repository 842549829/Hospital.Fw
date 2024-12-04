using Hospital.Fw.Domain.Shared.Core.Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Hospital.Fw.Domain.Shared.Exceptions;

public class TaskExceptionNotifier : IExceptionNotifier, ITransientDependency
{
    public static TaskExceptionNotifier Instance { get; } = new();

    public ILogger<TaskExceptionNotifier> Logger { get; set; } = NullLogger<TaskExceptionNotifier>.Instance;

    public virtual Task NotifyAsync(ExceptionNotificationContext context)
    {
        return Task.CompletedTask;

        //return System.Threading.Tasks.Task.Run(() =>
        //{
        //    Logger.LogError(context.Exception, context.Exception.Message);
        //});
    }
}