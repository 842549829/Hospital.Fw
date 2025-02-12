using Hospital.Fw.Domain.Shared.Core.System.Collections.Generic;

namespace Hospital.Fw.Domain.Shared.Core.DynamicProxy;

public static class DynamicProxyIgnoreTypes
{
    private static HashSet<Type> IgnoredTypes { get; } = new HashSet<Type>();

    public static void Add<T>()
    {
        Add(typeof(T));
    }

    public static void Add(Type type)
    {
        lock (IgnoredTypes)
        {
            IgnoredTypes.AddIfNotContains(type);
        }
    }

    public static void Add(params Type[] types)
    {
        lock (IgnoredTypes)
        {
            IgnoredTypes.AddIfNotContains(types);
        }
    }

    public static bool Contains(Type type, bool includeDerivedTypes = true)
    {
        lock (IgnoredTypes)
        {
            return includeDerivedTypes
                ? IgnoredTypes.Any(t => t.IsAssignableFrom(type))
                : IgnoredTypes.Contains(type);
        }
    }
}