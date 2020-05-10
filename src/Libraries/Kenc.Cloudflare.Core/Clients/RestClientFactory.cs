namespace Kenc.Cloudflare.Core.Clients
{
    using System.Net.Http;

    public class RestClientFactory : ICloudflareRestClientFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RestClientFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public IRestClient CreateRestClient(string username, string apiKey)
        {
            return new CloudflareRestClient(httpClientFactory, username, apiKey);
        }
    }
}
