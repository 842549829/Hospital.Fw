using Hospital.Fw.BackgroundJobs.Abstractions;

namespace Hospital.Fw.BackgroundJobs.Implementations;

public sealed class BackgroundJobInfo
{
    public string Id { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string JobName { get; set; } = default!;

    /// <summary>
    /// 任务参数
    /// </summary>
    public string JobArgs { get; set; } = default!;

    /// <summary>
    /// 任务失败重试的次数
    /// </summary>
    public short TryCount { get; set; }

    /// <summary>
    /// 创建任务的时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 下一次任务执行的时间
    /// </summary>
    public DateTime NextTryTime { get; set; }

    /// <summary>
    /// 最后一次执行任务的时间
    /// </summary>
    public DateTime? LastTryTime { get; set; }

    /// <summary>
    /// 如果该作业连续失败并且不会再次执行，则会出现这种情况
    /// </summary>
    public bool IsAbandoned { get; set; }

    /// <summary>
    /// Priority of this job.
    /// </summary>
    public BackgroundJobPriority Priority { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundJobInfo"/> class.
    /// </summary>
    public BackgroundJobInfo()
    {
        Priority = BackgroundJobPriority.Normal;
    }
}