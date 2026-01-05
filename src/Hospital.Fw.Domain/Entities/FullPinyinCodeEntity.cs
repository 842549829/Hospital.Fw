using SqlSugar;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 带拼音、拼音首字母、编码的实体
/// </summary>
/// <typeparam name="TKey">TKey</typeparam>
public abstract class FullPinyinCodeEntity<TKey> : FullPinyinEntity<TKey>, IHasCode
{
    /// <summary>
    /// Code
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = false, ColumnDescription = "Code")]
    public string Code { get; set; } = null!;
}