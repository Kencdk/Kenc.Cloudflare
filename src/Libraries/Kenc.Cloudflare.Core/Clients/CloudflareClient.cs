namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using Kenc.Cloudflare.Core.Clients.EntityClients;

    /// <summary>
    /// Implementation of <see cref="ICloudflareClient"/> for interacting with the Cloudflare API.
    /// </summary>
    /// <inheritdoc/>
    public class CloudflareClient : ICloudflareClient
    {
        private readonly IRestClient restClient;

        public IZoneClient Zones { get; private set; }

        public IUserClient UserClient { get; private set; }

        public IZoneDNSSettingsClient ZoneDNSSettingsClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudflareClient"/> class.
        /// </summary>
        /// <param name="restClientFactory">RestClient factory to use.</param>
        /// <param name="username">Cloudflare Username</param>
        /// <param name="apiKey">Cloudflare APIKey</param>
        /// <param name="endpoint">Cloudflare Endpoint. <see cref="CloudflareAPIEndpoint"/></param>
        /// <exception cref="ArgumentNullException">Throws when any of the parameters are null or <see cref="string.Empty"/></exception>
        public CloudflareClient(ICloudflareRestClientFactory restClientFactory, string username, string apiKey, Uri endpoint)
        {
            _ = string.IsNullOrEmpty(apiKey) ? throw new ArgumentNullException(nameof(apiKey)) : apiKey;
            _ = string.IsNullOrEmpty(username) ? throw new ArgumentNullException(nameof(username)) : username;
            _ = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            _ = restClientFactory ?? throw new ArgumentNullException(nameof(restClientFactory));

            restClient = restClientFactory.CreateRestClient(username, apiKey);

            Zones = new ZoneClient(restClient, endpoint);
            UserClient = new UserClient(restClient, endpoint);
            ZoneDNSSettingsClient = new ZoneDNSSettingsClient(restClient, endpoint);
        }
    }
}
