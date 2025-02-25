namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users;

public class HttpContextCurrentUserAccessor(ISessionUserAccessor sessionUserAccessor) : CurrentUser
{
    protected override ISessionUser GetSessionUserInfo()
    {
        return sessionUserAccessor.SessionUserInfo;
    }
}