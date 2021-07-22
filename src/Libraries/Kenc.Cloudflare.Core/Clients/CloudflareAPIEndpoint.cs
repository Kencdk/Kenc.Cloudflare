namespace Kenc.Cloudflare.Core.Clients
{
    using System;

    /// <summary>
    /// Static class listing Cloudflare API endpoints.
    /// </summary>
    public static class CloudflareAPIEndpoint
    {
        /// <summary>
        /// Cloudflare API v4 endpoint.
        /// </summary>
        public readonly static Uri V4Endpoint = new("https://api.cloudflare.com/client/v4/");
    }
}
