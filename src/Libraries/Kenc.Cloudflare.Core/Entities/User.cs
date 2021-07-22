namespace Kenc.Cloudflare.Core.Entities
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Class wrapping user objects in Cloudflare API
    /// https://api.cloudflare.com/#user-properties
    /// </summary>
    public class User : ICloudflareEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("telephone")]
        public string? Telephone { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("zipcode")]
        public string? Zipcode { get; set; }

        [JsonPropertyName("created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("modified_on")]
        public DateTime ModifiedOn { get; set; }

        [JsonPropertyName("two_factor_authentication_enabled")]
        public bool TwoFactorAuthenticationEnabled { get; set; }

        [JsonPropertyName("suspended")]
        public bool Suspended { get; set; }
    }
}
