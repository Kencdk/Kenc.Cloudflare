namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net.Http;

    /// <summary>
    /// Implementation of <see cref="ICloudflareRestClientFactory"/>
    /// </summary>
    /// <inheritdoc/>
    public class CloudflareRestClientFactory : ICloudflareRestClientFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudflareRestClientFactory"/>
        /// </summary>
        /// <param name="httpClientFactory">Http client factory.</param>
        public CloudflareRestClientFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public IRestClient CreateRestClient(string username, string password)
        {
            return new CloudflareRestClient(httpClientFactory, username, password);
        }
    }
}
