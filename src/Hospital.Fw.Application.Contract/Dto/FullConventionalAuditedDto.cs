using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带审计信息
/// </summary>
public abstract class FullConventionalAuditedDto : MultiTenantFullAuditedDto, IMayHaveRemarkDto
{
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(256)]
    public string? Remark { get; init; }
}