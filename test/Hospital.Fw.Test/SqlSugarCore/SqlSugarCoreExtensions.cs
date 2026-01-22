using Hospital.Fw.Domain.Shared.Constant;
using SqlSugar;
using System.Reflection;

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
            // 配置外置服务
            var configureExternalServices = new ConfigureExternalServices
            {
                EntityService = (c, p) =>
                {
                    // 处理列表字段
                    p.DbColumnName = UtilMethods.ToUnderLine(p.DbColumnName);//驼峰转下划线方法

                    #region 为空的字段配置

                    /***低版本C#写法***/
                    // int?  decimal?这种 isnullable=true 不支持string(下面.NET 7支持)
                    if (p.IsPrimarykey == false && c.PropertyType.IsGenericType &&
                        c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        p.IsNullable = true;
                    }

                    /***高版C#写法***/
                    //支持string?和string  
                    if (p.IsPrimarykey == false && new NullabilityInfoContext()
                            .Create(c).WriteState is NullabilityState.Nullable)
                    {
                        p.IsNullable = true;
                    }
                    #endregion
                },
                EntityNameService = (_, p) =>
                {
                    //处理表名
                    p.DbTableName = UtilMethods.ToUnderLine(p.DbTableName);//驼峰转下划线方法
                }
            };

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
                        },
                        new ConnectionConfig
                        {
                            ConfigId = SqlSugarCoreDbConst.Sequence,
                            DbType = DbType.MySql,
                            ConnectionString = configuration.GetConnectionString(SqlSugarCoreDbConst.Sequence),
                            IsAutoCloseConnection = true,
                            ConfigureExternalServices = configureExternalServices,
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