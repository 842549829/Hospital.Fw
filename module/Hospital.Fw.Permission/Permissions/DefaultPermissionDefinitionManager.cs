namespace Hospital.Fw.Permission.Permissions;

/// <summary>
/// 默认权限定义管理
/// </summary>
public class DefaultPermissionDefinitionManager : IPermissionDefinitionManager
{
    /// <summary>
    /// 是否有权限存在
    /// </summary>
    /// <param name="permissionName">权限名称</param>
    /// <returns>权限</returns>
    public Task<bool> IsPermissionsAsync(string permissionName)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// 是否有权限
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="permissionName">权限名称</param>
    /// <returns>用户权限</returns>
    public Task<bool> IsPermissionsAsync(string userId, string permissionName)
    {
        return Task.FromResult(true);
    }
}