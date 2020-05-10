namespace Kenc.Cloudflare.Core.Clients
{
    public interface ICloudflareClientFactory
    {
        ICloudflareClient Create();
        ICloudflareClientFactory WithAPIKey(string apiKey);
        ICloudflareClientFactory WithEndpoint(System.Uri endpoint);
        ICloudflareClientFactory WithRestClientFactory(ICloudflareRestClientFactory cloudflareRestClientFactory);
        ICloudflareClientFactory WithUsername(string username);
    }
}