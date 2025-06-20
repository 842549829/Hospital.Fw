using Microsoft.AspNetCore.Authorization;

namespace Hospital.Fw.Permission.Permissions;

/// <summary>
/// 权限验证策略
/// </summary>
/// <param name="permissionName">权限名称</param>
public class PermissionRequirement(string permissionName) : IAuthorizationRequirement
{
    /// <summary>
    /// 权限名称
    /// </summary>
    public string PermissionName { get; } = permissionName;

    /// <summary>
    /// 重写ToString
    /// </summary>
    /// <returns>权限名称</returns>
    public override string ToString()
    {
        return PermissionName;
    }
}