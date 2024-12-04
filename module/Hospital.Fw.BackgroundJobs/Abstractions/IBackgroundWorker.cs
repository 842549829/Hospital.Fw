using Hospital.Fw.BackgroundJobs.Threading;
using Hospital.Fw.Domain.Shared.Core.Autofac;

namespace Hospital.Fw.BackgroundJobs.Abstractions;

public interface IBackgroundWorker : IRunnable, ISingletonDependency;