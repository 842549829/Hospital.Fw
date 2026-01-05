namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 启用排序审计
/// </summary>
public abstract class FullEnableSortAuditedDto : FullCodeDto, IHasEnabledDto, IHasSortDto
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; init; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; init; } = 0;
}