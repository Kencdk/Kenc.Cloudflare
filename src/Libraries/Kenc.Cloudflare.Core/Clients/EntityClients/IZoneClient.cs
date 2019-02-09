namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    public interface IZoneClient
    {
        /// <summary>
        /// List zones.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="order"></param>
        /// <param name="direction"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<IList<Zone>> ListAsync(string name = null, string status = null, int? page = null, int? perPage = null, string order = null, string direction = null, string match = null, CancellationToken cancellationToken = default(CancellationToken));

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
        /// Attempts to delete a zone identified by <paramref name="identifier"/>
        /// </summary>
        /// <param name="identifier">Identifier of the zone to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<IdResult> DeleteAsync(string identifier, CancellationToken cancellationToken = default(CancellationToken));
    }
}