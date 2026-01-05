using SqlSugar;

namespace Hospital.Fw.Domain.Repositories;

/// <summary>
/// 仓储类
/// </summary>
/// <typeparam name="T">T</typeparam>
public class Repository<T> : SimpleClient<T> where T : class, new()
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">db</param>
    public Repository(ISqlSugarClient db)
    {
        base.Context = db;
    }
}