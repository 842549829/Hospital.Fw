using Hospital.Fw.Application.Contract;
using Hospital.Fw.Domain.Shared.Core.Autofac;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Hospital.Fw.Application;

/// <summary>
/// 基础服务
/// </summary>
public abstract class BaseAppService : IBaseAppService
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    [Autowired]
    public required IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// 获取SqlSugarClient
    /// </summary>
    /// <param name="configId">配置Id</param>
    /// <returns>db</returns>
    public virtual ISqlSugarClient GetSqlSugarClient(string configId)
    {
        var sqlSugarClient = ServiceProvider.GetRequiredService<ISqlSugarClient>();
        var db = sqlSugarClient.AsTenant().GetConnectionScope(configId);
        return db;
    }
}