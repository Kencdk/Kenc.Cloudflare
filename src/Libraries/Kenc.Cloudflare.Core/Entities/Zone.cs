namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Class wrapping zone objects in Cloudflare API
    /// https://api.cloudflare.com/#zone-properties
    /// </summary>
    public class Zone : ICloudflareEntity
    {
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// max length: 253
        /// pattern: ^([a - zA - Z0 - 9][\-a - zA - Z0 - 9]*\.)+[\-a-zA-Z0-9]{2,20}$
        /// </summary>
        [JsonProperty(propertyName: "name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(propertyName: "development_mode")]
        public int DevelopmentMode { get; set; }

        [JsonProperty(propertyName: "original_name_servers")]
        public IList<string>? OriginalNameServers { get; set; }

        [JsonProperty(propertyName: "original_registrar")]
        public string? OriginalRegistrar { get; set; }

        [JsonProperty(propertyName: "original_dnshost")]
        public string? OriginalDnsHost { get; set; }

        [JsonProperty(propertyName: "created_on")]
        public DateTime? CreatedOn { get; set; }

        [JsonProperty(propertyName: "modified_on")]
        public DateTime? ModifiedOn { get; set; }

        [JsonProperty(propertyName: "activated_on")]
        public DateTime? ActivatedOn { get; set; }

        [JsonProperty(propertyName: "owner")]
        public ZoneOwner? Owner { get; set; }

        [JsonProperty(propertyName: "Account")]
        public Account? Account { get; set; }

        [JsonProperty(propertyName: "permissions")]
        public IList<string>? Permissions { get; set; }

        [JsonProperty(propertyName: "plan")]
        public Plan? Plan { get; set; }

        [JsonProperty(propertyName: "plan_pending")]
        public Plan? PlanPending { get; set; }

        [JsonProperty(propertyName: "status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty(propertyName: "paused")]
        public bool Paused { get; set; }

        [JsonProperty(propertyName: "type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty(propertyName: "name_servers")]
        public IList<string> NameServers { get; set; } = new List<string>();
    }
}
