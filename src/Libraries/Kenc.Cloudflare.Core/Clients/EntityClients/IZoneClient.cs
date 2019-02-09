namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System.Collections.Generic;
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
        Task<IList<Zone>> GetZonesAsync(string name = null, string status = null, int? page = null, int? perPage = null, string order = null, string direction = null, string match = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<Zone> GetZoneAsync(string identifier);

        Task<Zone> CreateZoneAsync(string name, Account account);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="paused"></param>
        /// <param name="vanityNameServers"></param>
        /// <param name="planId"></param>
        /// <returns></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<Zone> PatchZoneAsync(string identifier, bool? paused = null, IList<string> vanityNameServers = null, string planId = null);

        /// <summary>
        /// Attempts to delete a Zone.
        /// </summary>
        /// <param name="identifier">Identifier of the target Zone.</param>
        /// <returns>The identifier if succesful.</returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<string> DeleteZoneAsync(string identifier);

        /// <summary>
        /// Initiate another zone activation check for the target zone.
        /// </summary>
        /// <param name="identifier">Target zone identifier.</param>
        /// <returns>Identifier of the new zone check.</returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<string> InitiateZoneActivationCheckAsync(string identifier);
    }
}