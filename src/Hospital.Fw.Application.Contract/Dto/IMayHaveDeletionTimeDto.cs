namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可有删除时间
/// </summary>
public interface IMayHaveDeletionTimeDto : IEntityDto
{
    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletionTime { get; set; }
}