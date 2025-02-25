namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users;

public interface ICurrentUser
{
    ISessionUser SessionUserInfo { get; }

    IDisposable Change(ISessionUser sessionUserInfo);
}