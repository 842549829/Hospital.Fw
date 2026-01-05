using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 获取列表输入
/// </summary>
public abstract class GetListInput : PageInput, IGetListInput
{
    /// <summary>
    /// 筛选
    /// </summary>
    [MaxLength(32)]
    public string? Filter { get; set; }
}