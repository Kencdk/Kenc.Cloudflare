namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    public class UserClient : IUserClient
    {
        private readonly IRestClient restClient;
        private readonly Uri baseUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient"/> class.
        /// </summary>
        /// <param name="restClient">Client to use to send requests.</param>
        public UserClient(IRestClient restClient, Uri baseUri)
        {
            this.baseUri = baseUri;
            this.restClient = restClient;
        }

        public async Task<User> GetUserAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var targetUri = new Uri(baseUri, "user");
            return await restClient.GetAsync<User>(targetUri, cancellationToken);
        }

        public Task<User> PatchUserAsync(string firstName = null, string lastName = null, string telephone = null, string country = null, string zipcode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
