namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带名称的Dto
/// </summary>
public interface IHasNameDto : IEntityDto
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; init; }
}