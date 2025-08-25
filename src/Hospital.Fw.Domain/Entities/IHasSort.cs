
namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 带排序的实体
/// </summary>
public interface IHasSort
{
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}