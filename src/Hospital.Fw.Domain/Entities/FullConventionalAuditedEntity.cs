using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 基础审计实体
/// </summary>
public abstract class FullConventionalAuditedEntity<TKey> : FullEnableSortAuditedEntity<TKey>, IMayHaveRemark
{
    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(Length = 512, IsNullable = true, ColumnDescription = "备注")]
    public string? Remark { get; set; }
}