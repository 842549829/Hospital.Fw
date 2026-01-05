using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 启用排序审计
/// </summary>
public abstract class FullEnableSortAuditedDto : FullCodeDto, IHasEnabledDto, IHasSortDto
{
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required]
    public bool IsEnabled { get; init; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int Sort { get; init; } = 0;
}