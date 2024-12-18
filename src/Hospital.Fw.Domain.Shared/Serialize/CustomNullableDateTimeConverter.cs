using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hospital.Fw.Domain.Shared.Serialize;

/// <summary>
/// 自定义日期时间解析器
/// </summary>
/// <param name="dateFormat"></param>
public class CustomNullableDateTimeConverter(string dateFormat) : JsonConverter<DateTime?>
{
    /// <summary>
    /// 日期时间格式
    /// </summary>
    private readonly string _dateFormat = dateFormat ?? throw new ArgumentNullException(nameof(dateFormat));

    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="reader">Utf8JsonReader</param>
    /// <param name="typeToConvert">Type</param>
    /// <param name="options">JsonSerializerOptions</param>
    /// <returns>DateTime</returns>
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var dateString = reader.GetString();
            if (DateTime.TryParseExact(dateString, _dateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
        }

        // 如果解析失败或者不是预期的 token 类型，则抛出异常或返回默认值
        throw new JsonException($"无法将 JSON 值 {reader.GetString()} 解析为指定格式 {_dateFormat} 的日期时间。");
    }

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="writer">Utf8JsonWriter</param>
    /// <param name="value">DateTime</param>
    /// <param name="options">JsonSerializerOptions</param>
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString(_dateFormat));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}