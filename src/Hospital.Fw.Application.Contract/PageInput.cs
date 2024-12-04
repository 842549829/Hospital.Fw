namespace Hospital.Fw.Application.Contract;

/// <summary>
/// 分页
/// </summary>
public class PageInput
{
    /// <summary>
    /// 页码
    /// </summary>
    public int PageIndex { get; set; } 

    /// <summary>
    /// 每页数量
    /// </summary>
    public int PageSize { get; set; }
}