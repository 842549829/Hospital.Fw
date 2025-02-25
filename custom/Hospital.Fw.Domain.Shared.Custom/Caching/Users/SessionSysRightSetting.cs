using System.Text.Json.Serialization;

namespace Hospital.Fw.Domain.Shared.Custom.Caching.Users
{
    public class SessionSysRightSetting
    {
        [JsonPropertyName("$type")] 
        public string Type { get; set; } = default!;

        public string Id { get; set; } = default!;

        public string OwnerId { get; set; } = default!;

        public decimal RightsType { get; set; } = default!;

        public string RightsId { get; set; } = default!;
    }
}