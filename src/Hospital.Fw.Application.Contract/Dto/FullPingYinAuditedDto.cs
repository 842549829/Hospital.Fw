namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 拼音
/// </summary>
public abstract class FullPingYinAuditedDto : FullAuditedDto, IMayHavePinyinDto
{
    /// <summary>
    /// 拼音
    /// </summary>
    public string? Pinyin { get; init; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    public string? PinyinFirstLetters { get; init; }
}