namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities;

    public interface IZoneClient : IEntityClient
    {
        IZoneSettingsClient Settings { get; }

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
        /// <returns></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<IList<Zone>> ListAsync(string domain = null, ZoneStatus? status = null, int? page = null, int? perPage = null, string order = null, Direction? direction = null, Match? match = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieves a single zone based on <paramref name="identifier"/>.
        /// </summary>
        /// <param name="identifier">Zone identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Zone"/></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<Zone> GetAsync(string identifier, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create a new zone
        /// </summary>
        /// <param name="name">Name of the zone.</param>
        /// <param name="account">Account object with name and id.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Zone"/>.</returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<Zone> CreateAsync(string name, Account account, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="paused"></param>
        /// <param name="vanityNameServers"></param>
        /// <param name="planId"></param>
        /// <returns></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<Zone> PatchZoneAsync(string identifier, bool? paused = null, IList<string> vanityNameServers = null, string planId = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Initiate another zone activation check for the target zone.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Identifier of the new zone check.</returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<IdResult> InitiateZoneActivationCheckAsync(string identifier, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Remove ALL files from Cloudflares cache.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="purgeEverything">Purge everything?</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Identifier of the new zone check.</returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<IdResult> PurgeAllFiles(string identifier, bool purgeEverything, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Granularly remove one or more files from Cloudflare's cache either by specifying the host or the associated Cache-Tag.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <param name="tags">Array of tags to clean/</param>
        /// <param name="hosts">Array of hosts to clean.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Identifier of the operation.</returns>
        /// <remarks>Enterprise only feature.</remarks>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<IdResult> PurgeFilesByTagsOrHosts(string identifier, string[] tags, string[] hosts, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Attempts to delete a zone identified by <paramref name="identifier"/>
        /// </summary>
        /// <param name="identifier">Identifier of the zone to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<IdResult> DeleteAsync(string identifier, CancellationToken cancellationToken = default(CancellationToken));
    }
}