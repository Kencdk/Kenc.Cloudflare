namespace Kenc.Cloudflare.Core.Payloads
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Clients.Enums;

    public class UpdateMembershipPayload : ICloudflarePayload
    {
        [JsonPropertyName("status")]
        public MembershipStatus Status { get; set; }
    }
}
