namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 带代码的实体
/// </summary>
public interface IHasCode
{
    /// <summary>
    /// 代码
    /// </summary>
    public string Code { get; }
}