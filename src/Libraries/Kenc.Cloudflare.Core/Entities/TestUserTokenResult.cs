namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using System.Text.Json.Serialization;

    public class TestUserTokenResult : ICloudflareEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public UserTokenStatus Status { get; set; }

        [JsonPropertyName("not_before")]
        public DateTimeOffset NotBefore { get; set; }

        [JsonPropertyName("expires_on")]
        public DateTimeOffset ExpiresOn { get; set; }
    }
}
