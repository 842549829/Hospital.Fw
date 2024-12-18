using System.Reflection;
using System.Text.Json;

namespace Hospital.Fw.Domain.Shared.Serialize;

/// <summary>
/// Json序列化扩展
/// </summary>
public static class JsonSerializerExtensions
{
    /// <summary>
    /// 动态配置日期时间格式
    /// </summary>
    /// <param name="options">JsonSerializerOptions</param>
    /// <param name="formatMap">Dictionary</param>
    /// <typeparam name="T">类型</typeparam>
    /// <returns>JsonSerializerOptions</returns>
    public static JsonSerializerOptions ConfigureDynamicDateTimeFormats<T>(this JsonSerializerOptions options, Dictionary<string, string> formatMap) where T : class
    {
        var type = typeof(T);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(DateTime))
            {
                if (formatMap.TryGetValue(property.Name, out var dateFormat))
                {
                    options.Converters.Add(new CustomDateTimeConverter(dateFormat));
                }
            }
            if (property.PropertyType == typeof(DateTime?))
            {
                if (formatMap.TryGetValue(property.Name, out var dateFormat))
                {
                    options.Converters.Add(new CustomNullableDateTimeConverter(dateFormat));
                }
            }
        }

        return options;
    }
}