namespace Hospital.Fw.Application.Contract;

public class EntityDto<TKey> : EntityDto, IEntityDto<TKey>
{
    public TKey Id { get; set; } = default!;
}

public class EntityDto : IEntityDto;

public interface IEntityDto;

public interface IEntityDto<TKey>
{
    public TKey Id { get; set; }
}