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
    using Kenc.Cloudflare.Core.Helpers;
    using Kenc.Cloudflare.Core.Payloads;

    public sealed class AccountClient : CloudflareEntityClient
    {
        private readonly Uri baseUri;

        private string EntityNamePlural => "accounts";

        public AccountClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            this.baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        }

        /// <summary>
        /// Get a specific membership
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="Membership"/></returns>
        public async Task<Account> GetAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/{identifier}");
            return await GetAsync<Account>(targetUri, cancellationToken);
        }

        /// <summary>
        /// List memberships of accounts the user can access
        /// </summary>
        /// <param name="page">Page (when paging results).</param>
        /// <param name="perPage">Results per page.</param>
        /// <param name="order">Sorting order.</param>
        /// <param name="direction">Sorting direction.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="IReadOnlyList{Membership}"/></returns>
        public async Task<IReadOnlyList<Membership>> ListAsync(int? page = null, int? perPage = null, string? order = null, Direction? direction = null, CancellationToken cancellationToken = default)
        {
            var parameters = new List<string>();
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

            var queryString = parameters.Any() ? $"?{string.Join('&', parameters)}" : string.Empty;

            var uri = new Uri(baseUri, $"{EntityNamePlural}{queryString}");
            return await GetAsync<EntityList<Membership>>(uri, cancellationToken);
        }

        /// <summary>
        /// Accept or reject this account invitation
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="status">New status</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="Membership"/></returns>
        public async Task<Account> UpdateAsync(string identifier, MembershipStatus status, CancellationToken cancellationToken = default)
        {
            var payload = new UpdateMembershipPayload
            {
                Status = status,
            };

            var targetUri = new Uri(baseUri, $"/{baseUri}/{identifier}");
            return await PutAsync<UpdateMembershipPayload, Account>(targetUri, payload, cancellationToken);
        }
    }
}
