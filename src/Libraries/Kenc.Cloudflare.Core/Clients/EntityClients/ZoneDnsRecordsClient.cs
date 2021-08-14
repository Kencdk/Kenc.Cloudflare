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
    using Kenc.Cloudflare.Core.Entities.DnsRecords;
    using Kenc.Cloudflare.Core.Payloads;

    /// <summary>
    /// Implementation of <see cref="IZoneDNSSettingsClient"/>.
    /// </summary>
    /// <inheritdoc/>
    public class ZoneDnsRecordsClient : CloudflareEntityClient
    {
        private const string EntityNamePlural = "dns_records";

        private readonly Uri baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneClient"/> class.
        /// </summary>
        /// <param name="httpClient">Client to use to send requests.</param>
        public ZoneDnsRecordsClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            this.baseUri = baseUri;
        }

        /// <summary>
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
        /// <returns><see cref="DnsRecord"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<DnsRecord> CreateRecordAsync(string zoneIdentifier, string name, DnsRecordType type, string content, int? ttl = null, int? priority = null, bool? proxied = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(zoneIdentifier))
            {
                throw new ArgumentNullException(nameof(zoneIdentifier));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            var payload = new CreateDnsRecordPayload(name, type, content, ttl, priority, proxied);
            return await PostAsync<DnsRecord, CreateDnsRecordPayload>(new Uri(baseUri, $"zones/{zoneIdentifier}/{EntityNamePlural}"), payload, cancellationToken);
        }

        /// <summary>
        /// Retrieves a single DNS record.
        /// </summary>
        /// <param name="zoneIdentifier">Target zone identifier.</param>
        /// <param name="name">Target dns setting name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="DnsRecord"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<DnsRecord> GetAsync(string zoneIdentifier, string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(zoneIdentifier))
            {
                throw new ArgumentNullException(nameof(zoneIdentifier));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var uri = new Uri(baseUri, $"{ZoneClient.EntityNamePlural}/{zoneIdentifier}/{EntityNamePlural}/{name}");
            return await GetAsync<DnsRecord>(uri, cancellationToken);
        }

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
        /// <returns><see cref="IReadOnlyList{DnsRecord}" /></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<IReadOnlyList<DnsRecord>> ListAsync(string zoneIdentifier, DnsRecordType? type = null, string? name = null, bool? proxied = null, string? content = null, int? page = null, int? perPage = null, string? order = null, Direction? direction = null, Match? match = null, CancellationToken cancellationToken = default)
        {
            var filter = new DnsRecordListFilter
            {
                Content = content,
                Direction = direction,
                Name = name,
                Page = page,
                PerPage = perPage,
                Proxied = proxied,
                Type = type,
                Match = match,
                Order = order
            };

            return await ListAsync(zoneIdentifier, filter);
        }

        public async Task<IReadOnlyList<DnsRecord>> ListAsync(string zoneIdentifier, DnsRecordListFilter filter, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(zoneIdentifier))
            {
                throw new ArgumentNullException(nameof(zoneIdentifier));
            }

            var queryString = string.Empty;
            IReadOnlyList<string>? parameters = filter.GenerateParameters();
            if (parameters.Any())
            {
                queryString = $"?{string.Join('&', parameters)}";
            }

            var uri = new Uri(baseUri, $"{ZoneClient.EntityNamePlural}/{zoneIdentifier}/{EntityNamePlural}{queryString}");
            return await GetAsync<IReadOnlyList<DnsRecord>>(uri, cancellationToken);
        }

        /// <summary>
        /// Update a DNS record.
        /// </summary>
        /// <param name="recordId">Record id.</param>
        /// <param name="zoneIdentififer">Zone identifier.</param>
        /// <param name="name">Name of the entry.</param>
        /// <param name="type">DNS entry type</param>
        /// <param name="content">DNS entry content.</param>
        /// <param name="ttl">Time to live.</param>
        /// <param name="proxied">Wether the connection should be proxied.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="DnsRecord"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<DnsRecord> UpdateRecordAsync(string recordId, string zoneIdentififer, string name, DnsRecordType type, string content, int ttl, bool proxied, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(zoneIdentififer))
            {
                throw new ArgumentNullException(nameof(zoneIdentififer));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            var payload = new UpdateDnsRecordPayload(name, type, content, ttl, proxied);
            var uri = new Uri(baseUri, $"zones/{zoneIdentififer}/{EntityNamePlural}/{recordId}");
            return await PutAsync<DnsRecord, UpdateDnsRecordPayload>(uri, payload, cancellationToken);
        }

        /// <summary>
        /// Update a DNS record.
        /// </summary>
        /// <param name="dnsRecord">Dns record object.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="DnsRecord"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public Task<DnsRecord> UpdateRecordAsync(DnsRecord dnsRecord, CancellationToken cancellationToken = default)
        {
            return UpdateRecordAsync(dnsRecord.Id, dnsRecord.ZoneId, dnsRecord.Name, dnsRecord.Type, dnsRecord.Content, dnsRecord.TimeToLive, dnsRecord.Proxied, cancellationToken);
        }

        /// <summary>
        /// Patch a DNS record.
        /// Pass in the required fields along with any optional field.
        /// </summary>
        /// <param name="recordId">Record id.</param>
        /// <param name="zoneIdentififer">Zone identifier.</param>
        /// <param name="name">Name of the entry.</param>
        /// <param name="type">Type, optional. Will update record if specified.</param>
        /// <param name="content">Content, optional. Will update the record if specified.</param>
        /// <param name="ttl">Time to live, optional. Will update the record if specified.</param>
        /// <param name="proxied">Wether the connection should be proxied, optional. Will update the record if specified.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns><see cref="DnsRecord"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<DnsRecord> PatchDnsRecord(string recordId, string zoneIdentififer, string name, DnsRecordType? type, string? content, int? ttl, bool? proxied, CancellationToken cancellationToken = default)
        {
            _ = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
            _ = string.IsNullOrEmpty(recordId) ? throw new ArgumentNullException(nameof(recordId)) : recordId;
            _ = string.IsNullOrEmpty(zoneIdentififer) ? throw new ArgumentNullException(nameof(zoneIdentififer)) : zoneIdentififer;

            var payload = new UpdateDnsRecordPayload(name, type, content, ttl, proxied);
            var uri = new Uri(baseUri, $"zones/{zoneIdentififer}/{EntityNamePlural}/{recordId}");
            return await PatchAsync<DnsRecord, UpdateDnsRecordPayload>(uri, payload, cancellationToken);
        }

        /// <summary>
        /// Delete a single DNS record.
        /// </summary>
        /// <param name="record">DNS Record to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="IdResult"/></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        public async Task<IdResult> DeleteRecordAsync(DnsRecord record, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(baseUri, $"{ZoneClient.EntityNamePlural}/{record.ZoneId}/{EntityNamePlural}/{record.Id}");
            return await DeleteAsync<IdResult>(uri, cancellationToken);
        }
    }
}
