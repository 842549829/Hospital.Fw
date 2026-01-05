namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带审计的Dto
/// </summary>
public abstract class FullAuditedDto : AuditedDto, IMayHaveDeletionDto
{
    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 删除人
    /// </summary>
    public string? DeletionName { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// 删除人标识
    /// </summary>
    public string? DeletionId { get; set; }
}