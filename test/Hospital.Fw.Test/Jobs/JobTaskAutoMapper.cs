using AutoMapper;
using Hospital.Fw.BackgroundJobs.Implementations;

namespace Hospital.Fw.Test.Jobs;

public class JobTaskAutoMapper : Profile
{
    public JobTaskAutoMapper()
    {
        CreateMap<JobTask, BackgroundJobInfo>().ReverseMap();
    }
}