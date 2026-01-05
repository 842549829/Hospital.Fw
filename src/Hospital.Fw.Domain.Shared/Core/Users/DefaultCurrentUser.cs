using Hospital.Fw.Domain.Shared.Core.Autofac;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Hospital.Fw.Domain.Shared.Core.Users;

/// <summary>
/// 当前用户
/// </summary>
public class DefaultCurrentUser : ICurrentUser, IScopedDependency
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="httpContextAccessor">httpContextAccessor</param>
    public DefaultCurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity == null)
        {
            return;
        }

        IsAuthenticated = user.Identity.IsAuthenticated;

        var nameClaim = user.FindFirst(ClaimTypes.Name);
        if (nameClaim != null)
        {
            UserName = nameClaim.Value;
        }
        var emailClaim = user.FindFirst(ClaimTypes.Email);
        if (emailClaim != null)
        {
            Email = emailClaim.Value;
        }
        var phoneClaim = user.FindFirst("phone");
        if (phoneClaim != null)
        {
            Phone = phoneClaim.Value;
        }
        Roles = user.FindAll("role").Select(x => x.Value).ToArray();
        Permissions = user.FindAll("permission").Select(x => x.Value).ToArray();
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (idClaim != null)
        {
            Id = idClaim.Value;
        }
        var nickClaim = user.FindFirst("nick");
        if (nickClaim != null)
        {
            NickName = nickClaim.Value;
        }
        var avatarClaim = user.FindFirst("avatar");
        if (avatarClaim != null)
        {
            Avatar = avatarClaim.Value;
        }

        var tenantClaim = user.FindFirst("tenantId");
        if (tenantClaim != null)
        {
            TenantId = tenantClaim.Value;
        }

        var organizationClaim = user.FindFirst("orgId");
        if (organizationClaim != null)
        {
            OrganizationId = organizationClaim.Value;
        }
    }

    /// <summary>
    /// 是否有登录
    /// </summary>
    public bool IsAuthenticated { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 用户名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; } = null!;

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
    public string TenantId { get; set; } = null!;

    /// <summary>
    /// 机构Id
    /// </summary>
    public string OrganizationId { get; set; } = null!;
}