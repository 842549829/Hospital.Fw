using Hospital.Fw.Api.Test.Dto;
using Hospital.Fw.Api.Test.Entities;
using Hospital.Fw.Application.Contract.Dto;
using Hospital.Fw.Automatic.Api.HttpApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Fw.Api.Test.Controllers;

[Route("api/job")]
public class JobTaskController : ApiBaseController<JobTask, JobTaskDto, JobTaskCreateDto, JobTaskUpdateDto, JobTaskGetListInput, JobTaskGetListDto>
{
    /// <summary>
    /// 根据ID获取单个实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>返回对应的实体数据</returns>
    [HttpGet("/test")]
    public override Task<ResultDto<JobTaskDto>> GetAsync(string id)
    {
        return base.GetAsync(id);
    }
}