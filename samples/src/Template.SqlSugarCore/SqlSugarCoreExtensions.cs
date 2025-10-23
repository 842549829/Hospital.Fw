using Hospital.Fw.Domain.Entities;
using Hospital.Fw.Domain.Shared.Constant;
using Hospital.Fw.Domain.Shared.Custom.Caching.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using SqlSugar;
using System.Reflection;

namespace Template.SqlSugarCore;

/// <summary>
/// SqlSugar核心扩展
/// </summary>
public static class SqlSugarCoreExtensions
{
    public static IServiceCollection AddSqlSugar(this IServiceCollection services, IConfiguration configuration)
    {
        // 配置外置服务
        ConfigureExternalServices configureExternalServices = new ConfigureExternalServices
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
            SqlSugarClient sqlSugar = new SqlSugarClient([
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
                    // 添加全局过滤器
                    db.QueryFilter.AddTableFilter<ISoftDelete>(it => it.IsDeleted == false);

                    ILogger<SqlSugarClient>? log = s.GetService<ILogger<SqlSugarClient>>();

                    // 添加审计
                    db.Aop.DataExecuting = (oldValue, entityInfo) =>
                    {
                        // 新增审计
                        if (entityInfo.OperationType == DataFilterType.InsertByObject)
                        {
                            // 如果没有传主键则生成guid
                            if (entityInfo.EntityColumnInfo.IsPrimarykey
                                && entityInfo is { PropertyName: nameof(IEntity<string>.Id), EntityValue: IEntity<string> id } && string.IsNullOrEmpty(id.Id))
                            {
                                entityInfo.SetValue(Guid.NewGuid().ToString("N"));
                            }

                            if (entityInfo.PropertyName == nameof(IHasCreatorTime.CreateTime))
                            {
                                if (oldValue is DateTime dateTime && dateTime == DateTime.MinValue)
                                {
                                    entityInfo.SetValue(DateTime.Now);
                                }
                            }
                            ICurrentUser currentUser = s.GetRequiredService<ICurrentUser>();
                            if (currentUser.SessionUserInfo.IsAuthenticated)
                            {
                                if (entityInfo.PropertyName == nameof(IMayHaveCreatorName.CreatorName) && !string.IsNullOrEmpty(currentUser.SessionUserInfo.UserName))
                                {
                                    if (oldValue is string userName && string.IsNullOrEmpty(userName))
                                    {
                                        entityInfo.SetValue(currentUser.SessionUserInfo.UserName);
                                    }
                                }
                                if (entityInfo.PropertyName == nameof(IMayHaveCreatorId.CreatorId) && !string.IsNullOrEmpty(currentUser.SessionUserInfo.UserId))
                                {
                                    if (oldValue is string userId && string.IsNullOrEmpty(userId))
                                    {
                                        entityInfo.SetValue(currentUser.SessionUserInfo.UserId);
                                    }
                                }
                            }
                        }
                        else if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                        {
                            // 删除审计
                            if (entityInfo.EntityValue is ISoftDelete { IsDeleted: true })
                            {
                                if (entityInfo.PropertyName == nameof(IMayHaveDeletion.DeletionTime))
                                {
                                    if (oldValue is DateTime dateTime && dateTime == DateTime.MinValue)
                                    {
                                        entityInfo.SetValue(DateTime.Now);
                                    }
                                }
                                ICurrentUser currentUser = s.GetRequiredService<ICurrentUser>();
                                if (currentUser.SessionUserInfo.IsAuthenticated)
                                {
                                    if (entityInfo.PropertyName == nameof(IMayHaveDeletion.DeletionName) && !string.IsNullOrEmpty(currentUser.SessionUserInfo.UserName))
                                    {
                                        if (oldValue is string userName && string.IsNullOrEmpty(userName))
                                        {
                                            entityInfo.SetValue(currentUser.SessionUserInfo.UserName);
                                        }
                                    }
                                    if (entityInfo.PropertyName == nameof(IMayHaveDeletion.DeletionId) && !string.IsNullOrEmpty(currentUser.SessionUserInfo.UserId))
                                    {
                                        if (oldValue is string userId && string.IsNullOrEmpty(userId))
                                        {
                                            entityInfo.SetValue(currentUser.SessionUserInfo.UserId);
                                        }
                                    }
                                }
                            }
                            //更新审计
                            else
                            {
                                if (entityInfo.PropertyName == nameof(IMayHaveLastModificationTime.LastModificationTime))
                                {
                                    if (oldValue is DateTime dateTime && dateTime == DateTime.MinValue)
                                    {
                                        entityInfo.SetValue(DateTime.Now);
                                    }
                                }
                                ICurrentUser currentUser = s.GetRequiredService<ICurrentUser>();
                                if (currentUser.SessionUserInfo.IsAuthenticated)
                                {
                                    if (entityInfo.PropertyName == nameof(IMayHaveLastModificationName.LastModificationName) && !string.IsNullOrEmpty(currentUser.SessionUserInfo.UserName))
                                    {
                                        if (oldValue is string userName && string.IsNullOrEmpty(userName))
                                        {
                                            entityInfo.SetValue(currentUser.SessionUserInfo.UserName);
                                        }
                                    }
                                    if (entityInfo.PropertyName == nameof(IMayHaveLastModificationId.LastModificationId) && !string.IsNullOrEmpty(currentUser.SessionUserInfo.UserId))
                                    {
                                        if (oldValue is string userId && string.IsNullOrEmpty(userId))
                                        {
                                            entityInfo.SetValue(currentUser.SessionUserInfo.UserId);
                                        }
                                    }
                                }
                            }
                        }
                    };

                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        string sqlStr = UtilMethods.GetNativeSql(sql, pars);
                        if (sqlStr != null)
                        {
                            using (LogContext.PushProperty("EventId_Name", nameof(SqlSugarCoreExtensions)))
                            {
                                log?.LogDebug(sqlStr);
                            }
                        }
                    };

                    db.Aop.OnLogExecuted = (sql, _) =>
                    {
                        //执行时间超过5秒
                        if (db.Ado.SqlExecutionTime.TotalSeconds > 5)
                        {
                            //代码CS文件名
                            string fileName = db.Ado.SqlStackTrace.FirstFileName;
                            //代码行数
                            int fileLine = db.Ado.SqlStackTrace.FirstLine;
                            //方法名
                            string firstMethodName = db.Ado.SqlStackTrace.FirstMethodName;
                            using (LogContext.PushProperty("EventId_Name", nameof(SqlSugarCoreExtensions)))
                            {
                                log?.LogWarning("执行时间超过5秒的sql：{fileName}:{fileLine} {firstMethodName}\n {sql}", fileName, fileLine, firstMethodName, sql);
                            }
                        }
                    };
                });
            return sqlSugar;
        });

        //services.AddScoped(typeof(Repository<>));
        return services;
    }
}