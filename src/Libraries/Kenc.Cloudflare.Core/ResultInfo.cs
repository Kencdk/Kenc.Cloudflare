namespace Kenc.Cloudflare.Core
{
    using System.Text.Json.Serialization;

    public class ResultInfo
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }

        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }
    }
}
