namespace Kenc.Cloudflare.Core.Payloads
{
    using System.Text.Json.Serialization;

    public class UpdateUserPayload : ICloudflarePayload
    {
        /// <summary>
        /// Gets or sets the User's first name
        /// </summary>
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the User's last name
        /// </summary>
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the User's telephone number
        /// </summary>
        [JsonPropertyName("telephone")]
        public string? Telephone { get; set; }

        /// <summary>
        /// Gets or sets the country in which the user lives.
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the zipcode or postal code where the user lives.
        /// </summary>
        [JsonPropertyName("zipcode")]
        public string? Zipcode { get; set; }
    }
}
