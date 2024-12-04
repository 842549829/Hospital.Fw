using System.Reflection;
using Autofac.Core;
using Hospital.Fw.Domain.Shared.Core.Autofac;

namespace Hospital.Fw.HttpApi.Autofac;

/// <summary>
/// 属性注入选择器
/// </summary>
public class AutowiredPropertySelector : IPropertySelector
{
    /// <summary>
    /// Provides filtering to determine if property should be injected.
    /// </summary>
    /// <param name="propertyInfo">Property to be injected.</param>
    /// <param name="instance">Instance that has the property to be injected.</param>
    /// <returns>Whether property should be injected.</returns>
    public bool InjectProperty(PropertyInfo propertyInfo, object instance)
    {
        // 带有 AutowiredAttribute 特性的属性会进行属性注入
        return propertyInfo.CustomAttributes.Any(it => it.AttributeType == typeof(AutowiredAttribute));
    }
}