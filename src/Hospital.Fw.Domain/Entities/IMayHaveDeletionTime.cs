namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可能拥有删除时间
/// </summary>
public interface IMayHaveDeletionTime : ISoftDelete
{
    /// <summary>
    /// 删除时间
    /// </summary>
    DateTime? DeletionTime { get; }
}