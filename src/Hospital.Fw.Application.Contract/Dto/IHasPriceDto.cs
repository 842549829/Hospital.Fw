namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 价格
/// </summary>
public interface IHasPriceDto : IEntityDto
{
    /// <summary>
    ///  价格
    /// </summary>
    public decimal Price { get; set; }
}