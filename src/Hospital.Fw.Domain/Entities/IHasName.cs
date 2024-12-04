namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可有名称
/// </summary>
public interface IHasName
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get;  }
}