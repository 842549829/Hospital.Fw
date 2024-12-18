using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hospital.Fw.Domain.Shared.Serialize;

/// <summary>
/// 自定义日期时间解析器
/// </summary>
/// <param name="format"></param>
public class DateTimeConverterUsingDateTimeParse(string format) : JsonConverter<DateTime>
{
    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="reader">Utf8JsonReader</param>
    /// <param name="typeToConvert">Type</param>
    /// <param name="options">JsonSerializerOptions</param>
    /// <returns>DateTime</returns>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString() ?? string.Empty, format, null);
    }

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="writer">Utf8JsonWriter</param>
    /// <param name="value">DateTime</param>
    /// <param name="options">JsonSerializerOptions</param>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(format));
    }
}