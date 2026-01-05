namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 默认
/// </summary>
public interface IHasDefaultDto : IEntityDto
{
    /// <summary>
    /// 是否默认
    /// </summary>
    public bool IsDefault { get; set; }
}