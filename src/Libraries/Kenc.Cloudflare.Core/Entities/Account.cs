namespace Kenc.Cloudflare.Core.Entities
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class wrapping Account objects in Cloudflare API
    /// https://api.cloudflare.com/#accounts-properties
    /// </summary>
    public class Account : ICloudflareEntity
    {
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(propertyName: "name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(propertyName: "settings")]
        public AccountSettings Settings { get; set; } = new AccountSettings();
    }
}
