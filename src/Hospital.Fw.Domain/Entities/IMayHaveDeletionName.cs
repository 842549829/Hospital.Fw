namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可能拥有删除人
/// </summary>
public interface IMayHaveDeletionName : ISoftDelete
{
    /// <summary>
    /// 删除人
    /// </summary>
    string? DeletionName { get; }
}