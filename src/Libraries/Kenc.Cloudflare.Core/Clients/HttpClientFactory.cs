namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net.Http;

    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly string apiKey;
        private readonly string username;
        private readonly IServiceProvider serviceProvider;

        public HttpClientFactory(IServiceProvider serviceProvider, string username, string apiKey)
        {
            this.apiKey = apiKey;
            this.username = username;
            this.serviceProvider = serviceProvider;
        }

        public HttpClient CreateClient(string name)
        {
            var httpClient = (HttpClient)this.serviceProvider.GetService(typeof(HttpClient));
            httpClient.DefaultRequestHeaders.Add("X-Auth-Key", this.apiKey);
            httpClient.DefaultRequestHeaders.Add("X-Auth-Email", this.username);

            return httpClient;
        }
    }
}
