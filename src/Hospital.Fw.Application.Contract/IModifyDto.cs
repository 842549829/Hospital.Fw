namespace Hospital.Fw.Application.Contract;

/// <summary>
/// 修改Dto
/// </summary>
public interface IModifyDto
{
    /// <summary>
    /// 最后修改人Id
    /// </summary>
    public string? LastModificationId { get; set; }

    /// <summary>
    /// 最后修改人
    /// </summary>
    public string? LastModificationName { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? LastModificationTime { get; set; }
}