namespace Kenc.Cloudflare.Core
{
    using Newtonsoft.Json;

    public class ResultInfo
    {
        [JsonProperty(propertyName: "page")]
        int Page { get; set; }

        [JsonProperty(propertyName: "per_page")]
        int PerPage { get; set; }

        [JsonProperty(propertyName: "total_count")]
        int TotalCount { get; set; }
    }
}
