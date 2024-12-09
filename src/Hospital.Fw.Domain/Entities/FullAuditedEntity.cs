using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 完整审计实体
/// </summary>
/// <typeparam name="TKey">TKey</typeparam>
public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IMayHaveDeletion
{
    /// <summary>
    /// 删除人Id
    /// </summary>
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength32, IsNullable = true, ColumnDescription = "删除人Id")]
    public virtual string? DeletionId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [SugarColumn(IsNullable = true, ColumnDescription = "删除时间")]
    public virtual DateTime? DeletionTime { get; set; }

    /// <summary>
    /// 删除人
    /// </summary>
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength64, IsNullable = true, ColumnDescription = "删除人")]
    public virtual string? DeletionName { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    [SugarColumn(IsNullable = false, ColumnDescription = "是否删除")]
    public virtual bool IsDeleted { get; set; }
}