using System.Text.Json.Serialization;

namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users
{
    /// <summary>
    /// 登录用户信息
    /// </summary>
    public class SessionUserInfo
    {
        [JsonPropertyName("$type")] 
        public string Type { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string LoginName { get; set; } = null!;

        public string UserNo { get; set; } = null!;

        public string UserWorkNo { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public bool IsDoctor => RoleCode == "01";

        public string RoleCode { get; set; } = null!;

        public string GradeCode { get; set; } = null!;

        public int Level { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsAdmin => IsSuperAdmin || RoleCode == "999";

        public string HosId { get; set; } = null!;

        public string HosName { get; set; } = null!;

        public string SecId { get; set; } = null!;

        public string SecName { get; set; } = null!;

        public string GroupId { get; set; } = null!;

        public byte? RightsLevel { get; set; }

        [JsonPropertyName("RightSettings")]
        public SessionUserInfoTemp SysRightSettingTemp { get; set; } = null!;

        public List<SessionSysRightSetting> RightSettingList => SysRightSettingTemp.RightSettings;

        public string ErrMsg { get; set; } = null!;

        public string Password { get; set; } = null!;

        public short LockTime { get; set; } = 30;
    }
}
