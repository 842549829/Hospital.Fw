using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;

namespace Hospital.Fw.Test.SqlSugarCore
{
    /// <summary>
    /// SqlSugarCore服务扩展
    /// </summary>
    public static class SqlSugarCoreExtensions
    {
        /// <summary>
        /// 添加SqlSugar服务
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISqlSugarClient>(s =>
            {
                var sqlSugar = new SqlSugarClient([
                        new ConnectionConfig
                        {
                            ConfigId = SqlSugarCoreDbConst.His,
                            DbType = DbType.Oracle,
                            ConnectionString = configuration.GetConnectionString(SqlSugarCoreDbConst.His),
                            IsAutoCloseConnection = true
                        },
                        new ConnectionConfig
                        {
                            ConfigId = SqlSugarCoreDbConst.Job,
                            DbType = DbType.Sqlite,
                            ConnectionString = configuration.GetConnectionString(SqlSugarCoreDbConst.Job),
                            IsAutoCloseConnection = true
                        }
                    ],
                    db =>
                    {
                        var log = s.GetService<ILogger<SqlSugarClient>>();
                        db.Aop.OnLogExecuting = (sql, pars) =>
                        {
                            var sqlStr = UtilMethods.GetNativeSql(sql, pars);
                            if (sqlStr != null)
                            {
                                log?.LogDebug(sqlStr);
                            }
                        };

                        db.Aop.OnLogExecuted = (sql, pars) =>
                        {
                            //执行时间超过5秒
                            if (db.Ado.SqlExecutionTime.TotalSeconds > 5)
                            {
                                var sqlStr = UtilMethods.GetNativeSql(sql, pars);
                                log?.LogWarning("执行时间超过5秒的sql：\n {sql}", sqlStr);
                            }
                        };
                    });
                return sqlSugar;
            });

            return services;
        }
    }
}