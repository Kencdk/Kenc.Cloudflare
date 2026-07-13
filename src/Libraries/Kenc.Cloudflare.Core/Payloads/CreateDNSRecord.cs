namespace Kenc.Cloudflare.Core.Payloads
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Clients.Enums;

    public class CreateDNSRecord : ICloudflarePayload
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("type")]
        public DNSRecordType Type { get; }

        [JsonPropertyName("content")]
        public string Content { get; }

        [JsonPropertyName("ttl")]
        public int? TimeToLive { get; }

        [JsonPropertyName("priority")]
        public int? Priority { get; }

        [JsonPropertyName("proxied")]
        public bool? Proxied { get; }

        public CreateDNSRecord(string name, DNSRecordType type, string content, int? ttl, int? priority, bool? proxied)
        {
            Name = name;
            Type = type;
            Content = content;
            TimeToLive = ttl;
            Priority = priority;
            Proxied = proxied;
        }
    }
}
