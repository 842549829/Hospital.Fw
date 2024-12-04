namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 拼音
/// </summary>
public interface IMayHaveNamePinyin : IHasName
{
    /// <summary>
    /// 拼音
    /// </summary>
    string? Pinyin { get; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    string? PinyinFirstLetters { get; }
}