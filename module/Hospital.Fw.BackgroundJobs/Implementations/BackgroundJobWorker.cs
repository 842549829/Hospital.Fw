using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Domain.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hospital.Fw.BackgroundJobs.Implementations;

public class BackgroundJobWorker : AsyncPeriodicBackgroundWorkerBase, IBackgroundJobWorker
{
    protected BackgroundJobOptions JobOptions { get; }

    protected BackgroundJobWorkerOptions WorkerOptions { get; }

    public BackgroundJobWorker(
        IOptions<BackgroundJobOptions> jobOptions,
        IOptions<BackgroundJobWorkerOptions> workerOptions,
        IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        WorkerOptions = workerOptions.Value;
        JobOptions = jobOptions.Value;
        Timer.Period = WorkerOptions.JobPollPeriod;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var store = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobStore>();

        var waitingJobs = await store.GetWaitingJobsAsync(WorkerOptions.MaxJobFetchCount);

        if (!waitingJobs.Any())
        {
            return;
        }

        var jobExecuter = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobExecuter>();
        var serializer = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobSerializer>();

        foreach (var jobInfo in waitingJobs)
        {
            jobInfo.TryCount++;
            jobInfo.LastTryTime = DateTime.Now;

            try
            {
                var jobConfiguration = JobOptions.GetJob(jobInfo.JobName);
                var jobArgs = serializer.Deserialize(jobInfo.JobArgs, jobConfiguration.ArgsType);
                var context = new JobExecutionContext(
                    workerContext.ServiceProvider,
                    jobConfiguration.JobType,
                    jobArgs,
                    workerContext.CancellationToken);

                try
                {
                    await jobExecuter.ExecuteAsync(context);

                    await store.DeleteAsync(jobInfo.Id);
                }
                catch (BackgroundJobExecutionException)
                {
                    var nextTryTime = CalculateNextTryTime(jobInfo);

                    if (nextTryTime.HasValue)
                    {
                        jobInfo.NextTryTime = nextTryTime.Value;
                    }
                    else
                    {
                        jobInfo.IsAbandoned = true;
                    }

                    await TryUpdateAsync(store, jobInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "BackgroundJobWorker.DoWorkAsync异常");
                jobInfo.IsAbandoned = true;
                await TryUpdateAsync(store, jobInfo);
            }
        }
    }

    protected virtual async Task TryUpdateAsync(IBackgroundJobStore store, BackgroundJobInfo jobInfo)
    {
        try
        {
            await store.UpdateAsync(jobInfo);
        }
        catch (Exception updateEx)
        {
            Logger.LogError(updateEx, "BackgroundJobWorker.TryUpdateAsync异常");
        }
    }

    protected virtual DateTime? CalculateNextTryTime(BackgroundJobInfo jobInfo)
    {
        var nextWaitDuration = WorkerOptions.DefaultFirstWaitDuration *
                               (Math.Pow(WorkerOptions.DefaultWaitFactor, jobInfo.TryCount - 1));
        var nextTryDate = jobInfo.LastTryTime?.AddSeconds(nextWaitDuration) ??
                          DateTime.Now.AddSeconds(nextWaitDuration);

        if (nextTryDate.Subtract(jobInfo.CreationTime).TotalSeconds > WorkerOptions.DefaultTimeout)
        {
            return null;
        }

        return nextTryDate;
    }
}
