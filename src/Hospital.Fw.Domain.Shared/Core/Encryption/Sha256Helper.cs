using System.Security.Cryptography;
using System.Text;

namespace Hospital.Fw.Domain.Shared.Core.Encryption;

/// <summary>
/// HMAC-SHA256 签名辅助类，用于生成和验证签名。
/// </summary>
public static class Sha256Helper
{
    /// <summary>
    /// 使用指定密钥对原始字符串生成 HMAC-SHA256 签名（小写十六进制字符串）
    /// </summary>
    /// <param name="message">待签名的原始字符串</param>
    /// <param name="key">密钥（AppSecret）</param>
    /// <returns>签名字符串（如: "a3b2c1d4..."）</returns>
    public static string GenerateSignature(string message, string key)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("待签名消息不能为空", nameof(message));
        }
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("密钥不能为空", nameof(key));
        }

        var messageBytes = Encoding.UTF8.GetBytes(message);
        var keyBytes = Encoding.UTF8.GetBytes(key);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(messageBytes);
        return BytesToHex(hash);
    }

    /// <summary>
    /// 验证提供的签名是否与使用指定密钥对原始字符串计算出的签名匹配
    /// 使用恒定时间比较（FixedTimeEquals）防止时序攻击
    /// </summary>
    /// <param name="message">待验证的原始字符串</param>
    /// <param name="providedSignature">请求中提供的签名（十六进制小写）</param>
    /// <param name="key">密钥（AppSecret）</param>
    /// <returns>签名是否有效</returns>
    public static bool VerifySignature(string message, string providedSignature, string key)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("待验证消息不能为空", nameof(message));
        }
        if (string.IsNullOrWhiteSpace(providedSignature))
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        // 规范化：转小写，去除空格
        providedSignature = providedSignature.Trim().ToLower();

        var expectedSignature = GenerateSignature(message, key);

        // 使用恒定时间比较防止时序攻击
        var expectedBytes = Encoding.UTF8.GetBytes(expectedSignature);
        var providedBytes = Encoding.UTF8.GetBytes(providedSignature);

        return CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes);
    }

    /// <summary>
    /// 将字节数组转换为小写十六进制字符串
    /// </summary>
    /// <param name="bytes">字节数组</param>
    /// <returns>十六进制字符串</returns>
    private static string BytesToHex(byte[] bytes)
    {
        return string.Concat(bytes.Select(b => b.ToString("x2")));
    }
}