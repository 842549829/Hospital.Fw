namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 必须时间范围
/// </summary>
public interface IHasDateTimeLength : IEntityDto
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }
}