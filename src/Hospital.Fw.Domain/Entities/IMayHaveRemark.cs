namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可有备注
/// </summary>
public interface IMayHaveRemark
{
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}