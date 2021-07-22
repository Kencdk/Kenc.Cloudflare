namespace Kenc.Cloudflare.Core.Exceptions
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Wrapper for errors from the Cloudflare REST API.
    /// https://api.cloudflare.com/#getting-started-responses
    /// </summary>
    public class CloudflareApiError
    {
        [JsonPropertyName("code")]
        public string Code { get; private set; }

        [JsonPropertyName("message")]
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instace of the <see cref="CloudflareApiError"/> class.
        /// </summary>
        /// <param name="code">Cloudflare API error code.</param>
        /// <param name="message">Cloudflare API error message.</param>
        public CloudflareApiError(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
