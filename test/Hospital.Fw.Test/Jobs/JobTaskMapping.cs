using Hospital.Fw.BackgroundJobs.Implementations;
using Mapster;

namespace Hospital.Fw.Test.Jobs;

public class JobTaskMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<JobTask, BackgroundJobInfo>();
        config.ForType<BackgroundJobInfo, JobTask>();
    }
}