namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可有删除人
/// </summary>
public interface IMayHaveDeletionNameDto : ISoftDeleteDto
{
    /// <summary>
    /// 删除人
    /// </summary>
    public string? DeletionName { get; set; }
}