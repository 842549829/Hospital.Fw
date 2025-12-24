using Hospital.Fw.Application.Contract;

namespace Hospital.Fw.Automatic.Api.Application.Contract;

/// <summary>
/// API基础服务
/// </summary>
/// <typeparam name="TEntity">实体类</typeparam>
/// <typeparam name="TKey">Key</typeparam>
/// <typeparam name="TDto">详情</typeparam>
/// <typeparam name="TCreateDto">创建</typeparam>
/// <typeparam name="TUpdateDto">修改</typeparam>
/// <typeparam name="TGetListInput">列表输入</typeparam>
/// <typeparam name="TGetListDto">列表输出</typeparam>
public interface IApiBaseAppService<TEntity, in TKey, TDto, in TCreateDto, in TUpdateDto, in TGetListInput, TGetListDto> : IBaseAppService
    where TEntity : class, new()
    where TDto : EntityDto
    where TCreateDto : EntityDto
    where TUpdateDto : EntityDto
    where TGetListInput : PageInput
    where TGetListDto : EntityDto
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input">创建</param>
    /// <returns>结果</returns>
    Task<bool> CreateAsync(TCreateDto input);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input">修改</param>
    /// <returns>结果</returns>
    Task<bool> UpdateAsync(TUpdateDto input);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">删除</param>
    /// <returns>结果</returns>
    Task<bool> DeleteAsync(TKey id);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="input">条件</param>
    /// <returns>结果</returns>
    Task<PageListDto<TGetListDto>> GetListAsync(TGetListInput input);

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>结果</returns>
    Task<TDto> GetAsync(TKey id);
}