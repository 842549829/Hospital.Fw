namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可排序
/// </summary>
public interface IMayHaveSortDto : IEntityDto
{
    /// <summary>
    /// 排序
    /// </summary>
    public int? Sort { get; set; }
}