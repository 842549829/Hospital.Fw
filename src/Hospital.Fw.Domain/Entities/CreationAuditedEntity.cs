using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 创建审计实体
/// </summary>
/// <typeparam name="TKey">TKey</typeparam>
public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, IHasCreator
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(IsNullable = false, ColumnDescription = "创建时间")]
    public virtual DateTime CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 创建人
    /// </summary>
    [SugarColumn(IsNullable = true, Length = SqlSugarCoreDbConst.SugarColumnLength64, ColumnDescription = "创建人")]
    public virtual string? CreatorName { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    [SugarColumn(IsNullable = true, Length = SqlSugarCoreDbConst.SugarColumnLength32, ColumnDescription = "创建人Id")]
    public virtual string? CreatorId { get; set; }
}