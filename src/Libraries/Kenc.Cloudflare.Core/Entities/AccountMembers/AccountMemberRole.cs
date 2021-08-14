namespace Kenc.Cloudflare.Core.Entities.AccountMembers
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class AccountMemberRole : CloudflareBaseEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("permissions")]
        public Dictionary<string, Permission>? Permissions { get; set; }
    }
}
