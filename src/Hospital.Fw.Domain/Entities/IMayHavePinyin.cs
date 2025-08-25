namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 可有拼音
/// </summary>
public interface IMayHavePinyin
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