namespace Kenc.Cloudflare.Core.Entities
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.JsonConverters;

    public class CloudflareApiMessage
    {
        [JsonPropertyName("code")]
        [JsonConverter(typeof(FlexibleStringConverter))]
        public string Code { get; private set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; private set; } = string.Empty;
    }
}
