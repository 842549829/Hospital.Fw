using System.Text;
using System.Text.Json;
using Hospital.Fw.Domain.Shared.Custom.Caching;
using Hospital.Fw.Domain.Shared.Custom.Caching.Users;

namespace Hospital.Fw.HttpApi.Custom.Security;

/// <summary>
/// SessionUserAccessor
/// </summary>
public class SessionUserAccessor : ISessionUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDistributedSessionCache _distributedSessionCache;
    private readonly ILogger<HttpContextCurrentUserAccessor> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="httpContextAccessor">httpContextAccessor</param>
    /// <param name="distributedSessionCache">distributedSessionCache</param>
    /// <param name="logger">logger</param>
    public SessionUserAccessor(IHttpContextAccessor httpContextAccessor,
        IDistributedSessionCache distributedSessionCache,
        ILogger<HttpContextCurrentUserAccessor> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _distributedSessionCache = distributedSessionCache;
        _logger = logger;
        SessionUserInfo = GetSessionUserInfo();
    }

    /// <summary>
    /// ISessionUser
    /// </summary>
    public ISessionUser SessionUserInfo { get; }

    private ISessionUser GetSessionUserInfo()
    {
        try
        {
            var sessionId = _httpContextAccessor.HttpContext?.Request.Cookies["ASP.NET_SessionId"];
            if (sessionId == null)
            {
                _logger.LogInformation("HttpContextCurrentUserAccessor.GetSessionUserInfo sessionId is null");
                return new SessionUser(false);
            }
            var bytes = _distributedSessionCache.Get(sessionId);
            if (bytes == null)
            {
                _logger.LogInformation("HttpContextCurrentUserAccessor.GetSessionUserInfo bytes is null");
                return new SessionUser(false);
            }
            var jsonString = Encoding.UTF8.GetString(bytes);
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                _logger.LogInformation("HttpContextCurrentUserAccessor.GetSessionUserInfo jsonString is null");
                return new SessionUser(false);
            }
            var sessionUserInfo = JsonSerializer.Deserialize<SessionUserInfo>(jsonString);
            if (sessionUserInfo == null)
            {
                _logger.LogInformation("HttpContextCurrentUserAccessor.GetSessionUserInfo sessionUserInfo is null");
                return new SessionUser(false);
            }

            return new SessionUser(true,
                sessionUserInfo.UserId,
                sessionUserInfo.LoginName,
                sessionUserInfo.UserNo,
                sessionUserInfo.UserWorkNo,
                sessionUserInfo.UserName,
                sessionUserInfo.IsDoctor,
                sessionUserInfo.RoleCode,
                sessionUserInfo.GradeCode,
                sessionUserInfo.Level,
                sessionUserInfo.IsSuperAdmin,
                sessionUserInfo.IsAdmin,
                sessionUserInfo.HosId,
                sessionUserInfo.HosName,
                sessionUserInfo.SecId,
                sessionUserInfo.SecName,
                sessionUserInfo.GroupId,
                sessionUserInfo.RightsLevel,
                sessionUserInfo.RightSettingList,
                sessionUserInfo.ErrMsg,
                sessionUserInfo.Password,
                sessionUserInfo.LockTime
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HttpContextCurrentUserAccessor.GetSessionUserInfo");
            return new SessionUser(false);
        }
    }
}