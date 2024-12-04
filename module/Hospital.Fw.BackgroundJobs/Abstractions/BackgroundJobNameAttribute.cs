namespace Hospital.Fw.BackgroundJobs.Abstractions;

public class BackgroundJobNameAttribute(string name) : Attribute, IBackgroundJobNameProvider
{
    public string Name { get; } = name;

    public static string GetName<TJobArgs>()
    {
        return GetName(typeof(TJobArgs));
    }

    public static string GetName(Type jobArgsType)
    {
        return (jobArgsType
                    .GetCustomAttributes(true)
                    .OfType<IBackgroundJobNameProvider>()
                    .FirstOrDefault()
                    ?.Name
                ?? jobArgsType.FullName)!;
    }
}