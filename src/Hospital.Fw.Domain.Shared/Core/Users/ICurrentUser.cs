namespace Hospital.Fw.Domain.Shared.Core.Users;

/// <summary>
/// 当前用户
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// 是否有登录
    /// </summary>
    public bool IsAuthenticated { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public string[]? Roles { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    public string[]? Permissions { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public string TenantId { get; set; }

    /// <summary>
    /// 机构Id
    /// </summary>
    public string OrganizationId { get; set; }

    /// <summary>
    /// 机构Code
    /// </summary>
    public string OrganizationCode { get; set; }
}