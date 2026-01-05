using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 带启用排序的审计实体
/// </summary>
/// <typeparam name="TKey">TKey</typeparam>
public abstract class FullEnableSortAuditedEntity<TKey> : FullPinyinCodeEntity<TKey>, IHasEnabled, IHasSort
{
    /// <summary>
    /// 是否启用
    /// </summary>
    [SugarColumn(IsNullable = false, ColumnDescription = "是否启用")]
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(IsNullable = false, ColumnDescription = "排序")]
    public int Sort { get; set; }
}