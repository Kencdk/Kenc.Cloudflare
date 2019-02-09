namespace Kenc.Cloudflare.Core.Entities
{
    using Newtonsoft.Json;

    public class IdResult : ICloudflareEntity
    {
        [JsonProperty(propertyName: "id")]
        string Id { get; set; }
    }
}
