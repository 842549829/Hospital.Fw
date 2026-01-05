using System.Security.Cryptography;

namespace Hospital.Fw.Domain.Shared.Core.Extensions;

/// <summary>
/// 字符串相关扩展方法
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// 获取固定长度的随机字符串
    /// </summary>
    /// <param name="length">字符串长度</param>
    /// <returns>随机字符串</returns>
    public static string GetRandomString(int length)
    { 
        // ReSharper disable once StringLiteralTypo
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random(CreateRandomSeed());
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    /// <summary>
    /// 随机种子
    /// </summary>
    /// <returns>结构</returns>
    private static int CreateRandomSeed()
    {
        var bytes = new byte[4];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
}