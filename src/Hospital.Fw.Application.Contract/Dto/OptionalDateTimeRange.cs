namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可选时间范围
/// </summary>
public abstract class OptionalDateTimeRange : PageInput
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; init; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; init; }
}