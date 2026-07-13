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

        private readonly Uri _baseUri;
        private readonly UserTokensClient _userTokensClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient"/> class.
        /// </summary>
        /// <param name="httpClient">Client to use to send requests.</param>
        public UserClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            _baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            _userTokensClient = new UserTokensClient(httpClient, new Uri(baseUri, $"{EntityNameSingular}/"));
        }

        public UserTokensClient UserTokenClient => _userTokensClient;

        public async Task<User> GetUserAsync(CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(_baseUri, EntityNameSingular);
            return await GetAsync<User>(targetUri, cancellationToken);
        }

        public async Task<User> PatchUserAsync(string? firstName = null, string? lastName = null, string? telephone = null, string? country = null, string? zipcode = null, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(_baseUri, EntityNameSingular);
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
