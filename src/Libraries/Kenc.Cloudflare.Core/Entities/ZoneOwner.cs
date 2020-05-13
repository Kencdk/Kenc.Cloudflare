namespace Kenc.Cloudflare.Core.Entities
{
    using Newtonsoft.Json;

    public class ZoneOwner
    {
        [JsonProperty(propertyName: "id")]
        public string? Id { get; set; }

        [JsonProperty(propertyName: "name")]
        public string? Name { get; set; }

        [JsonProperty(propertyName: "type")]
        public string? Type { get; set; }
    }
}
