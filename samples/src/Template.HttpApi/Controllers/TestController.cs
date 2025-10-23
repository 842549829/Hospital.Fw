using Hospital.Fw.Application.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Contract.Test;
using Template.Application.Contract.Test.Dto;

namespace Template.HttpApi.Controllers;

/// <summary>
/// test
/// </summary>
[ApiController]
[Route("/api/template/test")]
public class TestController: TemplateBaseController
{
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input">新增参数</param>
    /// <returns>新增结果</returns>
    [HttpPost("create")]
    public async Task<ResultDto<string>> CreateAsync([FromBody] TestDto input)
    {
        var testService = ServiceProvider.GetRequiredService<ITestAppService>();
        var id = await testService.CreateAsync(input);
        var result = new ResultDto<string>(id, true);
        return result;
    }
}