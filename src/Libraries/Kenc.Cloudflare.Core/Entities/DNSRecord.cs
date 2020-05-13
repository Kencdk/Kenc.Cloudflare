namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Newtonsoft.Json;

    public class DNSRecord : ICloudflareEntity
    {
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(propertyName: "type")]
        public DNSRecordType Type { get; set; }

        [JsonProperty(propertyName: "name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(propertyName: "content")]
        public string Content { get; set; } = string.Empty;

        [JsonProperty(propertyName: "proxiable")]
        public bool Proxiable { get; set; }

        [JsonProperty(propertyName: "proxied")]
        public bool Proxied { get; set; }

        [JsonProperty(propertyName: "ttl")]
        public int TimeToLive { get; set; }

        [JsonProperty(propertyName: "locked")]
        public bool Locked { get; set; }

        [JsonProperty(propertyName: "zone_id")]
        public string ZoneId { get; set; } = string.Empty;

        [JsonProperty(propertyName: "zone_name")]
        public string ZoneName { get; set; } = string.Empty;

        [JsonProperty(propertyName: "created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty(propertyName: "modified_on")]
        public DateTime? ModifiedOn { get; set; }

        [JsonProperty(propertyName: "data")]
        public string Data { get; set; } = string.Empty;
    }
}
