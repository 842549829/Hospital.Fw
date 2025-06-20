namespace Hospital.Fw.Permission.Jwt;

/// <summary>
/// 用户信息
/// </summary>
public class User
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; } = default!;

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; } = default!;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// 手机
    /// </summary>
    public string Phone { get; set; } = default!;

    /// <summary>
    /// 角色
    /// </summary>
    public string[] Roles { get; set; } = default!;

    /// <summary>
    /// 权限
    /// </summary>
    public string[] Permissions { get; set; } = default!;
}