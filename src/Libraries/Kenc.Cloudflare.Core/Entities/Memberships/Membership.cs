namespace Kenc.Cloudflare.Core.Entities.Memberships
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities.Accounts;

    public class Membership : CloudflareBaseEntity
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("status")]
        public MembershipStatus Status { get; set; }

        [JsonPropertyName("account")]
        public Account? Account { get; set; }

        [JsonPropertyName("roles")]
        public string[]? Roles { get; set; }

        [JsonPropertyName("permissions")]
        public Dictionary<string, Permission>? Permissions { get; set; }
    }
}
