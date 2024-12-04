namespace Hospital.Fw.Application.Contract;

/// <summary>
/// 分页查询返回对象
/// </summary>
[Serializable]
public class PageListDto<T>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="items">数据</param>
    public PageListDto(List<T> items)
    {
        Items = items;
    }

    /// <summary>
    /// 总条数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 当前页数据
    /// </summary>
    public List<T> Items { get; set; }
}