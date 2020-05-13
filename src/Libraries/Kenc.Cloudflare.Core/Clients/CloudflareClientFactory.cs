namespace Kenc.Cloudflare.Core.Clients
{
    using System;

    /// <summary>
    /// Implementation of <see cref="ICloudflareClientFactory"/>.
    /// </summary>
    /// <inheritdoc/>
    public class CloudflareClientFactory : ICloudflareClientFactory
    {
        private readonly string apiKey;
        private readonly string username;
        private readonly Uri endpoint;
        private readonly ICloudflareRestClientFactory restClientFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="CloudflareClientFactory"/>.
        /// </summary>
        /// <param name="username">Cloudflare username.</param>
        /// <param name="apiKey">Cloudflare API key.</param>
        /// <param name="cloudflareRestClientFactory">Rest factory.</param>
        /// <param name="endpoint">Cloudflare API endpoint. <see cref="CloudflareAPIEndpoint"/></param>
        public CloudflareClientFactory(string username, string apiKey, ICloudflareRestClientFactory cloudflareRestClientFactory, Uri endpoint)
        {
            this.apiKey = string.IsNullOrEmpty(apiKey) ? throw new ArgumentNullException(nameof(apiKey)) : apiKey;
            this.username = string.IsNullOrEmpty(username) ? throw new ArgumentNullException(nameof(username)) : username;
            this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            this.restClientFactory = cloudflareRestClientFactory ?? throw new ArgumentNullException(nameof(cloudflareRestClientFactory));
        }

        public ICloudflareClient Create()
        {
            return new CloudflareClient(restClientFactory, username, apiKey, endpoint);
        }
    }
}
