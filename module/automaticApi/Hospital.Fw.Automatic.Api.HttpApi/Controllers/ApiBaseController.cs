using Hospital.Fw.Application.Contract;
using Hospital.Fw.Automatic.Api.Application.Contract;
using Hospital.Fw.HttpApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Fw.Automatic.Api.HttpApi.Controllers;

/// <summary>
/// API基类
/// </summary>
public abstract class ApiBaseController<TEntity, TDto, TCreateDto, TUpdateDto, TGetListInput, TGetListDto> : BaseController
    where TEntity : class, new()
    where TDto : EntityDto
    where TCreateDto : EntityDto
    where TUpdateDto : EntityDto
    where TGetListInput : PageInput
    where TGetListDto : EntityDto
{
    /// <summary>
    /// 根据ID获取单个实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>返回对应的实体数据</returns>
    [HttpGet("detail")]
    public virtual async Task<ResultDto<TDto>> GetAsync([FromQuery] string id)
    {
        var service = Resolve();
        var result = await service.GetAsync(id);
        return new ResultDto<TDto>(result, true, "查询成功");
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>返回对应的实体数据</returns>
    [HttpGet("list")]
    public virtual async Task<ResultDto<PageListDto<TGetListDto>>> GetListAsync([FromQuery] TGetListInput input)
    {
        var service = Resolve();
        var result = await service.GetListAsync(input);
        return new ResultDto<PageListDto<TGetListDto>>(result, true, "查询成功");
    }

    /// <summary>
    /// 创建新实体
    /// </summary>
    /// <param name="input">创建数据传输对象</param>
    /// <returns>返回创建结果</returns>
    [HttpPost("create")]
    public virtual async Task<ResultDto<bool>> CreateAsync([FromBody] TCreateDto input)
    {
        var service = Resolve();
        var result = await service.CreateAsync(input);
        return new ResultDto<bool>(result, true, "创建成功");
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="input">更新数据传输对象</param>
    /// <returns>返回更新结果</returns>
    [HttpPut("update")]
    public virtual async Task<ResultDto<bool>> UpdateAsync([FromBody] TUpdateDto input)
    {
        var service = Resolve();
        var result = await service.UpdateAsync(input);
        return new ResultDto<bool>(result, true, "操作成功");
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="id">实体ID</param>
    /// <returns>返回删除结果</returns>
    [HttpDelete("delete")]
    public virtual async Task<ResultDto<bool>> DeleteAsync([FromQuery] string id)
    {
        var service = Resolve();
        var result = await service.DeleteAsync(id);
        return new ResultDto<bool>(result, true, "操作成功");
    }

    /// <summary>
    /// 获取Api接口列表
    /// </summary>
    /// <returns>接口</returns>
    protected virtual IApiBaseAppService<TEntity, string, TDto, TCreateDto, TUpdateDto, TGetListInput, TGetListDto> Resolve()
    {
        var service = ServiceProvider.GetRequiredService<IApiBaseAppService<TEntity, string, TDto, TCreateDto, TUpdateDto, TGetListInput, TGetListDto>>();
        return service;
    }
}