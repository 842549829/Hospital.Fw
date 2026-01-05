using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 启用排序审计
/// </summary>
public abstract class FullEnableSortAuditedDto : FullPinyinCodeDto, IHasEnabledDto, IHasSortDto
{
    /// <summary>
    /// 是否启用
    /// </summary>
    [Required]
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int Sort { get; set; } = 0;
}