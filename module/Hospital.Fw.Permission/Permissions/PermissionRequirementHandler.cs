using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Hospital.Fw.Permission.Permissions;

/// <summary>
/// 权限验证处理
/// </summary>
public class PermissionRequirementHandler(IPermissionDefinitionManager permissionDefinitionManager) : AuthorizationHandler<PermissionRequirement>
{
    /// <summary>
    /// 权限验证处理
    /// </summary>
    /// <param name="context">当前上下文</param>
    /// <param name="requirement">权限策略</param>
    /// <returns>Task</returns>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
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
                context.Succeed(requirement);
            }
            else
            {
                var userId = user.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (userId != null)
                {
                    if (await permissionDefinitionManager.IsPermissionsAsync(userId, requirement.PermissionName))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
                context.Fail();
            }
        }
        else
        {
            context.Fail();
        }
    }
}