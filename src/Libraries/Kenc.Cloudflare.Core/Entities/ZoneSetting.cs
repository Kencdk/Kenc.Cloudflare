namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using Newtonsoft.Json;

    public class ZoneSetting : ICloudflareEntity
    {
        [JsonProperty(propertyName: "id")]
        string Id { get; set; }

        [JsonProperty(propertyName: "value")]
        string Value { get; set; }

        [JsonProperty(propertyName: "editable")]
        bool Editable { get; set; }

        [JsonProperty(propertyName: "modified_on")]
        DateTime ModifiedOn { get; set; }
    }
}
