namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 租户Id
/// </summary>
public interface IHasTenantId
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public string TenantId { get; set; }
}