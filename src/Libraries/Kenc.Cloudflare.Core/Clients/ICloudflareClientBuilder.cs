namespace Kenc.Cloudflare.Core.Clients
{
    /// <summary>
    /// Interface for a factory creating a <see cref="ICloudflareClient"/>.
    /// </summary>
    public interface ICloudflareClientFactory
    {
        /// <summary>
        /// Create an instance of <see cref="ICloudflareClient"/>.
        /// </summary>
        /// <returns><see cref="ICloudflareClient"/></returns>
        ICloudflareClient Create();
    }
}