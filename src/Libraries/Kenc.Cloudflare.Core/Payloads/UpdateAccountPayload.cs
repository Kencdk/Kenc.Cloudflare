namespace Kenc.Cloudflare.Core.Payloads
{
    using Kenc.Cloudflare.Core.Entities;

    public class UpdateAccountPayload : ICloudflareEntity
    {
        public string? Name { get; set; }

        public AccountSettings? Settings { get; set; }
    }
}
