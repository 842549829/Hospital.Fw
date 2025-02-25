namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users;
public class SessionUser(
    bool isAuthenticated,
    string userId,
    string loginName,
    string userNo,
    string userWorkNo,
    string userName,
    bool isDoctor,
    string roleCode,
    string gradeCode,
    int level,
    bool isSuperAdmin,
    bool isAdmin,
    string hosId,
    string hosName,
    string secId,
    string secName,
    string groupId,
    byte? rightsLevel,
    List<SessionSysRightSetting> rightSettingList,
    string errMsg,
    string password,
    short lockTime)
    : ISessionUser
{
    public SessionUser(bool isAuthenticated) :
        this(isAuthenticated, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, string.Empty, string.Empty, 0, false, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, new List<SessionSysRightSetting>(), string.Empty, string.Empty, 0)
    {
    }

    public bool IsAuthenticated { get; } = isAuthenticated;
    public string UserId { get; set; } = userId;
    public string LoginName { get; set; } = loginName;
    public string UserNo { get; set; } = userNo;
    public string UserWorkNo { get; set; } = userWorkNo;
    public string UserName { get; set; } = userName;
    public bool IsDoctor { get; } = isDoctor;
    public string RoleCode { get; set; } = roleCode;
    public string GradeCode { get; set; } = gradeCode;
    public int Level { get; set; } = level;
    public bool IsSuperAdmin { get; set; } = isSuperAdmin;
    public bool IsAdmin { get; } = isAdmin;
    public string HosId { get; set; } = hosId;
    public string HosName { get; set; } = hosName;
    public string SecId { get; set; } = secId;
    public string SecName { get; set; } = secName;
    public string GroupId { get; set; } = groupId;
    public byte? RightsLevel { get; set; } = rightsLevel;
    public List<SessionSysRightSetting> RightSettingList { get; } = rightSettingList;
    public string ErrMsg { get; set; } = errMsg;
    public string Password { get; set; } = password;
    public short LockTime { get; set; } = lockTime;
}