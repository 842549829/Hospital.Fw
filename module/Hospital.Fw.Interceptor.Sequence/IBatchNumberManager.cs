namespace Hospital.Fw.Interceptor.Sequence;

/// <summary>
/// 批次  
/// </summary>
public interface IBatchNumberManager : ISequenceManager 
{
    /// <summary>
    /// 获取下一个批次号
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="number">数量</param>
    /// <param name="cancellationToken">取消Token</param>
    /// <returns>Task</returns>
    public Task<List<long>> GetNextSequenceAsync(string name, int number,CancellationToken cancellationToken = default);

    /// <summary>
    /// 将数字转换为字符串，并补零到指定长度。
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="length">目标字符串的最小长度</param>
    /// <param name="number">数量</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>补零后的字符串</returns>
    public async Task<List<string>> PadNumberWithZerosAsync(string name, int number, int length, CancellationToken cancellationToken = default)
    {
        // 1. 获取数字序列
        var numbers = await GetNextSequenceAsync(name, number, cancellationToken);

        // 2. 预分配列表容量，提高性能
        var result = new List<string>(numbers.Count);

        // 3. 处理每个数字并添加到列表
        foreach (var item in numbers)
        {
            result.Add(PadNumberWithZeros(item, length)); 
        }

        // 4. 返回列表
        return result;
    }

}