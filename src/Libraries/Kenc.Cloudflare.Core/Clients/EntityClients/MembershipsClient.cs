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

    public class MembershipsClient : CloudflareEntityClient<Membership>
    {
        protected override string EntityNamePlural => "memberships";

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipsClient"/> class.
        /// </summary>
        /// <param name="httpClient">Client to use to send requests.</param>
        /// <param name="baseUri">Base uri.</param>
        public MembershipsClient(HttpClient httpClient, Uri baseUri) : base(httpClient, baseUri)
        {
        }

        /// <summary>
        /// Get a specific membership
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="Membership"/></returns>
        public async Task<Membership> GetAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/{identifier}");
            return await GetAsync<Membership>(targetUri, cancellationToken);
        }

        /// <summary>
        /// List memberships of accounts the user can access
        /// </summary>
        /// <param name="membershipStatus">Status of this membership</param>
        /// <param name="accountName">Account name</param>
        /// <param name="page">Page (when paging results).</param>
        /// <param name="perPage">Results per page.</param>
        /// <param name="order">Sorting order.</param>
        /// <param name="direction">Sorting direction.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="IReadOnlyList{Membership}"/></returns>
        public async Task<IReadOnlyList<Membership>> ListAsync(MembershipStatus? membershipStatus = null, string? accountName = null, int? page = null, int? perPage = null, string? order = null, Direction? direction = null, CancellationToken cancellationToken = default)
        {
            var parameters = new List<string>();
            if (membershipStatus.HasValue)
            {
                parameters.Add($"status={membershipStatus.ConvertToString()}");
            }

            if (!string.IsNullOrEmpty(accountName))
            {
                parameters.Add($"account.name={accountName}");
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
        public async Task<Membership> UpdateAsync(string identifier, MembershipStatus status, CancellationToken cancellationToken = default)
        {
            var payload = new UpdateMembershipPayload
            {
                Status = status,
            };

            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/{identifier}");
            return await PutAsync<UpdateMembershipPayload, Membership>(targetUri, payload, cancellationToken);
        }
    }
}
