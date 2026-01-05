namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 全拼
/// </summary>
public abstract class FullPinyinDto : FullNameDto, IMayHavePinyinDto
{
    /// <summary>
    /// 拼音
    /// </summary>
    public string? Pinyin { get; set; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    public string? PinyinFirstLetters { get; set; }
}