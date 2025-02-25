using System.Text.Json.Serialization;

namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users
{
    public class SessionUserInfoTemp
    {
        [JsonPropertyName("$type")] 
        public string Type { get; set; } = default!;

        [JsonPropertyName("$values")]
        public List<SessionSysRightSetting> RightSettings { get; set; } = default!;
    }
}