using Hospital.Fw.Domain.Shared.Core.Autofac;

namespace Hospital.Fw.Domain.Shared.Xml;

/// <summary>
/// 定义 XML 序列化和反序列化的通用接口
/// </summary>
public interface IXmlSerializerService : ISingletonDependency
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
    string Serialize<T>(T obj, bool omitXmlDeclaration = false, string encoding = "utf-8", bool indent = true)
        where T : class;

    /// <summary>
    /// 将 XML 字符串反序列化为对象
    /// </summary>
    /// <typeparam name="T">要反序列化的目标对象类型 (必须是引用类型)</typeparam>
    /// <param name="xmlString">要反序列化的 XML 字符串</param>
    /// <returns>反序列化后的对象</returns>
    /// <exception cref="ArgumentException">当 xmlString 为 null 或空时抛出</exception>
    /// <exception cref="InvalidOperationException">反序列化过程中发生错误时抛出</exception>
    T Deserialize<T>(string xmlString) where T : class;
}