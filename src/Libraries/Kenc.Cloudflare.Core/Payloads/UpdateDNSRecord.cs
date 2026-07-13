namespace Kenc.Cloudflare.Core.Payloads
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Clients.Enums;

    public class UpdateDNSRecord : ICloudflarePayload
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("type")]
        public DNSRecordType? Type { get; }

        [JsonPropertyName("content")]
        public string? Content { get; }

        [JsonPropertyName("ttl")]
        public int? TimeToLive { get; }

        [JsonPropertyName("proxied")]
        public bool? Proxied { get; }

        public UpdateDNSRecord(string name, DNSRecordType? type, string? content, int? ttl, bool? proxied)
        {
            Name = name;
            Type = type;
            Content = content;
            TimeToLive = ttl;
            Proxied = proxied;
        }
    }
}
