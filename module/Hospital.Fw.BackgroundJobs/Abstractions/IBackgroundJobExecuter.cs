namespace Hospital.Fw.BackgroundJobs.Abstractions;

public interface IBackgroundJobExecuter
{
    Task ExecuteAsync(JobExecutionContext context);
}