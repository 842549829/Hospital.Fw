namespace Hospital.Fw.Domain.Shared.Core.Extensions;

/// <summary>
/// 数字扩展
/// </summary>
public static class NumberExtension
{
    /// <summary>
    /// 工具方法：去掉小数点后多余的零
    /// </summary>
    /// <param name="value">待处理的数字</param>
    /// <returns>处理后的数字</returns>
    public static string TrimTrailingZeros(this decimal value)
    {
        return value.ToString("F2").TrimEnd('0').TrimEnd('.');
    }
}