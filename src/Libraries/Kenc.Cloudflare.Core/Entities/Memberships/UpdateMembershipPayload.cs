namespace Kenc.Cloudflare.Core.Entities.Memberships
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Payloads;

    public class UpdateMembershipPayload : ICloudflarePayload
    {
        [JsonPropertyName("status")]
        public MembershipStatus Status { get; set; }
    }
}
