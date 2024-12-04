namespace Hospital.Fw.Domain.Shared.Core.Exception;

public class CustomException(string message, int number = 0) : System.Exception(message)
{
    public int Number { get; private set; } = number;
}