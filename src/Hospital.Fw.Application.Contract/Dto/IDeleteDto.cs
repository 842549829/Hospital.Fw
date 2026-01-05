namespace Hospital.Fw.Application.Contract.Dto;

public interface IDeleteDto
{
    /// <summary>
    /// 删除人Id
    /// </summary>
    public string? DeletionId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletionTime { get; set; }

    /// <summary>
    /// 删除人
    /// </summary>
    public string? DeletionName { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDeleted { get; set; }
}