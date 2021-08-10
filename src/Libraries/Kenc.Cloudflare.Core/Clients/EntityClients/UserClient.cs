namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Payloads;

    /// <summary>
    /// Implementation of a <see cref="IUserClient"/>
    /// </summary>
    /// <inheritdoc/>
    public class UserClient : CloudflareEntityClient
    {
        public static readonly string EntityNameSingular = "user";

        private readonly Uri baseUri;
        private readonly UserTokensClient userTokensClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient"/> class.
        /// </summary>
        /// <param name="httpClient">Client to use to send requests.</param>
        public UserClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            this.baseUri = baseUri;
            this.userTokensClient = new UserTokensClient(httpClient, new Uri(baseUri, UserTokensClient.EntityNamePlural));
        }

        public UserTokensClient UserTokenClient => userTokensClient;

        public async Task<User> GetUserAsync(CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, EntityNameSingular);
            return await GetAsync<User>(targetUri, cancellationToken);
        }

        public async Task<User> PatchUserAsync(string? firstName = null, string? lastName = null, string? telephone = null, string? country = null, string? zipcode = null, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, EntityNameSingular);
            var payload = new UpdateUserPayload
            {
                FirstName = firstName,
                LastName = lastName,
                Telephone = telephone,
                Country = country,
                Zipcode = zipcode
            };

            return await PatchAsync<UpdateUserPayload, User>(targetUri, payload, cancellationToken);
        }
    }
}
