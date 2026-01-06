namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 带Code的审计实体
/// </summary>
public abstract class CreateFullPinyinCodeDto : CreateFullPinyinDto, IHasCodeDto
{
    /// <summary>
    /// Code
    /// </summary>
    public string Code { get; set; } = null!;
}