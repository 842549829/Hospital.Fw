using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 主键
/// </summary>
public abstract class IdDto : IEntityDto<string>
{
    /// <summary>
    /// 主键
    /// </summary>
    [Length(32, 32)]
    [Required]
    public required string Id { get; init; }
}