namespace Hospital.Fw.Domain.Shared.Exceptions;

public class BackgroundJobExecutionException : TaskException
{
    public string JobType { get; set; } = default!;

    public object JobArgs { get; set; } = default!;

    public BackgroundJobExecutionException()
    {

    }

    /// <summary>
    /// Creates a new <see cref="BackgroundJobExecutionException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public BackgroundJobExecutionException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
