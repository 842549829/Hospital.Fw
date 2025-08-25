using Hospital.Fw.Domain.Manager;

namespace Hospital.Fw.Interceptor.Sequence;

/*
  创建序列
  CREATE SEQUENCE MySequence
   START WITH 1         -- 起始值为 1
   INCREMENT BY 1        -- 每次递增 1
   MINVALUE 1;           -- 设置最小值为 1
  查询序列
     SELECT 
       s.name AS SchemaName,
       seq.name AS SequenceName,
       CAST(seq.start_value AS BIGINT) AS StartValue,
       CAST(seq.increment AS BIGINT) AS Increment,
       CAST(seq.minimum_value AS BIGINT) AS MinValue,
       CAST(seq.maximum_value AS BIGINT) AS MaxValue,
       seq.is_cycling AS IsCycling,
       seq.current_value AS CurrentValue
      FROM 
        sys.sequences seq
       INNER JOIN 
        sys.schemas s ON seq.schema_id = s.schema_id
       ORDER BY 
        SchemaName, SequenceName;
  删除序列
   DROP SEQUENCE MySequence
 */

/// <summary>
/// 序列管理
/// </summary>
public interface ISequenceManager : IBaseManager
{
    /// <summary>
    /// 获取下一个序列
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>序列</returns>
    public Task<long> GetNextSequenceAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// 将数字转换为字符串，并补零到指定长度。
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="length">目标字符串的最小长度</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>补零后的字符串</returns>
    public async Task<string> PadNumberWithZerosAsync(string name, int length = 3, CancellationToken cancellationToken = default)
    {
        var number = await GetNextSequenceAsync(name, cancellationToken);
        return PadNumberWithZeros(number, length);
    }

    /// <summary>
    /// 将数字转换为字符串，并补零到指定长度。
    /// </summary>
    /// <param name="number">输入的数字</param>
    /// <param name="length">目标字符串的最小长度</param>
    /// <returns>补零后的字符串</returns>
    public string PadNumberWithZeros(long number, int length = 3)
    {
        if (length <= 0)
        {
            throw new ArgumentException("目标长度必须大于0", nameof(length));
        }

        return number.ToString($"D{length}");
    }
}