namespace Kenc.Cloudflare.Core.Payloads
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.PayloadEntities;

    public class PurgeCachePayload : ICloudflarePayload
    {
        [JsonPropertyName("purge_everything")]
        public bool PurgeEverything { get; }

        public PurgeCachePayload(bool purgeEverything)
        {
            PurgeEverything = purgeEverything;
        }
    }
}
