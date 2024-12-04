namespace Hospital.Fw.Domain.Shared.Core;

/// <summary>
/// 错误返回结果
/// </summary>
public class ErrorResult
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="msg">错误信息</param>
    public ErrorResult(string msg)
    {
        Msg = msg;
    }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuc { get; set; }
    
    /// <summary>
    /// 错误信息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 行数
    /// </summary>
    public int Rows { get; set; }
}