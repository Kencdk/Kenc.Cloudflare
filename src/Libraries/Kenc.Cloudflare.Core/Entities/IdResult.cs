namespace Kenc.Cloudflare.Core.Entities
{
    using System.Text.Json.Serialization;

    public class IdResult : ICloudflareEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
}
