namespace Kenc.Cloudflare.Core.Entities.AccountMembers
{
    using System.Text.Json.Serialization;

    public class AccountMember : CloudflareBaseEntity
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("user")]
        public User? User { get; set; }

        [JsonPropertyName("status")]
        public AccountMemberStatus Status { get; set; }

        [JsonPropertyName("roles")]
        public AccountMemberRole[]? Roles { get; set; }
    }
}
