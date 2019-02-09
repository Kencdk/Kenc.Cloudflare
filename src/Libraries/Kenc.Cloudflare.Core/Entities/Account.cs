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
        public string Id { get; set; }

        [JsonProperty(propertyName: "name")]
        public string Name { get; set; }

        [JsonProperty(propertyName: "settings")]
        public AccountSettings Settings { get; set; }
    }
}
