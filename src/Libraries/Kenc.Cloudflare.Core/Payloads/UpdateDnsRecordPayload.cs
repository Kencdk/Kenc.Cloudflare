namespace Kenc.Cloudflare.Core.Payloads
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Clients.Enums;

    public class UpdateDnsRecordPayload : ICloudflarePayload
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("type")]
        public DnsRecordType? Type { get; }

        [JsonPropertyName("content")]
        public string? Content { get; }

        [JsonPropertyName("ttl")]
        public int? TimeToLive { get; }

        [JsonPropertyName("proxied")]
        public bool? Proxied { get; }

        public UpdateDnsRecordPayload(string name, DnsRecordType? type, string? content, int? ttl, bool? proxied)
        {
            Name = name;
            Type = type;
            Content = content;
            TimeToLive = ttl;
            Proxied = proxied;
        }
    }
}
