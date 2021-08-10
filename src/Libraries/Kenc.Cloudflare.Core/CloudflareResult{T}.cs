namespace Kenc.Cloudflare.Core
{
    using System.Text.Json.Serialization;

    public sealed class CloudflareResult<T> : CloudflareResult
    {
        [JsonPropertyName("result")]
        public T Result { get; set; }
    }
}
