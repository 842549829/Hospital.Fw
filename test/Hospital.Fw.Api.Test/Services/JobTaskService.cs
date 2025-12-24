using Hospital.Fw.Api.Test.Dto;
using Hospital.Fw.Api.Test.Entities;
using Hospital.Fw.Automatic.Api.Application;
using SqlSugar;

namespace Hospital.Fw.Api.Test.Services;

public class JobTaskService : ApiBaseAppService<JobTask, string, JobTaskDto, JobTaskCreateDto, JobTaskUpdateDto, JobTaskGetListInput, JobTaskGetListDto>
{
    public override string ConfigId => "Job";

    /// <summary>
    /// 获取查询条件
    /// </summary>
    /// <param name="db">db</param>
    /// <param name="input">查询条件</param>
    /// <returns>ISugarQueryable</returns>
    protected override ISugarQueryable<JobTask> BuildBaseQuery(ISqlSugarClient db, JobTaskGetListInput input)
    {
        var queryable = db.Queryable<JobTask>()
            .WhereIF(!string.IsNullOrEmpty(input.JobArgs), x => x.JobArgs == input.JobArgs)
            .WhereIF(!string.IsNullOrEmpty(input.JobName), x => x.JobName == input.JobName)
            .OrderBy(x => x.Id);
        return queryable;
    }
}