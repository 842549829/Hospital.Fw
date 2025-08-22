using Hospital.Fw.Permission;
using Hospital.Fw.Permission.Permissions;

namespace Hospital.Fw.PermissionTest.Permissions;

public static class PermissionTestExtension
{
    /// <summary>
    /// 添加权限服务
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="configuration">configuration</param>
    public static void AddPermissionsTest(this IServiceCollection services, IConfiguration configuration)
    {
        // 注入默认权限定义管理器
        services.AddPermissions(configuration);

        // 注入默认权限定义管理器
        services.AddSingleton<IPermissionDefinitionManager, TestPermissionDefinitionManager>();
    }
}

/// <summary>
/// 默认权限定义管理
/// </summary>
public class TestPermissionDefinitionManager : IPermissionDefinitionManager
{
    /// <summary>
    /// 获取所有权限
    /// </summary>
    /// <returns>权限</returns>
    public Task<IReadOnlyList<string>> GetAllPermissionsAsync()
    {
        IReadOnlyList<string> result = new List<string>
        {
            "A1",
            "A2",
            "A3",
            "A4",
            "A5"
        };
        return Task.FromResult(result);
    }

    /// <summary>
    /// 是否有权限存在
    /// </summary>
    /// <param name="permissionName">权限名称</param>
    /// <returns>权限</returns>
    public async Task<bool> IsPermissionsAsync(string permissionName)
    {
        var result = (await GetAllPermissionsAsync()).Any(x => x == permissionName);
        return result;
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