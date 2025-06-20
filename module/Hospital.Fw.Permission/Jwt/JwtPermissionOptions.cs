namespace Hospital.Fw.Permission.Jwt;

/// <summary>
/// JWT权限配置
/// </summary>
public class JwtPermissionOptions
{
    /// <summary>
    /// 签发者
    /// </summary>
    public string Issuer { get; init; } = null!;

    /// <summary>
    /// 接收者
    /// </summary>
    public string Audience { get; init; } = null!;

    /// <summary>
    /// 密钥
    /// </summary>
    public string Secret { get; init; } = null!;

    /// <summary>
    /// 单位分钟
    /// </summary>
    public int Expires { get; init; } = 3600;
}