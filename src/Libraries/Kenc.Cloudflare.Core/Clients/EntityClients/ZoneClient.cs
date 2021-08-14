namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Entities.Accounts;
    using Kenc.Cloudflare.Core.Helpers;
    using Kenc.Cloudflare.Core.Payloads;

    public class ZoneClient : CloudflareEntityClient
    {
        public static readonly string EntityNamePlural = "zones";

        private readonly Uri baseUri;

        public ZoneSettingsClient Settings { get; private set; }

        public ZoneDnsRecordsClient DNSSettings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneClient"/>
        /// </summary>
        /// <param name="httpClient">Client to use to send requests.</param>
        public ZoneClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            this.baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));

            Settings = new ZoneSettingsClient(httpClient, baseUri);
            DNSSettings = new ZoneDnsRecordsClient(httpClient, baseUri);
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

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
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
            return await PostAsync<Zone, CreateZonePayload>(new Uri(baseUri, EntityNamePlural), payload, cancellationToken);
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

            return await GetAsync<Zone>(new Uri(baseUri, $"{EntityNamePlural}/{identifier}"), cancellationToken);
        }

        /// <summary>
        /// List zones.
        /// </summary>
        /// <param name="domain">Filter to include this domain.</param>
        /// <param name="status">Filter for status.</param>
        /// <param name="page">Page (when paging results).</param>
        /// <param name="perPage">Results per page.</param>
        /// <param name="order">Sorting order.</param>
        /// <param name="direction">Sorting direction.</param>
        /// <param name="match">Match settings.</param>
        /// <returns><see cref="IReadOnlyList{Zone}"/> of all zones matching the filters.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<IReadOnlyList<Zone>> ListAsync(string? domain = null, ZoneStatus? status = null, int? page = null, int? perPage = null, string? order = null, Direction? direction = null, Match? match = null, CancellationToken cancellationToken = default)
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

            var queryString = parameters.Any() ? $"?{string.Join('&', parameters)}" : string.Empty;

            var uri = new Uri(baseUri, $"{EntityNamePlural}{queryString}");
            return await GetAsync<IReadOnlyList<Zone>>(uri, cancellationToken);
        }

        /// <summary>
        /// Patches a zone.
        /// Only non-null values will be updated.
        /// https://api.cloudflare.com/#zone-edit-zone
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="paused">Indicates if the zone is only using Cloudflare DNS services. A true value means the zone will not receive security or performance benefits.</param>
        /// <param name="vanityNameServers">An array of domains used for custom name servers. This is only available for Business and Enterprise plans.</param>
        /// <param name="planId">The desired plan for the zone. Changing this value will create/cancel associated subscriptions. To view available plans for this zone, see Zone Plans</param>
        /// <returns>The resulting <see cref="Zone"/> object.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<Zone> PatchZoneAsync(string identifier, bool? paused = null, IList<string>? vanityNameServers = null, string? planId = null, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/{identifier}");
            var payload = new UpdateZonePayload
            {
                Paused = paused,
                Plan = string.IsNullOrEmpty(planId) ? null : new Plan { Id = planId },
                VanityNameServers = vanityNameServers?.ToArray()
            };

            return await PatchAsync<Zone, UpdateZonePayload>(targetUri, payload, cancellationToken);
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
            return await PutAsync<IdResult>(uri, cancellationToken);
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
            return await PostAsync<IdResult, PurgeCachePayload>(uri, payload, cancellationToken);
        }

        /// <summary>
        /// Granularly remove one or more files from Cloudflare's cache either by specifying the host or the associated Cache-Tag.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="tags">Array of tags to clean/</param>
        /// <param name="hosts">Array of hosts to clean.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Identifier of the operation as an <see cref="IdResult"/>.</returns>
        /// <remarks>Enterprise only feature.</remarks>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
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
            return await PostAsync<IdResult, PurgeFilesByTagsOrHostsPayload>(uri, payload, cancellationToken);
        }

        /// <summary>
        /// Attempts to delete a zone identified by <paramref name="identifier"/>
        /// </summary>
        /// <param name="identifier">Identifier of the zone to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="IdResult"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<IdResult> DeleteAsync(string identifier, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            return await DeleteAsync<IdResult>(new Uri(baseUri, $"{EntityNamePlural}/{identifier}"), cancellationToken);
        }
    }
}
