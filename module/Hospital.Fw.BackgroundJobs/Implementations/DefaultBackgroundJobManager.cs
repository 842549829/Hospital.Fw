using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Domain.Shared.Core.Autofac;

namespace Hospital.Fw.BackgroundJobs.Implementations;

public class DefaultBackgroundJobManager(
    IBackgroundJobSerializer serializer,
    IBackgroundJobStore store)
    : IBackgroundJobManager, ITransientDependency
{
    protected IBackgroundJobSerializer Serializer { get; } = serializer;
    protected IBackgroundJobStore Store { get; } = store;

    public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, DateTime? nextTryTime = null, string? jonId = null)
    {
        var jobName = BackgroundJobNameAttribute.GetName<TArgs>();
        var jobId = await EnqueueAsync(jobName, args!, priority, nextTryTime, jonId);
        return jobId.ToString();
    }

    protected virtual async Task<string> EnqueueAsync(string jobName, object args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, DateTime? nextTryTime = null, string? jonId = null)
    {
        var currentTime = DateTime.Now;
        var jobInfo = new BackgroundJobInfo
        {
            Id = jonId ?? Guid.NewGuid().ToString(),
            JobName = jobName,
            JobArgs = Serializer.Serialize(args),
            Priority = priority,
            CreationTime = currentTime,
            NextTryTime = currentTime
        };

        if (nextTryTime.HasValue)
        {
            jobInfo.NextTryTime = nextTryTime.Value;
        }

        await Store.InsertAsync(jobInfo);

        return jobInfo.Id;
    }
}