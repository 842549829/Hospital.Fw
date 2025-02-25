namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users
{
    public interface ISessionUserAccessor
    {
        ISessionUser SessionUserInfo { get; }
    }
}
