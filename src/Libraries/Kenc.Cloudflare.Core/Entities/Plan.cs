namespace Kenc.Cloudflare.Core.Entities
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Class wrapping Plan objects in Cloudflare API
    /// </summary>
    public class Plan
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("frequency")]
        public string? Fequency { get; set; }

        [JsonPropertyName("legacy_id")]
        public string? LegacyId { get; set; }

        [JsonPropertyName("is_subscribed")]
        public bool IsSubscribed { get; set; }

        [JsonPropertyName("can_subscribe")]
        public bool CanSubscribe { get; set; }
    }
}
