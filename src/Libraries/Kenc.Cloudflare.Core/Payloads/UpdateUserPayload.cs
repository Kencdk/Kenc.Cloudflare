namespace Kenc.Cloudflare.Core.Payloads
{
    using Kenc.Cloudflare.Core.PayloadEntities;
    using Newtonsoft.Json;

    public class UpdateUserPayload : ICloudflarePayload
    {
        /// <summary>
        /// Gets or sets the User's first name
        /// </summary>
        [JsonProperty(propertyName: "first_name")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the User's last name
        /// </summary>
        [JsonProperty(propertyName: "last_name")]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the User's telephone number
        /// </summary>
        [JsonProperty(propertyName: "telephone")]
        public string? Telephone { get; set; }

        /// <summary>
        /// Gets or sets the country in which the user lives.
        /// </summary>
        [JsonProperty(propertyName: "country")]
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the zipcode or postal code where the user lives.
        /// </summary>
        [JsonProperty(propertyName: "zipcode")]
        public string? Zipcode { get; set; }
    }
}
