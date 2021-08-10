namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Class wrapping user api token objects in Cloudflare API
    /// https://api.cloudflare.com/#user-api-tokens-list-tokens
    /// </summary>
    public class UserToken : ICloudflareEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public UserTokenStatus Status { get; set; }

        [JsonPropertyName("issued_on")]
        public DateTimeOffset? IssuedOn { get; set; }

        [JsonPropertyName("modified_on")]
        public DateTimeOffset? ModifiedOn { get; set; }

        [JsonPropertyName("not_before")]
        public DateTimeOffset? NotBefore { get; set; }

        [JsonPropertyName("expires_on")]
        public DateTimeOffset? ExpiresOn { get; set; }

        [JsonPropertyName("policies")]
        public Policy[] Policies { get; set; }

        [JsonPropertyName("condition")]
        public UserTokenCondition Condition { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class UserTokenCondition
    {
        [JsonPropertyName("requestip")]
        public UserTokenRequestIPRules Requestip { get; set; }
    }

    public class UserTokenRequestIPRules
    {
        [JsonPropertyName("in")]
        public string[] In { get; set; }

        [JsonPropertyName("not_in")]
        public string[] NotIn { get; set; }
    }

    public class Policy
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("effect")]
        public string Effect { get; set; }

        [JsonPropertyName("resources")]
        public Dictionary<string, string> Resources { get; set; }

        [JsonPropertyName("permission_groups")]
        public PermissionGroup[] PermissionGroups { get; set; }
    }

    public class PermissionGroup
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public enum UserTokenStatus
    {
        [EnumMember(Value = "active")]
        Active,
        [EnumMember(Value = "disabled")]
        Disabled,
        [EnumMember(Value = "expired")]
        Expired
    }
}
