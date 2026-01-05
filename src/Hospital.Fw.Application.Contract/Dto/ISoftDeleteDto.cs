namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 删除标记
/// </summary>
public interface ISoftDeleteDto
{
    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }
}