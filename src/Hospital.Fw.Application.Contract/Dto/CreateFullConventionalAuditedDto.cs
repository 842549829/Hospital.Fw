using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带审计信息-创建
/// </summary>
public abstract class CreateFullConventionalAuditedDto : CreateFullEnableSortAuditedDto, IMayHaveRemarkDto
{
    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(256)]
    public string? Remark { get; set; }
}