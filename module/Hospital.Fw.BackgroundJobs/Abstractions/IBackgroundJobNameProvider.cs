namespace Hospital.Fw.BackgroundJobs.Abstractions;

public interface IBackgroundJobNameProvider
{
    string Name { get; }
}