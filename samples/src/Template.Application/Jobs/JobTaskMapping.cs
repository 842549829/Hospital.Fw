using Hospital.Fw.BackgroundJobs.Implementations;
using Mapster;
using Template.Domain.Jobs.Entities;

namespace Template.Application.Jobs;

public class JobTaskMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<JobTask, BackgroundJobInfo>();
        config.ForType<BackgroundJobInfo, JobTask>();
    }
}