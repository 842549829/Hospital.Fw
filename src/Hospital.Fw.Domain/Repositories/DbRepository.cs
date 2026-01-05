using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Hospital.Fw.Domain.Repositories;

/// <summary>
/// 数据库仓储
/// </summary>
public class DbRepository(IServiceProvider serviceProvider) : IDbRepository
{
    /// <summary>
    /// 获取SqlSugarClient
    /// </summary>
    /// <param name="configId">配置Id</param>
    /// <returns>db</returns>
    public ISqlSugarClient GetSqlSugarClient(string configId)
    {
        var sqlSugarClient = serviceProvider.GetRequiredService<ISqlSugarClient>();
        var db = sqlSugarClient.AsTenant().GetConnectionScope(configId);
        return db;
    }

    /// <summary>
    /// CopyNewHisHisResourcePlanningSqlSugarCoreDb
    /// </summary>
    /// <param name="configId">配置Id</param>
    /// <returns>db</returns>
    public ISqlSugarClient CopySqlSugarClient(string configId)
    {
        return GetSqlSugarClient(configId).CopyNew();
    }
}