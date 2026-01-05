namespace Hospital.Fw.Domain.Manager.Permissions;

/// <summary>
/// 权限缓存服务
/// </summary>
public interface IPermissionCacheManager : IBaseManager
{
    /// <summary>
    /// 判断权限
    /// </summary>
    /// <param name="permissionName">权限名称</param>
    /// <returns>是否有权限</returns>
    bool HasPermission(string permissionName);

    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="permissionName">权限名称</param>
    /// <param name="hasPermission">是否有权限</param>
    /// <returns>结果</returns>
    bool SetPermission(string permissionName, bool hasPermission);

    /// <summary>
    /// 移除权限
    /// </summary>
    /// <param name="permissionName">权限名称</param>
    void RemovePermission(string permissionName);
}