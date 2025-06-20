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
    /// 获取权限
    /// </summary>
    /// <returns>权限</returns>
    public IReadOnlyList<string> GetPermissions()
    {
        return new List<string>
        {
            "A1",
            "A2",
            "A3",
            "A4",
            "A5"
        };
    }
}