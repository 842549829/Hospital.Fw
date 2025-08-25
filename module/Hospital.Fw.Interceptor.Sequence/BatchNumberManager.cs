using Hospital.Fw.Domain.Manager;
using Hospital.Fw.Domain.Shared.Constant;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Hospital.Fw.Interceptor.Sequence;

public class BatchNumberManager : BaseManager, IBatchNumberManager
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

        try
        {
            await db.Ado.BeginTranAsync();

            // 查询当前记录（带锁）
            var result = await db.Queryable<BatchNumber>()
                .TranLock(DbLockType.Wait).Where(it => it.Key == name)
                .FirstAsync(cancellationToken);

            long nextNumber;

            if (result != null)
            {
                // 存在当日记录，递增
                nextNumber = result.Number + 1;
                result.Number = nextNumber;
                await db.Updateable(result)
                    .UpdateColumns(it => new { it.Number })
                    .Where(it => it.Key == name)
                    .ExecuteCommandAsync(cancellationToken);
            }
            else
            {
                // 插入新记录
                nextNumber = 1;
                var entity = new BatchNumber
                {
                    Key = name,
                    Number = nextNumber
                };
                await db.Insertable(entity).ExecuteCommandAsync(cancellationToken);
            }

            await db.Ado.CommitTranAsync();

            return nextNumber;
        }
        catch (Exception ex)
        {
            await db.Ado.RollbackTranAsync();

            // 记录异常日志
            Console.WriteLine("获取批号失败：" + ex.Message);
            throw;
        }
    }
}