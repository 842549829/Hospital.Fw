using Hospital.Fw.Domain.Entities;
using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Api.Test.Entities;

/// <summary>
/// 任务
/// </summary>
[Tenant(SqlSugarCoreDbConst.Job)]
[SugarTable("JOB_TASK")]
public class JobTask : IEntity<string>
{
    /// <summary>
    /// 主键
    /// </summary>
    [SugarColumn(ColumnName = "ID", Length = SqlSugarCoreDbConst.SugarColumnLength32, IsPrimaryKey = true)]
    public string Id { get; set; } = null!;

    /// <summary>
    /// Type of the job.
    /// It's AssemblyQualifiedName of job type.
    /// </summary>
    [SugarColumn(ColumnName = "JOB_NAME", Length = SqlSugarCoreDbConst.SugarColumnLength256, IsNullable = false)]
    public string JobName { get; set; } = null!;

    /// <summary>
    /// Job arguments as serialized string.
    /// </summary>
    [SugarColumn(ColumnName = "JOB_ARGS", Length = SqlSugarCoreDbConst.SugarColumnLength4096, IsNullable = false)]
    public string JobArgs { get; set; } = null!;
}