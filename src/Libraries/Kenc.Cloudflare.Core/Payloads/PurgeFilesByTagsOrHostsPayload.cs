namespace Kenc.Cloudflare.Core.Payloads
{
    using System.Text.Json.Serialization;

    public class PurgeFilesByTagsOrHostsPayload : ICloudflarePayload
    {
        [JsonPropertyName("tags")]
        public string[] Tags { get; }

        [JsonPropertyName("hosts")]
        public string[] Hosts { get; }

        public PurgeFilesByTagsOrHostsPayload(string[] tags, string[] hosts)
        {
            Tags = tags;
            Hosts = hosts;
        }
    }
}
