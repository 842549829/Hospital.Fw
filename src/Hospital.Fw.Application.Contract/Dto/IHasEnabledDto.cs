namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 启用
/// </summary>
public interface IHasEnabledDto : IEntityDto
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; init; }
}