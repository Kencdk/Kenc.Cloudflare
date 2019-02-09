namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using Kenc.Cloudflare.Core.Clients.EntityClients;

    public class CloudflareClient : ICloudflareClient
    {
        public static Uri V4Endpoint = new Uri("https://api.cloudflare.com/client/v4/");

        private readonly IRestClient restClient;

        public CloudflareClient(ICloudflareRestClientFactory restClientFactory, string username, string apiKey, Uri endpoint)
        {
            restClient = restClientFactory.CreateRestClient(username, apiKey);

            Zones = new ZoneClient(restClient, endpoint);
            UserClient = new UserClient(restClient, endpoint);
        }

        public IZoneClient Zones { get; private set; }

        public IUserClient UserClient { get; private set; }
    }
}
