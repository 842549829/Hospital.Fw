namespace Hospital.Fw.BackgroundJobs.Abstractions;

public interface IAsyncBackgroundJob<in TArgs>
{
    /// <summary>
    /// Executes the job with the <paramref name="args"/>.
    /// </summary>
    /// <param name="args">Job arguments.</param>
    Task ExecuteAsync(TArgs args);
}