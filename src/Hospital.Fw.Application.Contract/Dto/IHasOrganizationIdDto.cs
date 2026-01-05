namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 组织Id
/// </summary>
public interface IHasOrganizationIdDto : IEntityDto
{
    /// <summary>
    /// 组织Id
    /// </summary>
    string OrganizationId { get; set; }
}