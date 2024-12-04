namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 实体基类
/// </summary>
public interface IHasCreatorTime
{
    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime CreateTime { get;}
}