using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 多租户
/// </summary>
public abstract class MultiTenantFullAuditedDto : FullEnableSortAuditedDto, IMultiTenantDto
{
    /// <summary>
    /// 组织code
    /// </summary>
    [Required]
    public string OrganizationCode { get; set; } = null!;

    /// <summary>
    /// 组织Id
    /// </summary>
    [Required]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 租户Id
    /// </summary>
    [Required]
    public string TenantId { get; set; } = null!;
}