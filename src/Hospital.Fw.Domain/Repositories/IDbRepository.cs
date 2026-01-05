using Hospital.Fw.Domain.Shared.Core.Autofac;
using SqlSugar;

namespace Hospital.Fw.Domain.Repositories;

/// <summary>
/// 数据库仓储接口
/// </summary>
public interface IDbRepository : ITransientDependency
{
    /// <summary>
    /// 获取SqlSugarClient
    /// </summary>
    /// <param name="configId">配置Id</param>
    /// <returns>db</returns>
    ISqlSugarClient GetSqlSugarClient(string configId);


    /// <summary>
    /// CopySqlSugarClient
    /// </summary>
    /// <param name="configId">配置Id</param>
    /// <returns>db</returns>
    ISqlSugarClient CopySqlSugarClient(string configId);
}