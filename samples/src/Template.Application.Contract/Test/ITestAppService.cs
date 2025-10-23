using Template.Application.Contract.Test.Dto;

namespace Template.Application.Contract.Test;

public interface ITestAppService: ITemplateBaseAppService
{
    Task<string> CreateAsync(TestDto input);
}