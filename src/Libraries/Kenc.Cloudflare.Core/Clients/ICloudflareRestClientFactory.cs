namespace Kenc.Cloudflare.Core.Clients
{
    /// <summary>
    /// Interface for creating restful clients.
    /// </summary>
    public interface ICloudflareRestClientFactory
    {
        /// <summary>
        /// Creates a new <see cref="IRestClient"/> with the <paramref name="username"/> and <paramref name="apiKey"/> for authentication.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="apiKey">API Key</param>
        /// <returns><see cref="IRestClient"/></returns>
        IRestClient CreateRestClient(string username, string apiKey);
    }
}
