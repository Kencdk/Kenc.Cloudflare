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
    using Kenc.Cloudflare.Core.Payloads;

    public class ZoneDNSSettingsClient : IZoneDNSSettingsClient
    {
        public static readonly string EntityNamePlural = "dns_records";

        private readonly Uri baseUri;
        private readonly IRestClient restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneClient"/>
        /// </summary>
        /// <param name="restClient">Client to use to send requests.</param>
        public ZoneDNSSettingsClient(IRestClient restClient, Uri baseUri)
        {
            this.baseUri = baseUri;
            this.restClient = restClient;
        }

        public async Task<DNSRecord> CreateRecordAsync(string zoneIdentififer, string name, DNSRecordType type, string content, int? ttl, int? priority, bool? proxied, CancellationToken cancellationToken = default)
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

            var payload = new CreateDNSRecord(name, type, content, ttl, priority, proxied);
            return await restClient.PostAsync<CreateDNSRecord, DNSRecord>(new Uri(baseUri, $"zones/{zoneIdentififer}/{EntityNamePlural}"), payload, cancellationToken);
        }

        /// <summary>
        /// Retrieves a single DNS record.
        /// </summary>
        /// <param name="zoneIdentifier">Target zone identifier.</param>
        /// <param name="name">Target dns setting name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="DNSRecord"/></returns>
        public async Task<DNSRecord> GetAsync(string zoneIdentifier, string name, CancellationToken cancellationToken = default(CancellationToken))
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
            return await restClient.GetAsync<DNSRecord>(uri, cancellationToken);
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
        /// <returns><see cref="EntityList{DNSRecord}" /></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        public async Task<EntityList<DNSRecord>> ListAsync(string zoneIdentifier, DNSRecordType? type = null, string name = null, string content = null, int? page = null, int? perPage = null, string order = null, Direction? direction = null, Match? match = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new List<string>();
            if (type.HasValue)
            {
                parameters.Add($"{nameof(type)}={type.ConvertToString()}");
            }

            if (!string.IsNullOrEmpty(name))
            {
                parameters.Add($"{nameof(name)}={name}");
            }

            if (!string.IsNullOrEmpty(content))
            {
                parameters.Add($"{nameof(content)}={content}");
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

            var uri = new Uri(baseUri, $"{ZoneClient.EntityNamePlural}/{zoneIdentifier}/{EntityNamePlural}{queryString}");
            return await restClient.GetAsync<EntityList<DNSRecord>>(uri, cancellationToken);
        }
    }
}
