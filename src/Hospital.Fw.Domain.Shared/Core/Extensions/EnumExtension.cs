using Hospital.Fw.Domain.Shared.Core;
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
    /// <param name="enumName">枚举名称</param>
    /// <returns>枚举映射</returns>
    public static List<EnumEntity> GetEnumListByName(string enumName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var enumInfo = assembly.CreateInstance(enumName, false);
        if (enumInfo == null)
        {
            return [];
        }
        var enumType = enumInfo.GetType();
        var enums = Enum.GetValues(enumType);
        var list = (from Enum item in enums select new EnumEntity
        {
            EnName = item.ToString(),
            Value = Convert.ToInt32(item), 
            Text = ToDescription(item)
        }).OrderBy(p => p.Value).ToList();
        return list.ToList();
    }

    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="enumValue">enumValue</param>
    /// <returns>取枚举描述</returns>
    public static string ToDescription(Enum enumValue)
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