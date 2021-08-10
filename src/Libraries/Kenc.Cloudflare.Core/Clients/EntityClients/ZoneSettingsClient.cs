namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    public class ZoneSettingsClient : IZoneSettingsClient
    {
        public static readonly string entityNamePlural = "settings";

        private readonly Uri baseUri;
        private readonly IRestClient restClient;

        public ZoneSettingsClient(IRestClient restClient, Uri baseUri)
        {
            this.baseUri = baseUri;
            this.restClient = restClient;
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

            var uri = new Uri(baseUri, $"{ZoneClient.EntityNamePlural}/{zoneIdentifier}/{entityNamePlural}");
            return await restClient.GetAsync<EntityList<ZoneSetting>>(uri, cancellationToken);
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

            var uri = new Uri(baseUri, $"{ZoneClient.EntityNamePlural}/{zoneIdentifier}/{entityNamePlural}/{name}");
            return await restClient.GetAsync<ZoneSetting>(uri, cancellationToken);
        }
    }
}
