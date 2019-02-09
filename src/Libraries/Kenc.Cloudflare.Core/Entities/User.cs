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
        string Id { get; set; }

        [JsonProperty(propertyName: "email")]
        string Email { get; set; }

        [JsonProperty(propertyName: "first_name")]
        string FirstName { get; set; }

        [JsonProperty(propertyName: "last_name")]
        string LastName { get; set; }

        [JsonProperty(propertyName: "username")]
        string Username { get; set; }

        [JsonProperty(propertyName: "telephone")]
        string Telephone { get; set; }

        [JsonProperty(propertyName: "country")]
        string Country { get; set; }

        [JsonProperty(propertyName: "zipcode")]
        string Zipcode { get; set; }

        [JsonProperty(propertyName: "created_on")]
        DateTime CreatedOn { get; set; }

        [JsonProperty(propertyName: "modified_on")]
        DateTime ModifiedOn { get; set; }

        [JsonProperty(propertyName: "two_factor_authentication_enabled")]
        bool TwoFactorAuthenticationEnabled { get; set; }

        [JsonProperty(propertyName: "suspended")]
        bool Suspended { get; set; }
    }
}
