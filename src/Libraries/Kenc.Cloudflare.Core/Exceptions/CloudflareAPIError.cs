namespace Kenc.Cloudflare.Core.Exceptions
{
    using Newtonsoft.Json;

    /// <summary>
    /// Wrapper for errors from the Cloudflare REST API.
    /// https://api.cloudflare.com/#getting-started-responses
    /// </summary>
    public class CloudflareAPIError
    {
        [JsonProperty(propertyName: "code")]
        public string Code { get; private set; }

        [JsonProperty(propertyName: "message")]
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instace of the <see cref="CloudflareAPIError"/> class.
        /// </summary>
        /// <param name="code">Cloudflare API error code.</param>
        /// <param name="message">Cloudflare API error message.</param>
        public CloudflareAPIError(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
