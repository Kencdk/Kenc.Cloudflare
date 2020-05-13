namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    public interface IZoneSettingsClient : IEntityClient
    {
        /// <summary>
        /// Retrieves available settings for your user in relation to a zone
        /// </summary>
        /// <param name="zoneIdentifier">Target zone identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An <see cref="EntityList{ZoneSettings}" /></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<EntityList<ZoneSetting>> ListAsync(string zoneIdentifier, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single setting.
        /// </summary>
        /// <param name="zoneIdentifier">Target zone identifier.</param>
        /// <param name="name">Target setting name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The setting as a <see cref="ZoneSetting"/> object.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<ZoneSetting> GetAsync(string zoneIdentifier, string name, CancellationToken cancellationToken = default);
    }
}
