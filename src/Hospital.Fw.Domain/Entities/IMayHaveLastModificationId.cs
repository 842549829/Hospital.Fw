namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可有最后修改人标识
/// </summary>
public interface IMayHaveLastModificationId
{
    /// <summary>
    /// 最后修改人标识
    /// </summary>
    string? LastModificationId { get; }
}