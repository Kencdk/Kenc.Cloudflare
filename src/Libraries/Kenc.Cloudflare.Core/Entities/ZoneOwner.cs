namespace Kenc.Cloudflare.Core.Entities
{
    using System.Text.Json.Serialization;

    public class ZoneOwner
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
