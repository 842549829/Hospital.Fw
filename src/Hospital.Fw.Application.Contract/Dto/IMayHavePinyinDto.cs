namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 可有拼音
/// </summary>
public interface IMayHavePinyinDto : IEntityDto
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