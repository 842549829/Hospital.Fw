namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 审计字段
/// </summary>
public abstract class AuditedDto : CreationAuditedDto, IMayHaveLastModificationDto
{
    /// <summary>
    /// 最后修改Id
    /// </summary>
    public string? LastModificationId { get; set; }

    /// <summary>
    /// 最后修改人
    /// </summary>
    public string? LastModificationName { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? LastModificationTime { get; set; }
}