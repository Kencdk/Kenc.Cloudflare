namespace Kenc.Cloudflare.Core.Entities
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Class wrapping Account objects in Cloudflare API
    /// https://api.cloudflare.com/#accounts-properties
    /// </summary>
    public class Account : ICloudflareEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("settings")]
        public AccountSettings Settings { get; set; } = new AccountSettings();
    }
}
