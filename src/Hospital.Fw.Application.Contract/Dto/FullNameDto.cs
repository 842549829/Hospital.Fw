using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带名称的通用DTO
/// </summary>
public abstract class FullNameDto : MultiTenantFullAuditedDto, IHasNameDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;
}