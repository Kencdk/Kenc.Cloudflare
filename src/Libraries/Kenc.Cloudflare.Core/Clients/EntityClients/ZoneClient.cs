namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.PayloadEntities;
    using Kenc.Cloudflare.Core.Payloads;

    public class ZoneClient : IZoneClient
    {
        private readonly string entityNamePlural = "zones";

        private readonly Uri baseUri;
        private readonly IRestClient restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneClient"/>
        /// </summary>
        /// <param name="restClient">Client to use to send requests.</param>
        public ZoneClient(IRestClient restClient, Uri baseUri)
        {
            this.baseUri = baseUri;
            this.restClient = restClient;
        }

        /// <summary>
        /// Create a new zone
        /// </summary>
        /// <param name="name">Name of the zone.</param>
        /// <param name="account">Account object with name and id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Zone"/>.</returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        public async Task<Zone> CreateAsync(string name, Account account, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(account.Name))
            {
                throw new ArgumentNullException($"{nameof(account)}.{nameof(account.Name)}");
            }

            if (string.IsNullOrEmpty(account.Id))
            {
                throw new ArgumentNullException($"{nameof(account)}.{nameof(account.Id)}");
            }

            var payload = new CreateZonePayload(name, account);
            return await restClient.PostAsync<CreateZonePayload, Zone>(new Uri(baseUri, "zones"), payload, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a single zone based on <paramref name="identifier"/>.
        /// </summary>
        /// <param name="identifier">Zone identifier.</param>
        /// <returns><see cref="Zone"/></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        public async Task<Zone> GetAsync(string identifier, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return await restClient.GetAsync<Zone>(new Uri(baseUri, $"zones/{identifier}"), cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IList<Zone>> ListAsync(string name = null, string status = null, int? page = null, int? perPage = null, string order = null, string direction = null, string match = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new List<string>();
            if (!string.IsNullOrEmpty(name))
            {
                parameters.Add($"{nameof(name)}={name}");
            }

            if (!string.IsNullOrEmpty(status))
            {
                parameters.Add($"{nameof(status)}={status}");
            }

            if (page.HasValue)
            {
                parameters.Add($"{nameof(page)}={page}");
            }

            if (perPage.HasValue)
            {
                parameters.Add($"per_page={perPage}");
            }

            if (!string.IsNullOrEmpty(order))
            {
                parameters.Add($"{nameof(order)}={order}");
            }

            if (!string.IsNullOrEmpty(direction))
            {
                parameters.Add($"{nameof(direction)}={direction}");
            }

            if (!string.IsNullOrEmpty(match))
            {
                parameters.Add($"{nameof(match)}={match}");
            }

            var queryString = string.Empty;
            if (parameters.Any())
            {
                queryString = "?" + string.Join('&', parameters);
            }

            var uri = new Uri(baseUri, $"zones{queryString}");
            return await restClient.GetAsync<EntityList<Zone>>(uri, cancellationToken)
                .ConfigureAwait(false);
        }


        public async Task<IdResult> InitiateZoneActivationCheckAsync(string identifier, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var uri = new Uri(baseUri, $"zones/{identifier}/activation_check");
            return await restClient.PutAsync<IdResult>(uri, cancellationToken);
        }

        /// <summary>
        /// Remove ALL files from Cloudflares cache.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="purgeEverything">Purge everything?</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Identifier of the new zone check.</returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        public async Task<IdResult> PurgeAllFiles(string identifier, bool purgeEverything, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var uri = new Uri(baseUri, $"zones/{identifier}/purge_cache");
            var payload = new PurgeCachePayload(purgeEverything);
            return await restClient.PostAsync<PurgeCachePayload, IdResult>(uri, payload, cancellationToken);
        }

        public Task<Zone> PatchZoneAsync(string identifier, bool? paused = null, IList<string> vanityNameServers = null, string planId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<IdResult> DeleteAsync(string identifier, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return await restClient.DeleteAsync<IdResult>(new Uri(baseUri, $"zones/{identifier}"), cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
