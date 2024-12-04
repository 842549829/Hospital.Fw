using Hospital.Fw.BackgroundJobs.Abstractions;
using Hospital.Fw.Domain.Shared.Exceptions;
using Hospital.Fw.Domain.Shared.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Fw.BackgroundJobs.Implementations;

public static class BackgroundWorkersApplicationInitializationContextExtensions
{
    public static async Task<IServiceProvider> AddBackgroundWorkerAsync<TWorker>(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        where TWorker : IBackgroundWorker
    {

        await serviceProvider.AddBackgroundWorkerAsync(typeof(TWorker), cancellationToken: cancellationToken);

        return serviceProvider;
    }

    public static async Task<IServiceProvider> AddBackgroundWorkerAsync(this IServiceProvider serviceProvider, Type workerType, CancellationToken cancellationToken = default)
    {

        if (!workerType.IsAssignableTo<IBackgroundWorker>())
        {
            throw new TaskException($"Given type ({workerType.AssemblyQualifiedName}) must implement the {typeof(IBackgroundWorker).AssemblyQualifiedName} interface, but it doesn't!");
        }

        var backgroundWorker = (IBackgroundWorker)serviceProvider.GetRequiredService(workerType);

        await serviceProvider
            .GetRequiredService<IBackgroundWorkerManager>()
            .AddAsync(backgroundWorker, cancellationToken);

        return serviceProvider;
    }
}
