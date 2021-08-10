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
    using Kenc.Cloudflare.Core.Payloads;

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
            this.baseUri = baseUri;
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
        /// <exception cref="Exceptions.CloudflareException">Thrown for errors returned from the API.</exception>
        public async Task<UserToken> CreateTokenAsync(string name, Policy[] policies, DateTimeOffset? notBefore, DateTimeOffset? expiresOn, UserTokenCondition? conditions = null, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}");

            var userToken = new UserToken
            {
                Name = name,
                Policies = policies,
                NotBefore = notBefore,
                ExpiresOn = expiresOn,
                Condition = conditions,
            };

            return await PostAsync<UserToken, UserToken>(targetUri, userToken, cancellationToken);
        }

        /// <summary>
        /// Destroy a token
        /// </summary>
        /// <param name="id">Token identifier</param>
        /// <returns>The id of the token that was destroyed.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown for errors returned from the API.</exception>
        public async Task<IdResult> DeleteTokenAsync(string id, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/{id}");
            return await DeleteAsync<IdResult>(targetUri, cancellationToken);
        }

        /// <summary>
        /// Get user token details.
        /// </summary>
        /// <param name="id">Token identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>User token properties</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown for errors returned from the API.</exception>
        public async Task<UserToken> GetUserToken(string id, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/{id}");
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
        /// <exception cref="Exceptions.CloudflareException">Thrown for errors returned from the API.</exception>
        public async Task<IReadOnlyList<UserToken>> ListTokensAsync(int page = 1, int perPage = 20, Direction direction = Direction.Asc, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(baseUri, $"{EntityNamePlural}?page={page}&per_page={perPage}&direction={direction.ConvertToString()}");
            return await GetAsync<EntityList<UserToken>>(uri, cancellationToken);
        }

        /// <summary>
        /// Rolls the token secret.
        /// </summary>
        /// <param name="id">Token identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The new secret.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown for errors returned from the API.</exception>
        public async Task<string> RollTokenAsync(string id, CancellationToken cancellationToken = default)
        {
            var uri = new Uri(baseUri, $"{EntityNamePlural}/{id}/value");
            return await PutAsync<string>(uri, cancellationToken);
        }

        /// <summary>
        /// Verifies a user token.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown for errors returned from the API.</exception>
        public async Task<TestUserTokenResult> VerifyTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/verify");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, targetUri);
            httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");
            return await SendMessage<TestUserTokenResult>(httpRequestMessage, cancellationToken);
        }

        /// <summary>
        /// Update a user token.
        /// Only specify the properties you wish to update.
        /// </summary>
        /// <param name="id">Token identifier.</param>
        /// <param name="expiresOn">New expires on.</param>
        /// <param name="notBefore">New not before.</param>
        /// <param name="name">New name</param>
        /// <param name="policies">New policies.</param>
        /// <param name="status">New status.</param>
        /// <param name="condition">New conditions.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated token.</returns>
        public async Task<UserToken> UpdateTokenAsync(string id, DateTimeOffset? expiresOn = null, DateTimeOffset? notBefore = null, string? name = null, Policy[]? policies = null, UserTokenStatus? status = null, UserTokenCondition? condition = null, CancellationToken cancellationToken = default)
        {
            var payload = new UpdateUserTokenPayload
            {
                Name = name,
                Status = status,
                ExpiresOn = expiresOn,
                NotBefore = notBefore,
                Condition = condition,
                Policies = policies,
            };

            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/{id}");
            return await PutAsync<UpdateUserTokenPayload, UserToken>(targetUri, payload, cancellationToken);
        }
    }
}
