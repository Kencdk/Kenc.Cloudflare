﻿namespace Kenc.Cloudflare.Core.Entities
{
    using System.Text.Json.Serialization;

    public class CloudflareApiMessage
    {
        [JsonPropertyName("code")]
        public string Code { get; private set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; private set; } = string.Empty;
    }
}
