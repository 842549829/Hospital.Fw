namespace Hospital.Fw.Domain.Shared.Custom.Caching
{
    public interface IDistributedSessionCache : IDisposable
    {
        byte[]? Get(string sessionId);
    }
}