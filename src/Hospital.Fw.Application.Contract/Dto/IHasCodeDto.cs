namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带编码的Dto
/// </summary>
public interface IHasCodeDto : IEntityDto
{
    /// <summary>
    /// Code
    /// </summary>
    public string Code { get; init; }
}