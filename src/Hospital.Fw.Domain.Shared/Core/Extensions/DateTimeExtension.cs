namespace Hospital.Fw.Domain.Shared.Core.Extensions;

/// <summary>
/// 时间相关扩展方法
/// </summary>
public static class DateTimeExtension
{
    /// <summary>
    /// 获取当前时间的时间戳
    /// </summary>
    /// <returns>时间戳</returns>
    public static long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}