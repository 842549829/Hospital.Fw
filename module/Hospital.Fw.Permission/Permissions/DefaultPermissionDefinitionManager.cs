namespace Hospital.Fw.Permission.Permissions;

/// <summary>
/// 默认权限定义管理
/// </summary>
public class DefaultPermissionDefinitionManager : IPermissionDefinitionManager
{
    /// <summary>
    /// 获取权限
    /// </summary>
    /// <returns>权限</returns>
    public IReadOnlyList<string> GetPermissions()
    {
        return new List<string>();
    }
}