namespace Kenc.Cloudflare.Core.Exceptions
{
    using Newtonsoft.Json;

    public class CloudflareAPIError
    {
        [JsonProperty(propertyName: "code")]
        public string Code { get; set; }

        [JsonProperty(propertyName: "message")]
        public string Message { get; set; }

        public CloudflareAPIError(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
