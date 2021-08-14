namespace Kenc.Cloudflare.Core.Entities
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Class wrapping Account objects settings in Cloudflare API
    /// https://api.cloudflare.com/#accounts-properties
    /// </summary>
    public class AccountSettings
    {
        /// <summary>
        /// Boolean value indicating whether or not membership in this account requires that Two-Factor Authentication is enabled
        /// </summary>
        [JsonPropertyName("enforce_twofactor")]
        public bool EnforceTwofactor { get; set; }

        /// <summary>
        /// Boolean value indicating whether new zones should use the account-level custom nameservers by default
        /// </summary>
        [JsonPropertyName("use_account_custom_ns_by_default")]
        public bool UseAccountCustomNameServersByDefault { get; set; }
    }
}
