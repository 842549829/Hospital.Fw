using FluentValidation;
using Hospital.Fw.Domain.Shared.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace Hospital.Fw.Application;

/// <summary>
/// 添加自定义扩展
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// 添加FluentValidation
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="type">编写验证器需要扫描的程序集所在类型</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddFluentValidation(this IServiceCollection services, Type type)
    {
        services.AddFluentValidationAutoValidation(configuration =>
        {
            // Disable the built-in .NET model (data annotations) validation.
            configuration.DisableBuiltInModelValidation = true;

            // Only validate controllers decorated with the `FluentValidationAutoValidation` attribute.
            //configuration.ValidationStrategy = ValidationStrategy.Annotation;

            // Enable validation for parameters bound from `BindingSource.Body` binding sources.
            configuration.EnableBodyBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from `BindingSource.Form` binding sources.
            configuration.EnableFormBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from `BindingSource.Query` binding sources.
            configuration.EnableQueryBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from `BindingSource.Path` binding sources.
            configuration.EnablePathBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from 'BindingSource.Custom' binding sources.
            configuration.EnableCustomBindingSourceAutomaticValidation = true;

            // Replace the default result factory with a custom implementation.
            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });
        services.AddValidatorsFromAssembly(type.Assembly);
        return services;
    }
}

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault() ?? "验证失败";
        return new OkObjectResult(new ErrorResult(errors));
    }
}