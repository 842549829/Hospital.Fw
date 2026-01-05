namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// RH因子
/// </summary>
public interface IHasRhFactorDto : IEntityDto
{
    /// <summary>
    /// RH因子
    /// </summary>
    public string RhFactor { get; init; }
}