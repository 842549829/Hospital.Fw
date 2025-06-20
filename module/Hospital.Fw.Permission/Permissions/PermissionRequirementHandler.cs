using Microsoft.AspNetCore.Authorization;

namespace Hospital.Fw.Permission.Permissions;

/// <summary>
/// 权限验证处理
/// </summary>
public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    /// <summary>
    /// 权限验证处理
    /// </summary>
    /// <param name="context">当前上下文</param>
    /// <param name="requirement">权限策略</param>
    /// <returns>Task</returns>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // 获取用户主体信息
        var user = context.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            // 获取用户权限
            var permissions = user.Claims.Where(x => x.Type == "permission").Select(x => x.Value).ToList();
            // 判断用户是否拥有权限
            if (permissions.Contains(requirement.PermissionName))
            {
                // 添加权限
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }
}