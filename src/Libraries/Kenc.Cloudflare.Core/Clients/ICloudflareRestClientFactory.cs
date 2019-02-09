namespace Kenc.Cloudflare.Core.Clients
{
    public interface ICloudflareRestClientFactory
    {
        IRestClient CreateRestClient(string username, string apiKey);
    }
}
