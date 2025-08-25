namespace Hospital.Fw.Domain.Shared.Core;

/// <summary>
/// 枚举项
/// </summary>
public class EnumEntity
{
    /// <summary>
    /// 枚举项英文名
    /// </summary>
    public required string EnName { get; set; }

    /// <summary>
    /// 枚举项值
    /// </summary>
    public required int Value { get; set; }

    /// <summary>
    /// 枚举项中文名
    /// </summary>
    public required string Text { get; set; }
}