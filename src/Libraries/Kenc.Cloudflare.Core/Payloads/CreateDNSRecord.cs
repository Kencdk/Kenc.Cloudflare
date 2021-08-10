namespace Kenc.Cloudflare.Core.Payloads
{
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.PayloadEntities;
    using Newtonsoft.Json;

    public class CreateDNSRecord : ICloudflarePayload
    {
        [JsonProperty(propertyName: "name")]
        public string Name { get; }

        [JsonProperty(propertyName: "type")]
        public DNSRecordType Type { get; }

        [JsonProperty(propertyName: "content")]
        public string Content { get; }

        [JsonProperty(propertyName: "ttl")]
        public int? TimeToLive { get; }

        [JsonProperty(propertyName: "priority")]
        public int? Priority { get; }

        [JsonProperty(propertyName: "proxied")]
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
