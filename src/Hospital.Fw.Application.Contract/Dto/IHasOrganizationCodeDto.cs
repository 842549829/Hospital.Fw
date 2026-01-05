namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 组织code
/// </summary>
public interface IHasOrganizationCodeDto : IEntityDto
{
    /// <summary>
    /// 组织code
    /// </summary>
    string OrganizationCode { get; set; }
}