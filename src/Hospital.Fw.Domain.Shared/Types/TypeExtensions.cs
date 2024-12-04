using System.Diagnostics.CodeAnalysis;

namespace Hospital.Fw.Domain.Shared.Types;

public static class TypeExtensions
{
    public static bool IsAssignableTo<TTarget>([NotNull] this Type type)
    {
        return type.IsAssignableTo(typeof(TTarget));
    }
}