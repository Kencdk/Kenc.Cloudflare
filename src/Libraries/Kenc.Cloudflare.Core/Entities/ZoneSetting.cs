namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using System.Text.Json.Serialization;

    public class ZoneSetting : ICloudflareEntity
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("editable")]
        public bool Editable { get; set; }

        [JsonPropertyName("modified_on")]
        public DateTime ModifiedOn { get; set; }
    }
}
