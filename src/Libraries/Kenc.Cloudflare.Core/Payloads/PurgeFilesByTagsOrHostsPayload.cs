namespace Kenc.Cloudflare.Core.Payloads
{
    using Kenc.Cloudflare.Core.PayloadEntities;
    using Newtonsoft.Json;

    public class PurgeFilesByTagsOrHostsPayload : ICloudflarePayload
    {
        [JsonProperty(propertyName: "tags")]
        public string[] Tags { get; }

        [JsonProperty(propertyName: "hosts")]
        public string[] Hosts { get; }

        public PurgeFilesByTagsOrHostsPayload(string[] tags, string[] hosts)
        {
            Tags = tags;
            Hosts = hosts;
        }
    }
}
