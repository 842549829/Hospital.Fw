using System.Security.Cryptography;
using System.Text;

namespace Hospital.Fw.Domain.Shared.Core.Encryption;

/// <summary>
/// MD5 加密帮助类
/// </summary>
public static class Md5Helper
{
    /// <summary>
    /// 对字符串进行 MD5 加密
    /// </summary>
    /// <param name="input">要加密的字符串</param>
    /// <param name="isUpper">是否返回大写，默认 false</param>
    /// <returns>MD5 加密后的字符串</returns>
    public static string Encrypt(string input, bool isUpper = false)
    {
        using var md5 = MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString(isUpper ? "X2" : "x2")); // X2 大写，x2 小写
        }

        return sb.ToString();
    }

    /// <summary>
    /// 密码加密
    /// </summary>
    /// <param name="input">密码</param>
    /// <returns>结果</returns>
    public static string PwdEncrypt(string input)
    {
        const string chars = "A3F9B8C7D2E1XAKE";
        return Encrypt($"{input}{chars}");
    }
}