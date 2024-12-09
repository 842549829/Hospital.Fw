using Hospital.Fw.Domain.Shared.Core.Autofac;

namespace Hospital.Fw.Domain.Manager;

public abstract class BaseManager : IBaseManager
{
    [Autowired]
    public required IServiceProvider ServiceProvider { get; set; }
}