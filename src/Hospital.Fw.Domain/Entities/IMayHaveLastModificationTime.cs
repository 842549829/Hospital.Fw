namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可能拥有最后修改人
/// </summary>
public interface IMayHaveLastModificationTime
{
    /// <summary>
    /// 最后修改时间
    /// </summary>
    DateTime? LastModificationTime { get; }
}