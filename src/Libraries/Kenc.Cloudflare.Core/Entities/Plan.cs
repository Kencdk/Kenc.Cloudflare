namespace Kenc.Cloudflare.Core.Entities
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class wrapping Plan objects in Cloudflare API
    /// </summary>
    public class Plan
    {
        [JsonProperty(propertyName: "id")]
        string Id { get; set; }

        [JsonProperty(propertyName: "name")]
        string Name { get; set; }

        [JsonProperty(propertyName: "price")]
        int Price { get; set; }

        [JsonProperty(propertyName: "currency")]
        string Currency { get; set; }

        [JsonProperty(propertyName: "frequency")]
        string Fequency { get; set; }

        [JsonProperty(propertyName: "legacy_id")]
        string LegacyId { get; set; }

        [JsonProperty(propertyName: "is_subscribed")]
        bool IsSubscribed { get; set; }

        [JsonProperty(propertyName: "can_subscribe")]
        bool CanSubscribe { get; set; }
    }
}
