namespace Hospital.Fw.Domain.Shared.Exceptions;

public interface IExceptionNotifier
{
    Task NotifyAsync(ExceptionNotificationContext context);
}