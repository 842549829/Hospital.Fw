namespace Hospital.Fw.Domain.Shared.Core;

public interface IServiceProviderAccessor
{
    IServiceProvider ServiceProvider { get; }
}