using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 带名称的实体
/// </summary>
public abstract class FullNameEntity<TKey> : MultiTenantFullAuditedEntity<TKey>, IHasName
{
    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = false, ColumnDescription = "名称")]
    public string Name { get; set; } = null!;
}