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
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength32, ColumnDescription = "删除人Id")]
    public string? DeletionId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [SugarColumn(ColumnDescription = "删除时间")]
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// 删除人
    /// </summary>
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength64, ColumnDescription = "删除人")]
    public string? DeletionName { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    [SugarColumn(ColumnDataType = SqlSugarCoreDbConst.SugarColumnBool, ColumnDescription = "是否删除")]
    public bool IsDeleted { get; set; } 
}