using Mapster;
using Template.Application.Contract.Test.Dto;

namespace Template.Application.Test;

public class TestMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<TestDto, Domain.Test.Entities.Test>();
    }
}