namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 列表项
/// </summary>
public class ItemSelectDto : EntityDto
{
    /// <summary>
    /// 值
    /// </summary>
    public required string Value { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public required bool IsEnabled { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    public bool? IsDefault { get; set; }
}