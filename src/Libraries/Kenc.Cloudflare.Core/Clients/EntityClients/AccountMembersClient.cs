namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities.AccountMembers;
    using Kenc.Cloudflare.Core.ListFilters;

    /// <summary>
    /// https://api.cloudflare.com/#account-members-properties
    /// </summary>
    public class AccountMembersClient : CloudflareEntityClient
    {
        private const string EntityNamePlural = "members";

        private readonly Uri baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountMembersClient"/> class.
        /// </summary>
        /// <param name="httpClient">Client to use to send requests.</param>
        /// <param name="baseUri">Base uri.</param>
        public AccountMembersClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            this.baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        }

        /// <summary>
        /// List all members of an account
        /// </summary>
        /// <returns>A <see cref="ReadOnlyList{AccountMember}"/></returns>
        public async Task<IReadOnlyList<AccountMember>> ListMemberships(string accountId, CloudflareListFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{accountId}/{EntityNamePlural}");
            return await ListAsync<AccountMember>(targetUri, filter, cancellationToken);
        }
    }
}
