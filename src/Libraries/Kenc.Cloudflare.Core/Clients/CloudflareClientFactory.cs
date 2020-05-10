namespace Kenc.Cloudflare.Core.Clients
{
    using System;

    public class CloudflareClientFactory : ICloudflareClientFactory
    {
        private string username;
        private string apiKey;
        private ICloudflareRestClientFactory restClientFactory;
        private Uri endpoint;

        public CloudflareClientFactory()
        {
        }

        public ICloudflareClient Create()
        {
            return new CloudflareClient(restClientFactory, username, apiKey, endpoint);
        }

        public ICloudflareClientFactory WithUsername(string username)
        {
            this.username = username;
            return this;
        }

        public ICloudflareClientFactory WithAPIKey(string apiKey)
        {
            this.apiKey = apiKey;
            return this;
        }

        public ICloudflareClientFactory WithEndpoint(Uri endpoint)
        {
            this.endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            return this;
        }

        public ICloudflareClientFactory WithRestClientFactory(ICloudflareRestClientFactory cloudflareRestClientFactory)
        {
            this.restClientFactory = cloudflareRestClientFactory;
            return this;
        }
    }
}
