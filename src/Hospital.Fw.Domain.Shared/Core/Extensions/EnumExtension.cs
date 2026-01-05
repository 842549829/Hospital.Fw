using System.ComponentModel;
using System.Reflection;

namespace Hospital.Fw.Domain.Shared.Core.Extensions;

/// <summary>
/// 枚举扩展
/// </summary>
public static class EnumExtension
{
    /// <summary>
    /// 根据枚举名称获取枚举列表
    /// </summary>
    /// <param name="enumName">枚举名称(全路径)</param>
    /// <returns>枚举映射</returns>
    public static List<EnumEntity> GetEnumListByName(string enumName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var enumInfo = assembly.CreateInstance(enumName, false);
        if (enumInfo == null) return new List<EnumEntity>();
        var enumType = enumInfo.GetType();
        var enums = Enum.GetValues(enumType);
        var list = new List<EnumEntity>();

        foreach (Enum item in enums)
        {
            list.Add(new EnumEntity
            {
                EnName = item.ToString(),
                Value = Convert.ToInt32(item),
                Text = ToDescription(item)
            });
        }
        list = list.OrderBy(p => p.Value).ToList();
        return list.ToList();
    }

    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="enumValue">枚举值</param>
    /// <returns>枚举描述</returns>
    public static string ToDescription(this Enum enumValue)
    {
        var value = enumValue.ToString();
        var field = enumValue.GetType().GetField(value);
        var objs = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
        if (objs is { Length: 0 }) //当描述属性没有时，直接返回名称
        {
            return value;
        }
        var descriptionAttribute = (DescriptionAttribute)objs?[0]!;
        return descriptionAttribute.Description;
    }
}