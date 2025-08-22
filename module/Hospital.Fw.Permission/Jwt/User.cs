namespace Hospital.Fw.Permission.Jwt;

/// <summary>
/// 用户信息
/// </summary>
public class User
{
    /// <summary>
    /// 构造函数（支持可选参数）
    /// </summary>
    /// <param name="id">ID（必填）</param>
    /// <param name="userName">用户名（必填）</param>
    /// <param name="nickName">昵称（必填）</param>
    /// <param name="avatar">头像</param>
    /// <param name="email">邮箱</param>
    /// <param name="phone">手机</param>
    /// <param name="roles">角色</param>
    /// <param name="permissions">权限</param>
    public User(
        string id,
        string userName,
        string nickName,
        string? avatar = null,
        string? email = null,
        string? phone = null,
        string[]? roles = null,
        string[]? permissions = null)
    {
        Id = id;
        UserName = userName;
        NickName = nickName;
        Avatar = avatar;
        Email = email;
        Phone = phone;
        Roles = roles;
        Permissions = permissions;
    }

    /// <summary>
    /// 用户Id
    /// </summary>
    public string Id { get; private init; } 

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; private init; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; private init; } 

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; private init; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; private init; } 

    /// <summary>
    /// 手机
    /// </summary>
    public string? Phone { get; private init; }

    /// <summary>
    /// 角色
    /// </summary>
    public string[]? Roles { get; private init; }

    /// <summary>
    /// 权限
    /// </summary>
    public string[]? Permissions { get; private init; } 
}