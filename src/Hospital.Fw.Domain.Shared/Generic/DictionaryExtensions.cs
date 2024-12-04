namespace Hospital.Fw.Domain.Shared.Generic;

public static class DictionaryExtensions 
{
    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var obj) ? obj : default;
    }
}