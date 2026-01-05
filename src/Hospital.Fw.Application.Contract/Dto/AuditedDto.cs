namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 审计字段
/// </summary>
public class AuditedDto : DeleteDto
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    public string? Pinyin { get; set; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    public string? PinyinFirstLetters { get; set; }
}