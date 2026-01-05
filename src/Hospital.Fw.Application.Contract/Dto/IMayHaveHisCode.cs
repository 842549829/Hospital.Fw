namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可有his系统编码
/// </summary>
public interface IMayHaveHisCode : IEntityDto
{
    /// <summary>
    /// his系统编码
    /// </summary>
    public string? HisCode { get; set; }
}