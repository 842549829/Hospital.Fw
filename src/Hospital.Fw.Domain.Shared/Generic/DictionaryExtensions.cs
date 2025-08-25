namespace Hospital.Fw.Domain.Shared.Generic;

/// <summary>
/// 集合扩展
/// </summary>
public static class DictionaryExtensions 
{
    /// <summary>
    /// 获取字典中的值
    /// </summary>
    /// <typeparam name="TKey">TKey</typeparam>
    /// <typeparam name="TValue">TValue</typeparam>
    /// <param name="dictionary">dictionary</param>
    /// <param name="key">key</param>
    /// <returns>字典中的值</returns>
    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var obj) ? obj : default;
    }
}