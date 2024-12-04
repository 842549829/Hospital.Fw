namespace Hospital.Fw.BackgroundJobs.Abstractions;

public abstract class BackgroundJob<TArgs> : IBackgroundJob<TArgs>
{
    public abstract void Execute(TArgs args);
}