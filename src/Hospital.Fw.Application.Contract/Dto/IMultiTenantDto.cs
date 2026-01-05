namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 多租户
/// </summary>
public interface IMultiTenantDto : IHasOrganizationCodeDto, IHasOrganizationIdDto, IHasTenantIdDto;