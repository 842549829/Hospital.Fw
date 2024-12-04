namespace Hospital.Fw.Domain.Shared.Core;

/// <summary>
/// This class can be used to provide an action when
/// Dispose method is called.
/// <typeparam name="T">The type of the parameter of the action.</typeparam>
/// </summary>
public class DisposeAction<T> : IDisposable
{
    private readonly Action<T> _action;

    private readonly T? _parameter;

    /// <summary>
    /// Creates a new object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    /// <param name="parameter">The parameter of the action.</param>
    public DisposeAction(Action<T> action, T parameter)
    {
        _action = action;
        _parameter = parameter;
    }

    public void Dispose()
    {
        if (_parameter != null)
        {
            _action(_parameter);
        }
    }
}