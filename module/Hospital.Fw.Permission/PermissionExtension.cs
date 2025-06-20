using Hospital.Fw.Permission.Jwt;
using Hospital.Fw.Permission.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hospital.Fw.Permission;

/// <summary>
/// 添加权限服务
/// </summary>
public static class PermissionExtension
{
    /// <summary>
    /// 添加权限服务
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="configuration">configuration</param>
    public static void AddPermissions(this IServiceCollection services, IConfiguration configuration)
    {
        // 注入默认权限定义管理器
        services.AddSingleton<IPermissionDefinitionManager, DefaultPermissionDefinitionManager>();
        // 替换默认获取策略实现
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        // 注册权限验证实现
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        // 配置JWT
        services.Configure<JwtPermissionOptions>(configuration.GetSection("Jwt"));
        // 注册认证方式
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection("Jwt").Get<JwtPermissionOptions>();
                if (jwtOptions == null)
                {
                    throw new ArgumentNullException(nameof(jwtOptions), "jwt未配置");
                }
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),    // 加密解密Token的密钥

                    // 是否验证发布者
                    ValidateIssuer = true,
                    // 发布者名称
                    ValidIssuer = jwtOptions.Issuer,

                    // 是否验证订阅者
                    // 订阅者名称
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    // 是否验证令牌有效期
                    ValidateLifetime = true
                };
            });
    }
}