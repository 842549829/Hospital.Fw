namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
///  创建审计
/// </summary>
public abstract class CreationAuditedDto : EntityDto<string>, ICreateDto
{
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatorName { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    public string? CreatorId { get; set; }
}