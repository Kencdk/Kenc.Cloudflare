namespace Kenc.Cloudflare.Core
{
    using System.Text.Json.Serialization;

    public sealed class CloudflareResult<T> : CloudflareResult
    {
        [JsonPropertyName("result")]
#pragma warning disable CS8601 // Possible null reference assignment.
        public T Result { get; set; } = default;
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}
