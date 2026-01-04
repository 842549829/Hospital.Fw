namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 租户Id
/// </summary>
internal interface IHasTenantId
{
    /// <summary>
    /// 组织Id
    /// </summary>
    public string OrganizationId { get; set; }
}