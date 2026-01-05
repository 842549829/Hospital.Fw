namespace Hospital.Fw.Application.Contract.Dto;

public class EntityDto<TKey> : EntityDto, IEntityDto<TKey>
{
    public TKey Id { get; init; } = default!;
}

public class EntityDto : IEntityDto;

public interface IEntityDto;

public interface IEntityDto<TKey>
{
    public TKey Id { get; init; }
}