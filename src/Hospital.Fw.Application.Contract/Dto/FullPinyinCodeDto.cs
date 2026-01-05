using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带启用排序的审计实体
/// </summary>
public abstract class FullPinyinCodeDto : FullPinyinDto, IHasCodeDto
{
    /// <summary>
    /// Code
    /// </summary>
    [Required]
    public string Code { get; set; } = null!;
}