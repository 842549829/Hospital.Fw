using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 启用排序审计-创建
/// </summary>
public abstract class CreateFullEnableSortAuditedDto : CreateFullPinyinCodeDto, IHasEnabledDto, IHasSortDto
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