namespace Kenc.Cloudflare.Core.Payloads
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Entities;

    internal class UpdateZonePayload
    {
        [JsonPropertyName("paused")]
        public bool? Paused { get; set; }

        [JsonPropertyName("vanity_name_servers")]
        public string[]? VanityNameServers { get; set; }

        [JsonPropertyName("plan")]
        public Plan? Plan { get; set; }
    }
}
