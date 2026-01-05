namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 创建者
/// </summary>
public interface IHasCreatorDto : IEntityDto
{
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 创建者
    /// </summary>
    public string? CreatorName { get; init; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public string? CreatorId { get; init; }
}