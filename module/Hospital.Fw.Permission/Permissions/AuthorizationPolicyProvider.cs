using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Hospital.Fw.Permission.Permissions;

/// <summary>
/// 实现根据策略名称获取策略
/// </summary>
/// <param name="options">权限配置</param>
/// <param name="permissionDefinitionManager">权限定义</param>
public class AuthorizationPolicyProvider(
    IOptions<AuthorizationOptions> options,
    IPermissionDefinitionManager permissionDefinitionManager)
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
        {
            return policy;
        }

        var permission = permissionDefinitionManager.GetPermissions();
        if (permission.All(d => d != policyName))
        {
            return null;
        }
        var policyBuilder = new AuthorizationPolicyBuilder([]);
        policyBuilder.Requirements.Add(new PermissionRequirement(policyName));
        return policyBuilder.Build();

    }
}