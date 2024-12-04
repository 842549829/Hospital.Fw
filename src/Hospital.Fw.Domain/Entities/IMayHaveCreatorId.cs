namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可有创建人标识
/// </summary>
public interface IMayHaveCreatorId
{
    /// <summary>
    /// 创建人标识
    /// </summary>
    string? CreatorId { get; }
}