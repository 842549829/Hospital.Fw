using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 响应结果
/// </summary>
/// <typeparam name="T">T</typeparam>
public class ResultDto<T> : ResultDto
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="result">结果</param>
    /// <param name="isSuc">是否成功</param>
    /// <param name="msg">错误信息</param>
    public ResultDto(T? result, bool isSuc = true, string msg = "") : base(isSuc, msg)
    {
        Result = result;
        IsSuc = isSuc;
        Msg = msg;
    }

    /// <summary>
    /// 结果
    /// </summary>
    public T? Result { get; set; }
}

/// <summary>
/// 相应结果
/// </summary>
public class ResultDto : EntityDto
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="isSuc">是否成功</param>
    /// <param name="msg">错误信息 如果成功错误信息则可以为空</param>
    public ResultDto(bool isSuc, string msg = "")
    {
        IsSuc = isSuc;
        Msg = msg;
    }

    /// <summary>
    /// 是否成功
    /// </summary>
    [Required]
    public bool IsSuc { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Msg { get; set; }
}