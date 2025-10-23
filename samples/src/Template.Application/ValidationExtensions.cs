using FluentValidation;
using Hospital.Fw.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Application;

/// <summary>
/// 验证扩展
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// 添加 FluentValidation 验证
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddInventoryFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidation(typeof(ValidationExtensions));
        return services;
    }

    /// <summary>
    /// 验证必填名称字段：非空，且长度32。
    /// 适用于 Id 等必填字符串字段。
    /// </summary>
    /// <param name="rule">FluentValidation 规则构建器</param>
    /// <param name="length">字段长度，默认为 32</param>
    /// <param name="fieldName">字段名称（用于提示），默认为“该字段”</param>
    /// <typeparam name="T">验证的实体类型</typeparam>
    /// <returns>规则构建器选项</returns>
    public static IRuleBuilderOptions<T, string> Equals<T>(
        this IRuleBuilderInitial<T, string> rule,
        int length = 32,
        string fieldName = "该字段")
    {
        return rule
            .NotEmpty().WithMessage($"{fieldName}不能为空")
            .Must(x => x.Length == length).WithMessage($"{fieldName}长度必须为{length}个字符");
    }


    /// <summary>
    /// 验证必填名称字段：非空，且长度在 1 到指定最大值之间（默认 32）。
    /// 适用于 Name、Title、Code 等必填字符串字段。
    /// </summary>
    /// <param name="rule">FluentValidation 规则构建器</param>
    /// <param name="maxLength">最大长度，默认为 32</param>
    /// <param name="fieldName">字段名称（用于提示），默认为“该字段”</param>
    /// <typeparam name="T">验证的实体类型</typeparam>
    /// <returns>规则构建器选项</returns>
    public static IRuleBuilderOptions<T, string> Required<T>(
        this IRuleBuilderInitial<T, string> rule,
        int maxLength = 32,
        string fieldName = "该字段")
    {
        return rule
            .NotEmpty().WithMessage($"{fieldName}不能为空")
            .Length(1, maxLength).WithMessage($"{fieldName}长度应在1到{maxLength}个字符之间");
    }

    /// <summary>
    /// 为可空字符串字段添加验证：如果字段非 null 且非空白，则其长度不能超过指定最大值。
    /// 适用于 Remark、Name、Description、Note 等可选文本字段。
    /// </summary>
    /// <param name="rule">FluentValidation 的规则构建器</param>
    /// <param name="maxLength">允许的最大长度（字符数），默认 256</param>
    /// <param name="fieldName">字段名称（用于错误消息），默认为“该字段”</param>
    /// <typeparam name="T">验证的实体类型</typeparam>
    /// <returns>规则构建器选项</returns>
    /// <summary>
    /// 验证可空字符串字段：如果非空，则长度不能超过指定最大值。
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MaximumLengthIfNotEmpty<T>(
        this IRuleBuilderInitial<T, string?> rule,
        int maxLength = 256,
        string fieldName = "该字段")
    {
        return rule
            .Must(value => string.IsNullOrWhiteSpace(value) || value.Trim().Length <= maxLength)
            .WithMessage($"{fieldName}长度不能超过 {maxLength}个字符");
    }

    /// <summary>
    /// 验证可空数字字段：如果非空，则长度不能超过指定最大值。
    /// </summary>
    /// <param name="rule">FluentValidation 的规则构建器</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="fieldName">字段名称（用于错误消息），默认为“该字段”</param>
    /// <typeparam name="T">验证的实体类型</typeparam>
    /// <returns>规则构建器选项</returns>
    public static IRuleBuilderOptions<T, int?> MaximumLengthIfNotEmpty<T>(
        this IRuleBuilderInitial<T, int?> rule,
        int min = 1,
        int max = 32,
        string fieldName = "该字段")
    {
        return rule
            .Must(value => value == null || (value >= min && value <= max))
            .WithMessage($"{fieldName}必须在{min}到{max}之间");
    }

    /// <summary>
    /// 验证可空数字字段：如果非空，则长度不能超过指定最大值。
    /// </summary>
    /// <param name="rule">FluentValidation 的规则构建器</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="fieldName">字段名称（用于错误消息），默认为“该字段”</param>
    /// <typeparam name="T">验证的实体类型</typeparam>
    /// <returns>规则构建器选项</returns>
    public static IRuleBuilderOptions<T, decimal?> MaximumLengthIfNotEmpty<T>(
        this IRuleBuilderInitial<T, decimal?> rule,
        decimal min = 1M,
        decimal max = 32M,
        string fieldName = "该字段")
    {
        return rule
            .Must(value => value == null || (value >= min && value <= max))
            .WithMessage($"{fieldName}必须在{min}到{max}之间");
    }
}