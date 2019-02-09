namespace Kenc.Cloudflare.Core.Entities
{
    using Newtonsoft.Json;

    public class ZoneOwner
    {
        [JsonProperty(propertyName: "id")]
        string Id { get; set; }

        [JsonProperty(propertyName: "name")]
        string Name { get; set; }

        [JsonProperty(propertyName: "type")]
        string Type { get; set; }
    }
}
