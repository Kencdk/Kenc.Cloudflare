namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    /// <summary>
    /// Client for interacting with Zone Settings, https://api.cloudflare.com/#zone-settings-properties
    /// </summary>
    public class ZoneSettingsClient : CloudflareEntityClient
    {
        private const string EntityNamePlural = "settings";

        private readonly Uri _baseUri;

        public ZoneSettingsClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            this._baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        }

        /// <summary>
        /// Retrieves available settings for your user in relation to a zone
        /// </summary>
        /// <param name="zoneIdentifier">Target zone identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="EntityList{ZoneSetting}" /></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<EntityList<ZoneSetting>> ListAsync(string zoneIdentifier, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(zoneIdentifier))
            {
                throw new ArgumentNullException(nameof(zoneIdentifier));
            }

            var uri = new Uri(_baseUri, $"{ZoneClient.EntityNamePlural}/{Uri.EscapeDataString(zoneIdentifier)}/{EntityNamePlural}");
            return await GetAsync<EntityList<ZoneSetting>>(uri, cancellationToken);
        }

        /// <summary>
        /// Retrieves a single setting.
        /// </summary>
        /// <param name="zoneIdentifier">Target zone identifier.</param>
        /// <param name="name">Target setting name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        public async Task<ZoneSetting> GetAsync(string zoneIdentifier, string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(zoneIdentifier))
            {
                throw new ArgumentNullException(nameof(zoneIdentifier));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var uri = new Uri(_baseUri, $"{ZoneClient.EntityNamePlural}/{Uri.EscapeDataString(zoneIdentifier)}/{EntityNamePlural}/{Uri.EscapeDataString(name)}");
            return await GetAsync<ZoneSetting>(uri, cancellationToken);
        }
    }
}
