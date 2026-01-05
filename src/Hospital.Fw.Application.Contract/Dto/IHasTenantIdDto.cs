namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 租户Id
/// </summary>
public interface IHasTenantIdDto : IEntityDto
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public string TenantId { get; set; }
}