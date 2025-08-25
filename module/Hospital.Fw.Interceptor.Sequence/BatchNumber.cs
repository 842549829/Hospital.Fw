using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Interceptor.Sequence;

/// <summary>
/// 批次号
/// </summary>
[Tenant(SqlSugarCoreDbConst.Sequence)]
[SugarTable(TableDescription = "批次号")]
public class BatchNumber
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, Length = 32, ColumnDescription = "主键")]
    public string Key { get; set; } = null!;
    
    /// <summary>
    /// 批次号
    /// </summary>
    [SugarColumn(IsNullable = false, ColumnDescription = "批次号")]
    public long Number { get; set; }
}