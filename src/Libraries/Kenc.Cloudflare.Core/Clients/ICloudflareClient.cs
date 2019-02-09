namespace Kenc.Cloudflare.Core.Clients
{
    using Kenc.Cloudflare.Core.Clients.EntityClients;

    public interface ICloudflareClient
    {
        IZoneClient Zones { get; }

        IUserClient UserClient { get; }
    }
}
