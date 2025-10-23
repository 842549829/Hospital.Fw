using Hospital.Fw.Domain.Shared.Constant;
using Hospital.Fw.Domain.Shared.Core.Exception;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Template.Application.Contract.Test;
using Template.Application.Contract.Test.Dto;

namespace Template.Application.Test;

public class TestAppService : TemplateBaseAppService, ITestAppService
{
    public async Task<string> CreateAsync(TestDto input)
    {
        var sqlSugarClient = ServiceProvider.GetRequiredService<ISqlSugarClient>();
        var db = sqlSugarClient.AsTenant().GetConnectionScope(SqlSugarCoreDbConst.Job);
        var mapper = ServiceProvider.GetRequiredService<IMapper>();

        var entity = mapper.Map<TestDto, Domain.Test.Entities.Test>(input);
        entity.Id = Guid.NewGuid().ToString("N");

        var result = await db.Insertable(entity).ExecuteCommandAsync();
        if (result != 1)
        {
            throw new CustomException("新增错误");
        }
        return entity.Id;
    }
}