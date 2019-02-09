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
        string Id { get; set; }

        /// <summary>
        /// max length: 253
        /// pattern: ^([a - zA - Z0 - 9][\-a - zA - Z0 - 9]*\.)+[\-a-zA-Z0-9]{2,20}$
        /// </summary>
        [JsonProperty(propertyName: "name")]
        string Name { get; set; }

        [JsonProperty(propertyName: "development_mode")]
        int DevelopmentMode { get; set; }

        [JsonProperty(propertyName: "original_name_servers")]
        IList<string> OriginalNameServers { get; set; }

        [JsonProperty(propertyName: "original_registrar")]
        string OriginalRegistrar { get; set; }

        [JsonProperty(propertyName: "original_dnshost")]
        string OriginalDnsHost { get; set; }

        [JsonProperty(propertyName: "created_on")]
        DateTime? CreatedOn { get; set; }

        [JsonProperty(propertyName: "modified_on")]
        DateTime? ModifiedOn { get; set; }

        [JsonProperty(propertyName: "activated_on")]
        DateTime? ActivatedOn { get; set; }

        [JsonProperty(propertyName: "owner")]
        ZoneOwner Owner { get; set; }

        [JsonProperty(propertyName: "Account")]
        Account Account { get; set; }

        [JsonProperty(propertyName: "permissions")]
        IList<string> Permissions { get; set; }

        [JsonProperty(propertyName: "plan")]
        Plan Plan { get; set; }

        [JsonProperty(propertyName: "plan_pending")]
        Plan PlanPending { get; set; }

        [JsonProperty(propertyName: "status")]
        string Status { get; set; }

        [JsonProperty(propertyName: "paused")]
        bool Paused { get; set; }

        [JsonProperty(propertyName: "type")]
        string Type { get; set; }

        [JsonProperty(propertyName: "name_servers")]
        IList<string> NameServers { get; set; }
    }
}
