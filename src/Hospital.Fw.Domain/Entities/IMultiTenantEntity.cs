namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 多租户
/// </summary>
public interface IMultiTenantEntity : IHasTenantId, IHasOrganizationId, IHasOrganizationCode;