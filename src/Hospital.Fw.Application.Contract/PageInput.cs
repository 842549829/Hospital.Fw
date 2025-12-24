using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract;

/// <summary>
/// 分页
/// </summary>
public class PageInput : EntityDto
{
    /// <summary>
    /// 页码
    /// </summary>
    [Required]
    public int PageIndex { get; set; }

    /// <summary>
    /// 每页数量
    /// </summary>
    [Required]
    public int PageSize { get; set; }
}