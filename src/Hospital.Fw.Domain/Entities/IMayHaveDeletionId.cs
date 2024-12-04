namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可有删除人标识
/// </summary>
public interface IMayHaveDeletionId : ISoftDelete
{
    /// <summary>
    /// 删除人标识
    /// </summary>
    string? DeletionId { get; }
}