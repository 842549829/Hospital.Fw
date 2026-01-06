using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带名称的审计实体-创建
/// </summary>
public abstract class CreateFullNameDto : EntityDto<string>, IHasNameDto
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;
}