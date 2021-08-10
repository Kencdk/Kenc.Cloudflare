namespace Kenc.Cloudflare.Core.Entities
{
    using Newtonsoft.Json;

    public class IdResult : ICloudflareEntity
    {
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; } = string.Empty;
    }
}
