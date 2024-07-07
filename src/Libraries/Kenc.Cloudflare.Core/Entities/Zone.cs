namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Clients.Enums;

    /// <summary>
    /// Class wrapping zone objects in Cloudflare API
    /// https://api.cloudflare.com/#zone-properties
    /// </summary>
    public class Zone : ICloudflareEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// max length: 253
        /// pattern: ^([a - zA - Z0 - 9][\-a - zA - Z0 - 9]*\.)+[\-a-zA-Z0-9]{2,20}$
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("development_mode")]
        public int DevelopmentMode { get; set; }

        [JsonPropertyName("original_name_servers")]
        public IList<string>? OriginalNameServers { get; set; }

        [JsonPropertyName("original_registrar")]
        public string? OriginalRegistrar { get; set; }

        [JsonPropertyName("original_dnshost")]
        public string? OriginalDnsHost { get; set; }

        [JsonPropertyName("created_on")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("modified_on")]
        public DateTime? ModifiedOn { get; set; }

        [JsonPropertyName("activated_on")]
        public DateTime? ActivatedOn { get; set; }

        [JsonPropertyName("owner")]
        public ZoneOwner? Owner { get; set; }

        [JsonPropertyName("Account")]
        public Account? Account { get; set; }

        [JsonPropertyName("permissions")]
        public IList<string>? Permissions { get; set; }

        [JsonPropertyName("plan")]
        public Plan? Plan { get; set; }

        [JsonPropertyName("plan_pending")]
        public Plan? PlanPending { get; set; }

        [JsonPropertyName("status")]
        public ZoneStatus Status { get; set; }

        [JsonPropertyName("paused")]
        public bool Paused { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("name_servers")]
        public IList<string> NameServers { get; set; } = [];
    }
}
