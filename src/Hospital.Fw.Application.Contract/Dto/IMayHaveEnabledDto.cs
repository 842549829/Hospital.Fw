namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可有启用
/// </summary>
public interface IMayHaveEnabledDto : IEntityDto
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? IsEnabled { get; init; }
}