using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

/// <summary>
/// 实体
/// </summary>
/// <typeparam name="TKey">TKey</typeparam>
public abstract class EntityDto<TKey> : EntityDto, IEntityDto<TKey>
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    public TKey Id { get; set; } = default!;
}

/// <summary>
/// 实体
/// </summary>
public abstract class EntityDto : IEntityDto;

/// <summary>
/// 实体
/// </summary>
public interface IEntityDto;

/// <summary>
/// 实体
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntityDto<TKey>
{
    [Required]
    public TKey Id { get; set; }
}