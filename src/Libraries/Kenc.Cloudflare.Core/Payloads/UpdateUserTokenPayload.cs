namespace Kenc.Cloudflare.Core.Payloads
{
    using System;
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Entities;

    public class UpdateUserTokenPayload : ICloudflareEntity
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("status")]
        public UserTokenStatus? Status { get; set; }

        [JsonPropertyName("not_before")]
        public DateTimeOffset? NotBefore { get; set; }

        [JsonPropertyName("expires_on")]
        public DateTimeOffset? ExpiresOn { get; set; }

        [JsonPropertyName("policies")]
        public Policy[]? Policies { get; set; }

        [JsonPropertyName("condition")]
        public UserTokenCondition? Condition { get; set; }
    }
}
