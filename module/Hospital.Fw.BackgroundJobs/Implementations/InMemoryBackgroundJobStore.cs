using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Domain.Shared.Core.Autofac;
using Hospital.Fw.Domain.Shared.Generic;
using System.Collections.Concurrent;

namespace Hospital.Fw.BackgroundJobs.Implementations;

public class InMemoryBackgroundJobStore : IBackgroundJobStore, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, BackgroundJobInfo> _jobs;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryBackgroundJobStore"/> class.
    /// </summary>
    public InMemoryBackgroundJobStore()
    {
        _jobs = new ConcurrentDictionary<string, BackgroundJobInfo>();
    }

    public virtual Task<BackgroundJobInfo> FindAsync(string jobId)
    {
        return Task.FromResult(_jobs.GetOrDefault(jobId)!);
    }

    public virtual Task InsertAsync(BackgroundJobInfo jobInfo)
    {
        _jobs[jobInfo.Id] = jobInfo;

        return Task.CompletedTask;
    }

    public virtual Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
    {
        var waitingJobs = _jobs.Values
            .Where(t => !t.IsAbandoned && t.NextTryTime <= DateTime.Now)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.TryCount)
            .ThenBy(t => t.NextTryTime)
            .Take(maxResultCount)
            .ToList();

        return Task.FromResult(waitingJobs);
    }


    public virtual Task DeleteAsync(string jobId)
    {
        _jobs.TryRemove(jobId, out _);

        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(BackgroundJobInfo jobInfo)
    {
        if (jobInfo.IsAbandoned)
        {
            return DeleteAsync(jobInfo.Id);
        }

        return Task.CompletedTask;
    }
}