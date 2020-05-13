namespace Kenc.Cloudflare.Core
{
    using System.Collections.Generic;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Exceptions;
    using Newtonsoft.Json;

    public class CloudflareResult<T> where T : class, ICloudflareEntity
    {
        [JsonProperty(propertyName: "result")]
        public T Result { get; set; }

        [JsonProperty(propertyName: "success")]
        public bool Success { get; set; }

        [JsonProperty(propertyName: "errors")]
        public IList<CloudflareAPIError> Errors { get; set; }

        [JsonProperty(propertyName: "messages")]
        public IList<string> Messages { get; set; }

        [JsonProperty(propertyName: "result_info")]
        public ResultInfo ResultInfo { get; set; }
    }
}
