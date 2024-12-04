namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可能拥有最后修改人
/// </summary>
public interface IMayHaveLastModificationName
{
    /// <summary>
    /// 最后修改人
    /// </summary>
    string? LastModificationName { get; }
}