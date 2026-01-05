namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可有备注
/// </summary>
public interface IMayHaveRemarkDto : IEntityDto
{
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; init; }
}