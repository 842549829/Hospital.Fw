namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可有父级Id
/// </summary>
public interface IMayHaveParentId<TKey>
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public TKey? ParentId { get; set; }
}