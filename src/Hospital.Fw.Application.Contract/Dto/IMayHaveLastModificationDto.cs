namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可有最后修改
/// </summary>
public interface IMayHaveLastModificationDto : IEntityDto
{
    /// <summary>
    /// 最后修改Id
    /// </summary>
    public string? LastModificationId { get; init; }

    /// <summary>
    /// 最后修改人
    /// </summary>
    public string? LastModificationName { get; init; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? LastModificationTime { get; init; }
}