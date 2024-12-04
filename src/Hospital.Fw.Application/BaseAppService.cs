using Hospital.Fw.Application.Contract;
using Hospital.Fw.Domain.Shared.Core.Autofac;

namespace Hospital.Fw.Application;

public abstract class BaseAppService : IBaseAppService
{
    [Autowired]
    public IServiceProvider ServiceProvider { get; set; } = default!;
}