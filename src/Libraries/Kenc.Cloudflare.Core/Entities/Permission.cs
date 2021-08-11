namespace Kenc.Cloudflare.Core.Entities
{
    using System.Text.Json.Serialization;

    public class Permission
    {
        [JsonPropertyName("read")]
        public bool Read { get; set; }

        [JsonPropertyName("write")]
        public bool Write { get; set; }
    }
}
