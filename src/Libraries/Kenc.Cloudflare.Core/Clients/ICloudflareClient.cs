namespace Kenc.Cloudflare.Core.Clients
{
    using Kenc.Cloudflare.Core.Clients.EntityClients;

    /// <summary>
    /// Interface for a client interacting with the Cloudflare API.
    /// </summary>
    public interface ICloudflareClient
    {
        /// <summary>
        /// Gets a client for interacting with Zones.
        /// https://api.cloudflare.com/#zone-properties
        /// </summary>
        IZoneClient Zones { get; }

        /// <summary>
        /// Gets a client for interacting with the currently signed in user.
        /// https://api.cloudflare.com/#user-properties
        /// </summary>
        IUserClient UserClient { get; }

        /// <summary>
        /// Gets a client for interacting with a Zones DNS settings.
        /// https://api.cloudflare.com/#zone-settings-get-all-zone-settings
        /// </summary>
        IZoneDNSSettingsClient ZoneDNSSettingsClient { get; }
    }
}
