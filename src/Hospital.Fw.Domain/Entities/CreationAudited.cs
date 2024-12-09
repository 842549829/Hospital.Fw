using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 创建审计实体
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class CreationAudited<TKey> : Entity<TKey>, IHasCreator
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间")]
    public DateTime CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 创建人
    /// </summary>
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength64, ColumnDescription = "创建人")]
    public string? CreatorName { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength32, ColumnDescription = "创建人Id")]
    public string? CreatorId { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [SugarColumn(ColumnDataType = SqlSugarCoreDbConst.SugarColumnBool, ColumnDescription = "是否启用")]
    public bool IsEnabled { get; set; } = true!;
}