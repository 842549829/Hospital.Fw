namespace Hospital.Fw.Permission.Jwt;

/// <summary>
/// JWT权限配置
/// </summary>
public class JwtPermissionOptions
{
    /// <summary>
    /// 签发者
    /// </summary>
    public string Issuer { get; init; } = "Issuer";

    /// <summary>
    /// 接收者
    /// </summary>
    public string Audience { get; init; } = "Audience";

    /// <summary>
    /// 密钥
    /// </summary>
    public string Secret { get; init; } = "Secret";

    /// <summary>
    /// 是否生成刷新Token
    /// </summary>
    public bool IsRefreshToken { get; init; } = true;

    /// <summary>
    /// AccessToken过期时间
    /// </summary>
    public int AccessTokenExpirationInMinutes { get; init; } = 120;

    /// <summary>
    ///  RefreshToken过期时间
    /// </summary>
    public int RefreshTokenExpirationInDays { get; init; }  = 31;
}