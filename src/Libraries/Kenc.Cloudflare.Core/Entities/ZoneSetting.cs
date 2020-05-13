namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using Newtonsoft.Json;

    public class ZoneSetting : ICloudflareEntity
    {
        [JsonProperty(propertyName: "id")]
        public string? Id { get; set; }

        [JsonProperty(propertyName: "value")]
        public string? Value { get; set; }

        [JsonProperty(propertyName: "editable")]
        public bool Editable { get; set; }

        [JsonProperty(propertyName: "modified_on")]
        public DateTime ModifiedOn { get; set; }
    }
}
