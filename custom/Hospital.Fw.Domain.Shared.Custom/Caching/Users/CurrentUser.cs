using Hospital.Fw.Domain.Shared.Core;

namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users;

public abstract class CurrentUser : ICurrentUser
{
    private readonly AsyncLocal<ISessionUser> _currentSessionUserInfo = new();

    public ISessionUser SessionUserInfo => _currentSessionUserInfo.Value ??  GetSessionUserInfo();

    protected abstract ISessionUser GetSessionUserInfo();

    public IDisposable Change(ISessionUser sessionUserInfo)
    {
        return SetCurrent(sessionUserInfo);
    }

    private IDisposable SetCurrent(ISessionUser sessionUserInfo)
    {
        var parent = SessionUserInfo;
        _currentSessionUserInfo.Value = sessionUserInfo;

        return new DisposeAction<ValueTuple<AsyncLocal<ISessionUser>, ISessionUser>>(static (state) =>
        {
            var (currentPrincipal, parent) = state;
            currentPrincipal.Value = parent;
        }, (_currentSessionUserInfo, parent));
    }
}