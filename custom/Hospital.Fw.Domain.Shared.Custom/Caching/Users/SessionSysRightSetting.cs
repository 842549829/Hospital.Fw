using System.Text.Json.Serialization;

namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users
{
    public class SessionSysRightSetting
    {
        [JsonPropertyName("$type")] 
        public string Type { get; set; } = null!;

        public string Id { get; set; } = null!;

        public string OwnerId { get; set; } = null!;

        public decimal RightsType { get; set; } 

        public string RightsId { get; set; } = null!;
    }
}