using Microsoft.Extensions.Caching.Memory;

namespace Hospital.Fw.Domain.Manager.Permissions;

/// <summary>
/// 权限缓存服务
/// </summary>
/// <param name="memoryCache">内存缓存</param>
public class PermissionCacheManager(IMemoryCache memoryCache)
    : BaseManager, IPermissionCacheManager
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    private readonly IMemoryCache _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));

    /// <summary>
    /// 缓存选项
    /// </summary>
    private static readonly MemoryCacheEntryOptions CacheOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // 30分钟滑动过期
        .SetAbsoluteExpiration(TimeSpan.FromHours(6));   // 6小时绝对过期

    /// <summary>
    /// 判断权限
    /// </summary>
    /// <param name="permissionName">权限名称</param>
    /// <returns>是否有权限</returns>
    public bool HasPermission(string permissionName)
    {
        if (string.IsNullOrWhiteSpace(permissionName))
        {
            return false;
        }
        // 尝试从缓存获取
        return _memoryCache.TryGetValue(permissionName, out bool hasPermission) && hasPermission;
    }

    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="permissionName">权限名称</param>
    /// <param name="hasPermission">是否有权限</param>
    /// <returns>结果</returns>
    public bool SetPermission(string permissionName, bool hasPermission)
    {
        return !string.IsNullOrWhiteSpace(permissionName) && _memoryCache.Set(permissionName, hasPermission, CacheOptions);
    }

    /// <summary>
    /// 移除权限
    /// </summary>
    /// <param name="permissionName">权限名称</param>
    public void RemovePermission(string permissionName)
    {
        if (string.IsNullOrWhiteSpace(permissionName))
        {
            return;
        }
        _memoryCache.Remove(permissionName);
    }
}