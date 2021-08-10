namespace Kenc.Cloudflare.Core
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Entities;

    public class CloudflareResult<T> : CloudflareResult where T : class, ICloudflareEntity
    {
        [JsonPropertyName("result")]
        public T Result { get; set; }
    }
}
