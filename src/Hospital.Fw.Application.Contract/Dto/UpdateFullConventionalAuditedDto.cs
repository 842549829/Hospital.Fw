namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带审计信息-修改
/// </summary>
public abstract class UpdateFullConventionalAuditedDto : CreateFullConventionalAuditedDto, IEntityDto<string>
{
}