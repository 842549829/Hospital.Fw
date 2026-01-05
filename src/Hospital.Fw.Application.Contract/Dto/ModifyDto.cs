namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 审计编辑
/// </summary>
public class ModifyDto : CreateDto, IModifyDto
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