namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 根审计数据传输对象
/// </summary>
public class RootAuditedDto : DeleteDto, IEntityDto<string>
{
    /// <summary>
    /// 主键
    /// </summary>
    public required string Id { get; init; }
}