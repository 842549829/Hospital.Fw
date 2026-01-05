namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 数量
/// </summary>
public interface IHasCountDto : IEntityDto
{
    /// <summary>
    /// 数量
    /// </summary>
    public decimal Count { get; init; }
}