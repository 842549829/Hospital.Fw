using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 创建审计实体
/// </summary>
/// <typeparam name="TKey">TKey</typeparam>
public abstract class Entity<TKey> : IEntity<TKey>
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, Length = SqlSugarCoreDbConst.SugarColumnLength32, ColumnDataType = SqlSugarCoreDbConst.SugarColumnChar, ColumnDescription = "主键")]
    public virtual TKey Id { get; set; } = default!;
}