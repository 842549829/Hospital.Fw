namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 列表项
/// </summary>
public class ItemSelectDto : IEntityDto
{
    /// <summary>
    /// 值
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// 标签
    /// </summary>
    public required string Label { get; init; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public required bool IsEnabled { get; init; }

    /// <summary>
    /// 是否默认
    /// </summary>
    public bool? IsDefault { get; init; }
}