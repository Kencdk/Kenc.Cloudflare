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

    /// <summary>
    /// Implementation of <see cref="IZoneDNSSettingsClient"/>.
    /// </summary>
    /// <inheritdoc/>
    public class ZoneDNSSettingsClient : IZoneDNSSettingsClient
    {
        public static readonly string EntityNamePlural = "dns_records";

        private readonly Uri baseUri;
        private readonly IRestClient restClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneClient"/> class.
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

        public async Task<DNSRecord> GetAsync(string zoneIdentifier, string name, CancellationToken cancellationToken = default)
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

        public async Task<EntityList<DNSRecord>> ListAsync(string zoneIdentifier, DNSRecordType? type = null, string? name = null, string? content = null, int? page = null, int? perPage = null, string? order = null, Direction? direction = null, Match? match = null, CancellationToken cancellationToken = default)
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

        public async Task<DNSRecord> UpdateRecordAsync(string recordId, string zoneIdentififer, string name, DNSRecordType type, string content, int ttl, bool proxied, CancellationToken cancellationToken = default)
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

            var payload = new UpdateDNSRecord(name, type, content, ttl, proxied);
            var uri = new Uri(baseUri, $"zones/{zoneIdentififer}/{EntityNamePlural}/{recordId}");
            return await restClient.PutAsync<UpdateDNSRecord, DNSRecord>(uri, payload, cancellationToken);
        }

        public Task<DNSRecord> UpdateRecordAsync(DNSRecord dnsRecord, CancellationToken cancellationToken = default)
        {
            return UpdateRecordAsync(dnsRecord.Id, dnsRecord.ZoneId, dnsRecord.Name, dnsRecord.Type, dnsRecord.Content, dnsRecord.TimeToLive, dnsRecord.Proxied, cancellationToken);
        }

        public async Task<DNSRecord> PatchDNSRecordAsync(string recordId, string zoneIdentififer, string name, DNSRecordType? type, string? content, int? ttl, bool? proxied, CancellationToken cancellationToken = default)
        {
            _ = string.IsNullOrEmpty(name) ? throw new ArgumentNullException(nameof(name)) : name;
            _ = string.IsNullOrEmpty(recordId) ? throw new ArgumentNullException(nameof(recordId)) : recordId;
            _ = string.IsNullOrEmpty(zoneIdentififer) ? throw new ArgumentNullException(nameof(zoneIdentififer)) : zoneIdentififer;

            var payload = new UpdateDNSRecord(name, type, content, ttl, proxied);
            var uri = new Uri(baseUri, $"zones/{zoneIdentififer}/{EntityNamePlural}/{recordId}");
            return await restClient.PatchAsync<UpdateDNSRecord, DNSRecord>(uri, payload, cancellationToken);
        }

        public async Task<IdResult> DeleteRecordAsync(DNSRecord record, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(baseUri, $"{ZoneClient.EntityNamePlural}/{record.ZoneId}/{EntityNamePlural}/{record.Id}");
            return await restClient.DeleteAsync<IdResult>(uri, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
