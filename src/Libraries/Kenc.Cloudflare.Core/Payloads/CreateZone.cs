namespace Kenc.Cloudflare.Core.PayloadEntities
{
    using System.Text.Json.Serialization;
    using Kenc.Cloudflare.Core.Entities;

    public class CreateZonePayload : ICloudflarePayload
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("account")]
        public Account Account { get; }

        public CreateZonePayload(string name, Account account)
        {
            Name = name;
            Account = account;
        }
    }
}
