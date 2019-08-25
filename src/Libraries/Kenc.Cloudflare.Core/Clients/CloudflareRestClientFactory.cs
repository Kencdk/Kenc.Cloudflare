namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class CloudflareRestClientFactory : ICloudflareRestClientFactory
    {
        private readonly ServiceProvider services;

        public CloudflareRestClientFactory(Uri baseUri)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient("cloudflare", c =>
            {
                c.BaseAddress = baseUri;
            });

            services = serviceCollection.BuildServiceProvider();
        }

        public IRestClient CreateRestClient(string username, string password)
        {
            var httpClientFactory = services.GetService<IHttpClientFactory>();
            return new CloudflareRestClient(httpClientFactory, username, password);
        }
    }
}
