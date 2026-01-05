namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 获取列表输入
/// </summary>
public interface IGetListInput : IEntityDto
{
    /// <summary>
    /// 筛选
    /// </summary>
    public string? Filter { get; set; }
}