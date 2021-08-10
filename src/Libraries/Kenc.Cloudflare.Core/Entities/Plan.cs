namespace Kenc.Cloudflare.Core.Entities
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class wrapping Plan objects in Cloudflare API
    /// </summary>
    public class Plan
    {
        [JsonProperty(propertyName: "id")]
        public string? Id { get; set; }

        [JsonProperty(propertyName: "name")]
        public string? Name { get; set; }

        [JsonProperty(propertyName: "price")]
        public int Price { get; set; }

        [JsonProperty(propertyName: "currency")]
        public string? Currency { get; set; }

        [JsonProperty(propertyName: "frequency")]
        public string? Fequency { get; set; }

        [JsonProperty(propertyName: "legacy_id")]
        public string? LegacyId { get; set; }

        [JsonProperty(propertyName: "is_subscribed")]
        public bool IsSubscribed { get; set; }

        [JsonProperty(propertyName: "can_subscribe")]
        public bool CanSubscribe { get; set; }
    }
}
