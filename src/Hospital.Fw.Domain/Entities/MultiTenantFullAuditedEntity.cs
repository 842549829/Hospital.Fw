using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 多租户审计类
/// </summary>
public abstract class MultiTenantFullAuditedEntity<TKey> : FullAuditedEntity<TKey>, IMultiTenantEntity
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength32, IsNullable = false, ColumnDescription = "租户Id")]
    public string TenantId { get; set; } = null!;

    /// <summary>
    /// 组织Id
    /// </summary>
    [SugarColumn(Length = SqlSugarCoreDbConst.SugarColumnLength32, IsNullable = false, ColumnDescription = "组织Id")]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 组织code
    /// </summary>
    [SugarColumn(Length = 512, IsNullable = false, ColumnDescription = "组织code")]
    public string OrganizationCode { get; set; } = null!;
}