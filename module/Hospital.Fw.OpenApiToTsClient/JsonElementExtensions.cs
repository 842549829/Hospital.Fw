using System.Text.Json;

namespace Hospital.Fw.OpenApiToTsClient;

/// <summary>
/// JsonElement 扩展方法
/// </summary>
public static class JsonElementExtensions
{
    /// <summary>
    /// 获取属性值，如果不存在则返回 null
    /// </summary>
    /// <param name="element">Json文档</param>
    /// <param name="propertyName">属性名</param>
    /// <returns>JsonElement?</returns>
    public static JsonElement? GetPropertyOrNull(this JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var value) ? value : null;
    }

    /// <summary>
    /// 获取字符串值，如果 JsonElement 为 null 则返回空字符串
    /// </summary>
    /// <param name="element">Json文档</param>
    /// <returns>字符串</returns>
    public static string GetString(this JsonElement? element) => element?.GetString() ?? string.Empty;

    /// <summary>
    /// 获取布尔值，如果 JsonElement 为 null 则返回 false
    /// </summary>
    /// <param name="element">Json文档</param>
    /// <returns>布尔值</returns>
    public static bool GetBoolean(this JsonElement? element) => element?.GetBoolean() ?? false;

    /// <summary>
    /// 获取 Int32 值，如果 JsonElement 为 null 则返回默认值
    /// </summary>
    /// <param name="element">Json文档</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>Int32 值</returns>
    public static int GetInt32(this JsonElement? element, int defaultValue = 0)
    {
        return element?.TryGetInt32(out var value) == true ? value : defaultValue;
    }

    /// <summary>
    /// 获取 Double 值，如果 JsonElement 为 null 则返回默认值
    /// </summary>
    /// <param name="element">Json文档</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>Double 值</returns>
    public static double GetDouble(this JsonElement? element, double defaultValue = 0.0)
    {
        return element?.TryGetDouble(out var value) == true ? value : defaultValue;
    }
}

