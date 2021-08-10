namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Class wrapping user objects in Cloudflare API
    /// https://api.cloudflare.com/#user-properties
    /// </summary>
    public class User : ICloudflareEntity
    {
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty(propertyName: "email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty(propertyName: "first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty(propertyName: "last_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty(propertyName: "username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty(propertyName: "telephone")]
        public string? Telephone { get; set; }

        [JsonProperty(propertyName: "country")]
        public string? Country { get; set; }

        [JsonProperty(propertyName: "zipcode")]
        public string? Zipcode { get; set; }

        [JsonProperty(propertyName: "created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty(propertyName: "modified_on")]
        public DateTime ModifiedOn { get; set; }

        [JsonProperty(propertyName: "two_factor_authentication_enabled")]
        public bool TwoFactorAuthenticationEnabled { get; set; }

        [JsonProperty(propertyName: "suspended")]
        public bool Suspended { get; set; }
    }
}
