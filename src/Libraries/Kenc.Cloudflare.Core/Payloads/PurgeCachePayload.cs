namespace Kenc.Cloudflare.Core.Payloads
{
    using Kenc.Cloudflare.Core.PayloadEntities;
    using Newtonsoft.Json;

    public class PurgeCachePayload : ICloudflarePayload
    {
        [JsonProperty(propertyName: "purge_everything")]
        public bool PurgeEverything { get; }

        public PurgeCachePayload(bool purgeEverything)
        {
            PurgeEverything = purgeEverything;
        }
    }
}
