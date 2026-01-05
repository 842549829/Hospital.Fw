namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 时间范围
/// </summary>
public abstract class MustDateTimeRange : PageInput
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public required DateTime StartTime { get; init; }
    
    /// <summary>
    /// 结束时间
    /// </summary>
    public required DateTime EndTime { get; init; }
}