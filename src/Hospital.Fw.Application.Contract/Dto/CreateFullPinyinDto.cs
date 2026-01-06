namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带全拼的审计-创建
/// </summary>
public abstract class CreateFullPinyinDto : CreateFullNameDto, IMayHavePinyinDto
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