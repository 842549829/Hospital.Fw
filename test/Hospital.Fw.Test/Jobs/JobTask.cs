using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Test.Jobs;

/// <summary>
/// 任务
/// </summary>
[Tenant(SqlSugarCoreDbConst.Job)]
[SugarTable("JOB_TASK")]
public class JobTask
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

    /// <summary>
    /// Try count of this job.
    /// A job is re-tried if it fails.
    /// </summary>
    [SugarColumn(ColumnName = "TRY_COUNT", IsNullable = false)]
    public short TryCount { get; set; }

    /// <summary>
    /// Creation time of this job.
    /// </summary>
    [SugarColumn(ColumnName = "CREATION_TIME", IsNullable = false)]
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Next try time of this job.
    /// </summary>
    [SugarColumn(ColumnName = "NEXT_TRY_TIME", IsNullable = false)]
    public DateTime NextTryTime { get; set; }

    /// <summary>
    /// Last try time of this job.
    /// </summary>
    [SugarColumn(ColumnName = "LAST_TRY_TIME", IsNullable = true)]
    public DateTime? LastTryTime { get; set; }

    /// <summary>
    /// This is true if this job is continuously failed and will not be executed again.
    /// </summary>
    [SugarColumn(ColumnName = "IS_ABANDONED", IsNullable = false)]
    public bool IsAbandoned { get; set; }

    [SugarColumn(ColumnName = "PRIORITY", IsNullable = false)]
    public BackgroundJobPriority Priority { get; set; }
}