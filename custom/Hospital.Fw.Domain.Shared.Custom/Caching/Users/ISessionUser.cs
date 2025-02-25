namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users;

public interface ISessionUser
{
    public bool IsAuthenticated { get; }

    public string UserId { get; }

    public string LoginName { get; }

    public string UserNo { get; }

    public string UserWorkNo { get; }

    public string UserName { get; }

    public bool IsDoctor { get; }

    public string RoleCode { get; }

    public string GradeCode { get; }

    public int Level { get; }

    public bool IsSuperAdmin { get; }

    public bool IsAdmin { get; }

    public string HosId { get; }

    public string HosName { get; }

    public string SecId { get; }

    public string SecName { get; }

    public string GroupId { get; }

    public byte? RightsLevel { get; }

    public List<SessionSysRightSetting> RightSettingList { get; }

    public string ErrMsg { get; }

    public string Password { get; }

    public short LockTime { get; }
}