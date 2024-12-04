namespace Hospital.Fw.BackgroundJobs.Abstractions;

public interface IBackgroundJobManager
{
    /// <summary>
    /// Enqueues a job to be executed.
    /// </summary>
    /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
    /// <param name="args">Job arguments.</param>
    /// <param name="priority">Job priority.</param>
    /// <param name="nextTryTime">下次执行时间</param>
    /// <param name="jobId">jobId</param>
    /// <returns>Unique identifier of a background job.</returns>
    Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        DateTime? nextTryTime  = null,
        string? jobId = null
    );
}