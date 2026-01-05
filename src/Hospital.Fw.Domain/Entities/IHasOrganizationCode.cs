namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 组织Code
/// </summary>
public interface IHasOrganizationCode
{
    /// <summary>
    /// 组织code
    /// </summary>
    string OrganizationCode { get; set; }
}