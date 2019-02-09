namespace Kenc.Cloudflare.Core.PayloadEntities
{
    using Kenc.Cloudflare.Core.Entities;
    using Newtonsoft.Json;

    public class CreateZonePayload : ICloudflarePayload
    {
        [JsonProperty(propertyName: "name")]
        public string Name { get; }

        [JsonProperty(propertyName: "account")]
        public Account Account { get; }

        public CreateZonePayload(string name, Account account)
        {
            Name = name;
            Account = account;
        }
    }
}
