namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Helpers;

    /// <summary>
    /// Class for interacting with UserTokens API.
    /// </summary>
    public class UserTokensClient : CloudflareEntityClient
    {
        public static readonly string EntityNamePlural = "tokens";

        private readonly Uri baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient"/> class.
        /// </summary>
        /// <param name="restClient">Client to use to send requests.</param>
        public UserTokensClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            this.baseUri = new Uri(baseUri, EntityNamePlural);
        }

        /// <summary>
        /// Creates a new user token.
        /// </summary>
        /// <param name="name">Name of token.</param>
        /// <param name="policies">Policies that apply to the new token.</param>
        /// <param name="notBefore">Optional datetime for when the token is valid from.</param>
        /// <param name="expiresOn">Optional datetime for when the token expires.</param>
        /// <param name="conditions">Conditions for the token.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The properties of the user token.</returns>
        public async Task<UserToken> CreateTokenAsync(string name, Policy[] policies, DateTimeOffset? notBefore, DateTimeOffset? expiresOn, UserTokenCondition conditions = null, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, name);

            var userToken = new UserToken
            {
                Name = name,
                Policies = policies,
                NotBefore = notBefore,
                ExpiresOn = expiresOn,
                Condition = conditions,
            };

            return await PutAsync<UserToken, UserToken>(targetUri, userToken, cancellationToken);
        }

        /// <summary>
        /// Destroy a token
        /// </summary>
        /// <param name="id">Token identifier</param>
        /// <returns>The id of the token that was destroyed.</returns>
        public async Task<IdResult> DeleteTokenAsync(string id, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, id);
            return await DeleteAsync<IdResult>(targetUri, cancellationToken);
        }

        public async Task<UserToken> GetUserToken(string id, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, id);
            return await GetAsync<UserToken>(targetUri, cancellationToken);
        }

        /// <summary>
        /// Retrieves a list of tokens registered under the current user.
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="per_page">Results per page.</param>
        /// <param name="direction">Sorting direction.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A readonly list of <see cref="UserToken"/></returns>
        public async Task<IReadOnlyList<UserToken>> ListTokensAsync(int page = 1, int perPage = 20, Direction direction = Direction.Asc, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(baseUri, $"{EntityNamePlural}?page={page}&per_page={perPage}&direction={direction.ConvertToString()}");
            return await GetAsync<EntityList<UserToken>>(uri, cancellationToken);
        }

        public Task<string> RollTokenSecretAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<TestUserTokenResult> TestTokenAsync(string id, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, id);
            return await GetAsync<TestUserTokenResult>(targetUri, cancellationToken);
        }

        public Task<UserToken> UpdateTokenAsync(string id, DateTimeOffset? expiresOn = null, DateTimeOffset? notBefore = null, string name = null, Policy[] policies = null, UserTokenStatus? status = null, DateTimeOffset? issuedOn = null, DateTimeOffset? modifiedOn = null, UserTokenCondition condition = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
