namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities;

    /// <summary>
    /// Interface for clients interacting with the Cloudflare API Zone entity.
    /// https://api.cloudflare.com/#zone-properties
    /// </summary>
    public interface IZoneClient : IEntityClient
    {
        IZoneSettingsClient Settings { get; }
        IZoneDNSSettingsClient DNSSettings { get; }

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
        /// <returns><see cref="IList{Zone}"/> of all zones matching the filters.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<IList<Zone>> ListAsync(string? domain = null, ZoneStatus? status = null, int? page = null, int? perPage = null, string? order = null, Direction? direction = null, Match? match = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single zone based on <paramref name="identifier"/>.
        /// </summary>
        /// <param name="identifier">Zone identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Zone"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<Zone> GetAsync(string identifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new zone
        /// </summary>
        /// <param name="name">Name of the zone.</param>
        /// <param name="account">Account object with name and id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created <see cref="Zone"/> object.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<Zone> CreateAsync(string name, Account account, CancellationToken cancellationToken = default);

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
        Task<Zone> PatchZoneAsync(string identifier, bool? paused = null, IList<string>? vanityNameServers = null, string? planId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Initiate another zone activation check for the target zone.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Identifier of the new zone check as an <see cref="IdResult"/>.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<IdResult> InitiateZoneActivationCheckAsync(string identifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove ALL files from Cloudflares cache.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="purgeEverything">Purge everything?</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Identifier of the new zone check as an <see cref="IdResult"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<IdResult> PurgeAllFiles(string identifier, bool purgeEverything, CancellationToken cancellationToken = default);

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
        Task<IdResult> PurgeFilesByTagsOrHosts(string identifier, string[] tags, string[] hosts, CancellationToken cancellationToken = default);

        /// <summary>
        /// Attempts to delete a zone identified by <paramref name="identifier"/>
        /// </summary>
        /// <param name="identifier">Identifier of the zone to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="IdResult"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<IdResult> DeleteAsync(string identifier, CancellationToken cancellationToken = default);
    }
}