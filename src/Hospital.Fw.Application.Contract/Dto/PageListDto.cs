using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 分页查询返回对象
/// </summary>
[Serializable]
public class PageListDto<T> : EntityDto
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
    [Required]
    public int Total { get; set; }

    /// <summary>
    /// 当前页数据
    /// </summary>
    public List<T> Items { get; set; }
}