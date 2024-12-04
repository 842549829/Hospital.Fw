using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Domain.Shared.Core.Autofac;
using Hospital.Fw.Domain.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hospital.Fw.BackgroundJobs.Implementations;

public class BackgroundJobExecuter : IBackgroundJobExecuter, ITransientDependency
{
    public ILogger<BackgroundJobExecuter> Logger { protected get; set; }

    protected BackgroundJobOptions Options { get; }

    public BackgroundJobExecuter(IOptions<BackgroundJobOptions> options, ILogger<BackgroundJobExecuter> logger)
    {
        Options = options.Value;

        Logger = logger;
    }

    public virtual async Task ExecuteAsync(JobExecutionContext context)
    {
        var job = context.ServiceProvider.GetService(context.JobType);
        if (job == null)
        {
            throw new TaskException("The job type is not registered to DI: " + context.JobType);
        }

        var jobExecuteMethod = context.JobType.GetMethod(nameof(IBackgroundJob<object>.Execute)) ??
                               context.JobType.GetMethod(nameof(IAsyncBackgroundJob<object>.ExecuteAsync));
        if (jobExecuteMethod == null)
        {
            throw new TaskException($"Given job type does not implement {typeof(IBackgroundJob<>).Name} or {typeof(IAsyncBackgroundJob<>).Name}. " +
                                    "The job type was: " + context.JobType);
        }

        try
        {
            if (jobExecuteMethod.Name == nameof(IAsyncBackgroundJob<object>.ExecuteAsync))
            {
                await ((Task)jobExecuteMethod.Invoke(job, new[] { context.JobArgs })!);
            }
            else
            {
                jobExecuteMethod.Invoke(job, new[] { context.JobArgs });
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "BackgroundJobExecuter异常");

            await context.ServiceProvider
                .GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(new ExceptionNotificationContext(ex,LogLevel.Error));

            throw new BackgroundJobExecutionException("A background job execution is failed. See inner exception for details.", ex)
            {
                JobType = context.JobType.AssemblyQualifiedName!,
                JobArgs = context.JobArgs
            };
        }
    }
}