namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Net.Http;
    using Kenc.Cloudflare.Core.Entities.Memberships;
    using Kenc.Cloudflare.Core.Payloads;

    public class MembershipsClient : CloudflareEntityClient<Membership, MembershipsListFilter, UpdateMembershipPayload>
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
    }
}
