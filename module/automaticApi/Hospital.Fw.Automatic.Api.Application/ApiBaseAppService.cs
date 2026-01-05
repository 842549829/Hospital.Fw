
using Hospital.Fw.Application;
using Hospital.Fw.Application.Contract.Dto;
using Hospital.Fw.Automatic.Api.Application.Contract;
using Hospital.Fw.Domain;
using Hospital.Fw.Domain.Entities;
using Hospital.Fw.Domain.Shared.Core.Exception;
using Hospital.Fw.Domain.Shared.Core.Users;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Hospital.Fw.Automatic.Api.Application;

/// <summary>
/// API基类
/// </summary>
/// <typeparam name="TEntity">实体类</typeparam>
/// <typeparam name="TKey">Key</typeparam>
/// <typeparam name="TDto">详情</typeparam>
/// <typeparam name="TCreateDto">创建</typeparam>
/// <typeparam name="TUpdateDto">修改</typeparam>
/// <typeparam name="TGetListInput">列表输入</typeparam>
/// <typeparam name="TGetListDto">列表输出</typeparam>
public abstract class ApiBaseAppService<TEntity,  TKey, TDto,  TCreateDto,  TUpdateDto,  TGetListInput, TGetListDto> : BaseAppService, IApiBaseAppService<TEntity,  TKey, TDto,  TCreateDto,  TUpdateDto,  TGetListInput, TGetListDto>
    where TEntity : class, IEntity<TKey>, new()
    where TDto : EntityDto
    where TCreateDto : EntityDto
    where TUpdateDto : EntityDto
    where TGetListInput : PageInput
    where TGetListDto : EntityDto
{
    /// <summary>
    /// 配置Id
    /// </summary>
    public virtual string ConfigId => "ConfigId";

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input">创建</param>
    /// <returns>结果</returns>
    public virtual async Task<bool> CreateAsync(TCreateDto input)
    {
        var mapper = ServiceProvider.GetRequiredService<IMapper>();
        var entity = mapper.Map<TEntity>(input);
        var db = GetSqlSugarClient(ConfigId);
        await db.Insertable(entity).ExecuteCommandAsync("实际操作影响行数与期望影响行数不一致");
        return true;
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input">修改</param>
    /// <returns>结果</returns>
    public virtual async Task<bool> UpdateAsync(TUpdateDto input)
    {
        var mapper = ServiceProvider.GetRequiredService<IMapper>();
        var entity = mapper.Map<TEntity>(input);
        var db = GetSqlSugarClient(ConfigId);
        await db.Updateable(entity).ExecuteCommandAsync("实际操作影响行数与期望影响行数不一致");
        return true;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">删除</param>
    /// <returns>结果</returns>
    public virtual async Task<bool> DeleteAsync(TKey id)
    {
        if (id == null)
        {
            throw new CustomException("删除的主键Id为空");
        }

        var db = GetSqlSugarClient(ConfigId);

        // 检查 TEntity 是否继承自 ISoftDelete 接口
        if (typeof(TEntity).GetInterface(nameof(ISoftDelete)) != null)
        {
            // --- 执行软删除 ---
            var updateable = db.Updateable<TEntity>()
                               .SetColumns(it => (it as ISoftDelete)!.IsDeleted == true); // 必须设置 IsDeleted 为 true

            // --- 动态添加可选的删除信息 ---
            // 1. 检查并设置 DeletionId (如果实现了 IMayHaveDeletionId)
            if (typeof(TEntity).GetInterface(nameof(IMayHaveDeletionId)) != null)
            {
                // 这里需要提供具体的 DeletionId 值，例如从当前用户上下文获取
                // 示例：使用被删除记录的ID作为删除ID，实际应用中应替换为当前用户ID
                var currentUserId = GetCurrentUser().Id;
                updateable.SetColumns(it => ((IMayHaveDeletionId)it).DeletionId == (string?)currentUserId);
            }

            // 2. 检查并设置 DeletionTime (如果实现了 IMayHaveDeletionTime)
            if (typeof(TEntity).GetInterface(nameof(IMayHaveDeletionTime)) != null)
            {
                updateable.SetColumns(it => ((it as IMayHaveDeletionTime)!).DeletionTime == DateTime.Now);
            }

            // 3. 检查并设置 DeletionName (如果实现了 IMayHaveDeletionName)
            if (typeof(TEntity).GetInterface(nameof(IMayHaveDeletionName)) != null)
            {
                var currentUserDisplayName = GetCurrentUser().Name;
                // 这里需要提供具体的 DeletionName 值，例如从当前用户上下文获取
                updateable.SetColumns(it => ((it as IMayHaveDeletionName)!).DeletionName == currentUserDisplayName);
            }

            // 完成 Updateable 配置，添加 WHERE 条件
            updateable.Where(it =>  it.Id!.Equals(id)); // 假设主键属性名为 Id

            // 执行更新命令
            await updateable.ExecuteCommandAsync("实际操作影响行数与期望影响行数不一致");
        }
        else
        {
            // --- 执行硬删除 ---
            var deleteable = db.Deleteable<TEntity>(id);
            await deleteable.ExecuteCommandAsync("实际操作影响行数与期望影响行数不一致");
        }
        return true; // 操作成功
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="input">条件</param>
    /// <returns>结果</returns>
    public virtual async Task<PageListDto<TGetListDto>> GetListAsync(TGetListInput input)
    {
        var db = GetSqlSugarClient(ConfigId);
        var queryable = BuildBaseQuery(db, input);
        RefAsync<int> totalCount = 0;
        var list = await queryable.ToPageListAsync(input.PageIndex, input.PageSize, totalCount);
        var mapper = ServiceProvider.GetRequiredService<IMapper>();
        var listDto = mapper.Map<List<TGetListDto>>(list);
        return new PageListDto<TGetListDto>(listDto)
        {
            Total = totalCount.Value
        };
    }

    /// <summary>
    /// 获取查询条件
    /// </summary>
    /// <param name="db">db</param>
    /// <param name="input">查询条件</param>
    /// <returns>ISugarQueryable</returns>
    protected virtual ISugarQueryable<TEntity> BuildBaseQuery(ISqlSugarClient db, TGetListInput input)
    {
        // 基础查询
        var queryable = db.Queryable<TEntity>();

        // TODO 后期改造更近查询类特性反射动态生成查询条件

        //// 应用输入参数中的条件 (例如 BloodBagNo)
        //if (!string.IsNullOrEmpty(input.BloodBagNo))
        //{
        //    queryable = queryable.Where(x => x.BloodBagNo.Contains(input.BloodBagNo!));
        //}

        //// 应用固定的 Where 条件 (例如 status)
        //var status = new List<int> { 1, 2, 3 }; // 示例：请替换为实际的 status 值
        //queryable = queryable.Where(x => status.Contains(x.Status));

        //// 应用默认排序
        //queryable = queryable.OrderBy(x => x.CreateTime, OrderByType.Desc);
        return queryable;
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>结果</returns>
    public virtual async Task<TDto> GetAsync(TKey id)
    {
        var mapper = ServiceProvider.GetRequiredService<IMapper>();
        var db = GetSqlSugarClient(ConfigId);
        var  entity = await db.Queryable<TEntity>()
            .Where(it => it.Id!.Equals(id))
            .FirstAsync();
        var dto = mapper.Map<TDto>(entity);
        return dto;
    }

    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    /// <returns>结果</returns>
    protected virtual ICurrentUser GetCurrentUser()
    {
        var userInfo = ServiceProvider.GetRequiredService<ICurrentUser>();
        return !userInfo.IsAuthenticated ? throw new CustomException("用户未登录") : userInfo;
    }
}