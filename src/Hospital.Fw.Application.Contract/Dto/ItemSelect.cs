namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 列表项
/// </summary>
public class ItemSelect
{
    /// <summary>
    /// 值
    /// </summary>
    public required string Value { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// 是否可用
    /// </summary>
    public required bool IsEnabled { get; set; }
}