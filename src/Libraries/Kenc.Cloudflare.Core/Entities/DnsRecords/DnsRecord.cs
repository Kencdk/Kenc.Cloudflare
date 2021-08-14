using System;
using System.Text.Json.Serialization;
using Kenc.Cloudflare.Core.Clients.Enums;

namespace Kenc.Cloudflare.Core.Entities.DnsRecords
{
    public class DnsRecord : CloudflareBaseEntity
    {
        [JsonPropertyName("type")]
        public DnsRecordType Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("proxiable")]
        public bool Proxiable { get; set; }

        [JsonPropertyName("proxied")]
        public bool Proxied { get; set; }

        [JsonPropertyName("ttl")]
        public int TimeToLive { get; set; }

        [JsonPropertyName("locked")]
        public bool Locked { get; set; }

        [JsonPropertyName("zone_id")]
        public string ZoneId { get; set; } = string.Empty;

        [JsonPropertyName("zone_name")]
        public string ZoneName { get; set; } = string.Empty;

        [JsonPropertyName("created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("modified_on")]
        public DateTime? ModifiedOn { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; } = string.Empty;
    }
}
