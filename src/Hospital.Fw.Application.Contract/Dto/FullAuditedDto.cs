using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带审计的Dto
/// </summary>
public abstract class FullAuditedDto : IHasCreatorDto, IMayHaveLastModificationDto, IEntityDto<string>
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public required string Id { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required]
    public required DateTime CreateTime { get; init; }

    /// <summary>
    /// 创建者
    /// </summary>
    public string? CreatorName { get; init; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public string? CreatorId { get; init; }

    /// <summary>
    /// 最后修改Id
    /// </summary>
    public string? LastModificationId { get; init; }

    /// <summary>
    /// 最后修改人
    /// </summary>
    public string? LastModificationName { get; init; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? LastModificationTime { get; init; }
}