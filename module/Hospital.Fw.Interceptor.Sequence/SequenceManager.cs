using Hospital.Fw.Domain.Manager;
using Hospital.Fw.Domain.Shared.Constant;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Hospital.Fw.Interceptor.Sequence;

/// <summary>
/// 序列管理
/// </summary>
public class SequenceManager : BaseManager, ISequenceManager
{
    /// <summary>
    /// 获取下一个序列
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>序列</returns>
    public async Task<long> GetNextSequenceAsync(string name, CancellationToken cancellationToken = default)
    {
        // 确保传入的序列名称不为空
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("序列名称不能为空", nameof(name));
        }

        var sqlSugarClient = ServiceProvider.GetRequiredService<ISqlSugarClient>();
        var db = sqlSugarClient.AsTenant().GetConnectionScope(SqlSugarCoreDbConst.Sequence);


        // 定义 SQL 查询
        var sqlQuery = $"SELECT NEXT VALUE FOR {name} AS NextSeqValue;";

        // 使用 FromSqlRaw 执行查询并获取结果
        var result = await  db.Ado.SqlQuerySingleAsync<long>(sqlQuery);

        return result;
    }
}