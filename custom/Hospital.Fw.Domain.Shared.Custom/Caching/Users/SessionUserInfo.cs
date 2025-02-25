using System.Text.Json.Serialization;

namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users
{
    /// <summary>
    /// 登录用户信息
    /// </summary>
    public class SessionUserInfo
    {
        [JsonPropertyName("$type")] 
        public string Type { get; set; } = default!;

        public string UserId { get; set; } = default!;

        public string LoginName { get; set; } = default!;

        public string UserNo { get; set; } = default!;

        public string UserWorkNo { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public bool IsDoctor => RoleCode == "01";

        public string RoleCode { get; set; } = default!;

        public string GradeCode { get; set; } = default!;

        public int Level { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsAdmin => IsSuperAdmin || RoleCode == "999";

        public string HosId { get; set; } = default!;

        public string HosName { get; set; } = default!;

        public string SecId { get; set; } = default!;

        public string SecName { get; set; } = default!;

        public string GroupId { get; set; } = default!;

        public byte? RightsLevel { get; set; }

        [JsonPropertyName("RightSettings")]
        public SessionUserInfoTemp SysRightSettingTemp { get; set; } = default!;

        public List<SessionSysRightSetting> RightSettingList => SysRightSettingTemp.RightSettings;

        public string ErrMsg { get; set; } = default!;

        public string Password { get; set; } = default!;

        public short LockTime { get; set; } = 30;
    }
}
