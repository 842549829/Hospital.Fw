using Hospital.Fw.Domain.Shared.Core.Extensions;
using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 全拼
/// </summary>
public abstract class FullPinyinEntity<TKey> : FullNameEntity<TKey>, IMayHavePinyin
{
    /// <summary>
    /// 拼音
    /// </summary>
    [SugarColumn(Length = 512, IsNullable = true, ColumnDescription = "拼音")]
    public string? Pinyin { get; set; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = true, ColumnDescription = "拼音首字母")]
    public string? PinyinFirstLetters { get; set; }

    /// <summary>
    /// 设置拼音
    /// </summary>
    public virtual void SetPinyin()
    {
        Pinyin = PinyinExtension.GetPinyin(Name);
        PinyinFirstLetters = PinyinExtension.GetFirstPinyin(Name);
    }
}