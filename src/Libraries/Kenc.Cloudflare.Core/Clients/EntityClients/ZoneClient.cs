namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Helpers;
    using Kenc.Cloudflare.Core.PayloadEntities;
    using Kenc.Cloudflare.Core.Payloads;

    public class ZoneClient : IZoneClient
    {
        public static readonly string EntityNamePlural = "zones";

        private readonly Uri baseUri;
        private readonly IRestClient restClient;

        public IZoneSettingsClient Settings { get; private set; }

        public IZoneDNSSettingsClient DNSSettings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneClient"/>
        /// </summary>
        /// <param name="restClient">Client to use to send requests.</param>
        public ZoneClient(IRestClient restClient, Uri baseUri)
        {
            this.baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            this.restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));

            Settings = new ZoneSettingsClient(restClient, baseUri);
            DNSSettings = new ZoneDNSSettingsClient(restClient, baseUri);
        }

        /// <summary>
        /// Create a new zone
        /// </summary>
        /// <param name="name">Name of the zone.</param>
        /// <param name="account">Account object with name and id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Zone"/>.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<Zone> CreateAsync(string name, Account account, CancellationToken cancellationToken = default)
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
            return await restClient.PostAsync<CreateZonePayload, Zone>(new Uri(baseUri, EntityNamePlural), payload, cancellationToken);
        }

        /// <summary>
        /// Retrieves a single zone based on <paramref name="identifier"/>.
        /// </summary>
        /// <param name="identifier">Zone identifier.</param>
        /// <returns><see cref="Zone"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<Zone> GetAsync(string identifier, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return await restClient.GetAsync<Zone>(new Uri(baseUri, $"{EntityNamePlural}/{identifier}"), cancellationToken);
        }

        public async Task<IList<Zone>> ListAsync(string? domain = null, ZoneStatus? status = null, int? page = null, int? perPage = null, string? order = null, Direction? direction = null, Match? match = null, CancellationToken cancellationToken = default)
        {
            var parameters = new List<string>();
            if (!string.IsNullOrEmpty(domain))
            {
                parameters.Add($"name={domain}");
            }

            if (status.HasValue)
            {
                parameters.Add($"{nameof(status)}={status.ConvertToString()}");
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

            if (direction.HasValue)
            {
                parameters.Add($"{nameof(direction)}={direction.ConvertToString()}");
            }

            if (match.HasValue)
            {
                parameters.Add($"{nameof(match)}={match.ConvertToString()}");
            }

            var queryString = string.Empty;
            if (parameters.Any())
            {
                queryString = "?" + string.Join('&', parameters);
            }

            var uri = new Uri(baseUri, $"{EntityNamePlural}{queryString}");
            return await restClient.GetAsync<EntityList<Zone>>(uri, cancellationToken);
        }

        public Task<Zone> PatchZoneAsync(string identifier, bool? paused = null, IList<string>? vanityNameServers = null, string? planId = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initiate another zone activation check for the target zone.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Identifier of the new zone check.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<IdResult> InitiateZoneActivationCheckAsync(string identifier, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var uri = new Uri(baseUri, $"{EntityNamePlural}/{identifier}/activation_check");
            return await restClient.PutAsync<IdResult>(uri, cancellationToken);
        }

        /// <summary>
        /// Remove ALL files from Cloudflares cache.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="purgeEverything">Purge everything?</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Identifier of the new zone check.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<IdResult> PurgeAllFiles(string identifier, bool purgeEverything, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            var uri = new Uri(baseUri, $"{EntityNamePlural}/{identifier}/purge_cache");
            var payload = new PurgeCachePayload(purgeEverything);
            return await restClient.PostAsync<PurgeCachePayload, IdResult>(uri, payload, cancellationToken);
        }

        public async Task<IdResult> PurgeFilesByTagsOrHosts(string identifier, string[] tags, string[] hosts, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            if ((tags == null || !tags.Any()) && (hosts == null || !hosts.Any()))
            {
                throw new ArgumentOutOfRangeException(nameof(tags), "Tags and hosts can't both be null/empty.");
            }

            var uri = new Uri(baseUri, $"{EntityNamePlural}/{identifier}/purge_cache");
            var payload = new PurgeFilesByTagsOrHostsPayload(tags ?? new string[0], hosts ?? new string[0]);
            return await restClient.PostAsync<PurgeFilesByTagsOrHostsPayload, IdResult>(uri, payload, cancellationToken);
        }

        public async Task<IdResult> DeleteAsync(string identifier, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return await restClient.DeleteAsync<IdResult>(new Uri(baseUri, $"{EntityNamePlural}/{identifier}"), cancellationToken);
        }
    }
}
