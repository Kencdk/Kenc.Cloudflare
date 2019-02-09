namespace Kenc.Cloudflare.Core.Exceptions
{
    using Newtonsoft.Json;

    public class CloudflareAPIError
    {
        [JsonProperty(propertyName: "code")]
        string Code { get; set; }

        [JsonProperty(propertyName: "message")]
        string Message { get; set; }

        public CloudflareAPIError(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
