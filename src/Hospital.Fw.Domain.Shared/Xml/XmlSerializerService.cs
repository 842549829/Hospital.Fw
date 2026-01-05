using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Hospital.Fw.Domain.Shared.Xml;

/// <summary>
/// XML 序列化和反序列化的实现
/// </summary>
public class XmlSerializerService : IXmlSerializerService
{
    /// <summary>
    /// 将对象序列化为 XML 字符串
    /// </summary>
    /// <typeparam name="T">要序列化的对象类型 (必须是引用类型)</typeparam>
    /// <param name="obj">要序列化的对象</param>
    /// <param name="omitXmlDeclaration">是否省略 XML 声明 (e.g., )</param>
    /// <param name="encoding">指定输出 XML 字符串的编码声明 (例如 "utf-8")</param>
    /// <param name="indent">是否对输出的 XML 进行缩进格式化</param>
    /// <returns>序列化后的 XML 字符串</returns>
    /// <exception cref="ArgumentNullException">当 obj 为 null 时抛出</exception>
    /// <exception cref="InvalidOperationException">序列化过程中发生错误时抛出</exception>
    public string Serialize<T>(T obj, bool omitXmlDeclaration = false, string encoding = "utf-8", bool indent = true)
        where T : class
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj), "Object to serialize cannot be null.");
        }

        try
        {
            // 为每个类型 T 创建或获取 XmlSerializer 实例
            // 注意：这里没有像之前那样缓存静态实例，因为方法是泛型的。
            // 如果性能是关键，并且会频繁序列化同一种类型，可以考虑引入一个字典来缓存。
            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlWriterSettings
            {
                Indent = indent,
                IndentChars = "  ",
                OmitXmlDeclaration = omitXmlDeclaration,
                Encoding = Encoding.GetEncoding(encoding),
            };

            using (var stringWriter = new CustomStringWriter(Encoding.GetEncoding(encoding)))
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", ""); // 移除默认命名空间声明

                    serializer.Serialize(xmlWriter, obj, namespaces);
                }

                return stringWriter.ToString();
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to serialize object of type {typeof(T)} to XML. See inner exception for details.", ex);
        }
    }

    /// <summary>
    /// 将 XML 字符串反序列化为对象
    /// </summary>
    /// <typeparam name="T">要反序列化的目标对象类型 (必须是引用类型)</typeparam>
    /// <param name="xmlString">要反序列化的 XML 字符串</param>
    /// <returns>反序列化后的对象</returns>
    /// <exception cref="ArgumentException">当 xmlString 为 null 或空时抛出</exception>
    /// <exception cref="InvalidOperationException">反序列化过程中发生错误时抛出</exception>
    public T Deserialize<T>(string xmlString) where T : class
    {
        if (string.IsNullOrWhiteSpace(xmlString))
        {
            throw new ArgumentException("XML string cannot be null or empty.", nameof(xmlString));
        }

        try
        {
            // 为每个类型 T 创建或获取 XmlSerializerService 实例
            var serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(xmlString))
            {
                var result = serializer.Deserialize(stringReader);

                if (result is T deserializedObj)
                {
                    return deserializedObj;
                }
                else
                {
                    // 理论上不应发生，因为 Serializer 是针对 typeof(T) 创建的
                    throw new InvalidOperationException(
                        $"Deserialization returned an unexpected type. Expected {typeof(T)}, but got {result?.GetType() ?? typeof(object)}.");
                }
            }
        }
        catch (InvalidOperationException)
        {
            // 重新抛出 XmlSerializerService 的主要异常
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to deserialize XML to object of type {typeof(T)}. See inner exception for details.", ex);
        }
    }

    /// <summary>
    /// 这对于 XmlWriter 正确生成带有 encoding="utf-8" 的声明很重要
    /// </summary>
    /// <param name="encoding">encoding</param>
    private class CustomStringWriter(Encoding encoding) : StringWriter
    {
        public override Encoding Encoding { get; } = encoding ?? throw new ArgumentNullException(nameof(encoding));
    }
}