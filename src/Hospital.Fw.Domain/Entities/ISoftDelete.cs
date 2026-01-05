using Hospital.Fw.Application.Contract.Dto;

namespace Hospital.Fw.Domain.Entities;

/// <summary>
/// 删除标记
/// </summary>
public interface ISoftDelete : IEntityDto
{
    /// <summary>
    /// 是否删除
    /// </summary>
    bool IsDeleted { get; }
}