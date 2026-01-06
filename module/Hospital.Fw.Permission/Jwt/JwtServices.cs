using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hospital.Fw.Permission.Jwt;

/// <summary>
/// JWT服务
/// </summary>
public class JwtServices : IJwtServices
{
    /// <summary>
    /// 配置
    /// </summary>
    private readonly IOptions<JwtPermissionOptions> _options;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger<JwtServices> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options">配置</param>
    /// <param name="logger">日志</param>
    public JwtServices(IOptions<JwtPermissionOptions> options,
        ILogger<JwtServices> logger)
    {
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// 创建AccessToken
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns>token</returns>
    public string GenerateAccessToken(User user)
    {
        var jwtOptions = _options.Value;

        // 定义用户信息
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Name, user.UserName),
            new("nick", user.NickName),
            new("tenant", user.TenantId),
            new("orgid", user.OrganizationId),
            new("orgcode", user.OrganizationCode)

        };

        if (!string.IsNullOrWhiteSpace(user.Avatar))
        {
            claims.Add(new Claim("avatar", user.Avatar));
        }
        if (!string.IsNullOrWhiteSpace(user.Phone))
        {
            claims.Add(new Claim("phone", user.Phone));
        }
        if(!string.IsNullOrWhiteSpace(user.Email))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        }
        if (user.Roles != null)
        {
            claims.AddRange(user.Roles.Select(role => new Claim("role", role)));
        }
        if (user.Permissions != null)
        {
            claims.AddRange(user.Permissions.Select(permission => new Claim("permission", permission)));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(jwtOptions.AccessTokenExpirationInMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return jwtToken;
    }

    /// <summary>
    /// 解析并验证 JWT Token
    /// </summary>
    /// <param name="accesstoken">JWT Token 字符串</param>
    /// <returns>ClaimsPrincipal 或 null（验证失败）</returns>
    public ClaimsPrincipal? ValidateAccessToken(string accesstoken)
    {
        return ValidateToken(accesstoken);
    }

    /// <summary>
    /// 生成刷新Token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns>刷新Token</returns>

    public string GenerateRefreshToken(User user)
    {
        var jwtOptions = _options.Value;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new("tenant", user.TenantId),
            new("orgid", user.OrganizationId),
            new("orgcode", user.OrganizationCode),
            new("ref", "ref")
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer, 
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(jwtOptions.RefreshTokenExpirationInDays),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// 验证 Refresh Token (也是一个 JWT 验证)
    /// </summary>
    /// <param name="refreshToken">刷新token</param>
    /// <returns>ClaimsPrincipal 或 null（验证失败）</returns>
    public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
    {
        var claimsPrincipal =  ValidateToken(refreshToken);
        return claimsPrincipal?.FindFirst("ref")?.Value == "ref" ? claimsPrincipal : null;
    }

    /// <summary>
    /// 验证token 
    /// </summary>
    /// <param name="token">token</param>
    /// <returns>ClaimsPrincipal?</returns>
    private ClaimsPrincipal? ValidateToken(string token)
    {
        var jwtOptions = _options.Value;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,    // 确保验证过期时间
                IssuerSigningKey = key,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                ClockSkew = TimeSpan.FromMinutes(5) // 允许时间偏差(5分钟)
            };

            // 验证 Token 并获取主体
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            // 可选：确保解析的是 JWT 格式 Token
            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                // Token 类型不匹配或算法错误
                return null;
            }

            return principal;
        }
        catch (SecurityTokenExpiredException)
        {
            // Token 已过期
            // 使用 ILogger 记录日志
            _logger.LogWarning("Token has expired.");
            return null;
        }
        catch (SecurityTokenSignatureKeyNotFoundException)
        {
            // 签名密钥未找到
            _logger.LogWarning("Token signature key not found.");
            return null;
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            // 签名无效
            _logger.LogWarning("Token signature is invalid.");
            return null;
        }
        catch (SecurityTokenInvalidIssuerException)
        {
            // 发行者不合法
            _logger.LogWarning("Token issuer is invalid.");
            return null;
        }
        catch (SecurityTokenInvalidAudienceException)
        {
            // 接收方不合法
            _logger.LogWarning("Token audience is invalid.");
            return null;
        }
        catch (Exception ex)
        {
            // 其他异常（如格式错误）
            _logger.LogError(ex, "Token validation failed.");
            return null;
        }
    }
}