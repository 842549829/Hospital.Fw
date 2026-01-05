using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带编码的通用DTO
/// </summary>
public abstract class FullCodeDto : IHasNameDto, IHasCodeDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [Length(1, 32)]
    [Required]
    public required string Name { get; init; }

    /// <summary>
    /// Code
    /// </summary>
    [Length(1, 32)]
    [Required]
    public required string Code { get; init; }
}