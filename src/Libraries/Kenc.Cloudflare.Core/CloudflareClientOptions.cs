namespace Kenc.Cloudflare.Core
{
    using System;

    public class CloudflareClientOptions
    {
        /// <summary>
        /// Api Key to use with Cloudflare client.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Username for cloudflare client.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Api endpoint to use with Cloudflare client.
        /// </summary>
        public Uri Endpoint { get; set; }
    }
}
