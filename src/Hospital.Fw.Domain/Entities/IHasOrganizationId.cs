namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 组织Id
/// </summary>
public interface IHasOrganizationId
{
    /// <summary>
    /// 组织Id
    /// </summary>
    public string OrganizationId { get; set; }
}