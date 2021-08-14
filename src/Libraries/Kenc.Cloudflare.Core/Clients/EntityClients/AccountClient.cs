namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Net.Http;
    using Kenc.Cloudflare.Core.Entities.Accounts;
    using Kenc.Cloudflare.Core.ListFilters;

    public sealed class AccountClient : CloudflareEntityClient<Account, CloudflareListFilter, UpdateAccountPayload>
    {
        protected override string EntityNamePlural => "accounts";

        public AccountMembersClient AccountMembersClient { get; private set; }

        public AccountClient(HttpClient httpClient, Uri baseUri) : base(httpClient, baseUri)
        {
            AccountMembersClient = new AccountMembersClient(httpClient, new Uri(baseUri, $"{EntityNamePlural}/"));
        }
    }
}
