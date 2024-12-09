using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 审计实体
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class AuditedEntity<TKey> : CreationAudited<TKey>, IMayHaveLastModification
{  
    /// <summary>
    /// 最后修改人Id
    /// </summary>
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength32, ColumnDescription = "最后修改人Id")]
    public string? LastModificationId { get; set; }

    /// <summary>
    /// 最后修改人
    /// </summary>
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength64, ColumnDescription = "最后修改人")]
    public string? LastModificationName { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    [SugarColumn(ColumnDescription = "最后修改时间")]
    public DateTime? LastModificationTime { get; set; }
}