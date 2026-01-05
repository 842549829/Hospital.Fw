using System.ComponentModel.DataAnnotations;

namespace Hospital.Fw.Application.Contract.Dto;

public class EntityDto<TKey> : EntityDto, IEntityDto<TKey>
{
    [Required]
    public TKey Id { get; init; } = default!;
}

public class EntityDto : IEntityDto;

public interface IEntityDto;

public interface IEntityDto<TKey>
{
    [Required]
    public TKey Id { get; init; }
}