namespace Hospital.Fw.Application.Contract;

public class EntityDto<T> : EntityDto, IEntityDto<T>
{
    public T Id { get; set; } = default!;
}

public class EntityDto : IEntityDto;

public interface IEntityDto;

public interface IEntityDto<T>
{
    public T Id { get; set; }
}