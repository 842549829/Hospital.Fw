using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Domain.Shared.Core.Autofac;
using Microsoft.Extensions.Logging;
using Template.Application.Contract.Jobs.Eto;

namespace Template.Application.Jobs.Tasks;

public class UserInitJobs( ILogger<UserInitJobs> logger) : IAsyncBackgroundJob<UserInitEto>, ITransientDependency
{
    public Task ExecuteAsync(UserInitEto args)
    {
        logger.LogInformation("UserInitJobs is running...args:{args}", args);
        return Task.CompletedTask;
    }
}

