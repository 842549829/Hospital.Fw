using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 审计创建
/// </summary>
public abstract class CreateDto : EntityDto, ICreateDto
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [Required]
    public DateTime CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatorName { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    public string? CreatorId { get; set; }
}