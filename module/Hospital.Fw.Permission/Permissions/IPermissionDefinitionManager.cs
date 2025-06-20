namespace Hospital.Fw.Permission.Permissions;

/// <summary>
/// 权限定义管理器
/// </summary>
public interface IPermissionDefinitionManager
{
    /// <summary>
    /// 获取权限
    /// </summary>
    /// <returns>权限</returns>
    IReadOnlyList<string> GetPermissions();
}