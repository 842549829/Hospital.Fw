namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 排序
/// </summary>
public interface IHasSortDto : IEntityDto
{
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; } 
}