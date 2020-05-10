namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities;

    public interface IZoneDNSSettingsClient : IEntityClient
    {
        /// <summary>
        /// List, search, sort, and filter a zones' DNS records.
        /// </summary>
        /// <param name="zoneIdentifier">Target zone identifier.</param>
        /// <param name="type">DNS record type.</param>
        /// <param name="name">DNS record name.</param>
        /// <param name="content">DNS record content.</param>
        /// <param name="page">Page number of paginated results.</param>
        /// <param name="perPage">Number of DNS records per page.</param>
        /// <param name="order">Field to order records by.</param>
        /// <param name="direction">Direction to order domains.</param>
        /// <param name="match">Whether to match all search requirements or at least one (any).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="EntityList{DNSRecord}" /></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<EntityList<DNSRecord>> ListAsync(string zoneIdentifier, DNSRecordType? type = null, string name = null, string content = null, int? page = null, int? perPage = null, string order = null, Direction? direction = null, Match? match = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single DNS record.
        /// </summary>
        /// <param name="zoneIdentifier">Target zone identifier.</param>
        /// <param name="name">Target dns setting name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="DNSRecord"/></returns>
        Task<DNSRecord> GetAsync(string zoneIdentifier, string name, CancellationToken cancellationToken = default);

        /// Creates a new DNS record
        /// </summary>
        /// <param name="zoneIdentifier">Target zone identifier.</param>
        /// <param name="name">Target dns setting name.</param>
        /// <param name="type">DNS record type.</param>
        /// <param name="content">Content of the DNS record.</param>
        /// <param name="ttl">Time to live</param>
        /// <param name="priority">Priority</param>
        /// <param name="proxied">Wether traffic is proxied.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="DNSRecord"/></returns>
        Task<DNSRecord> CreateRecordAsync(string zoneIdentififer, string name, DNSRecordType type, string content, int? ttl = null, int? priority = null, bool? proxied = null, CancellationToken cancellationToken = default);
    
        /// <summary>
        /// Delete a single DNS record.
        /// </summary>
        /// <param name="record">DNS Record to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="IdResult"/></returns>
        Task<IdResult> DeleteRecord(DNSRecord record, CancellationToken cancellationToken = default);
    }
}
