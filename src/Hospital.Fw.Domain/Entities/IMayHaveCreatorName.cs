namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 实体基类
/// </summary>
public interface IMayHaveCreatorName
{
    /// <summary>
    /// 创建人名称
    /// </summary>
    string? CreatorName { get; }
}