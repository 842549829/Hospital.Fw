namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可有删除Id
/// </summary>
public interface IMayHaveDeletionIdDto : IEntityDto
{
    /// <summary>
    /// 删除人标识
    /// </summary>
    public string? DeletionId { get; set; }
}