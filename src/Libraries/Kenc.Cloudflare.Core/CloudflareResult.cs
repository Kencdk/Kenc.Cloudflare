﻿namespace Kenc.Cloudflare.Core
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Exceptions;

    public class CloudflareResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("errors")]
        public IList<CloudflareApiError> Errors { get; set; }

        [JsonPropertyName("messages")]
        public IList<string> Messages { get; set; }

        [JsonPropertyName("result_info")]
        public ResultInfo ResultInfo { get; set; }
    }
}
