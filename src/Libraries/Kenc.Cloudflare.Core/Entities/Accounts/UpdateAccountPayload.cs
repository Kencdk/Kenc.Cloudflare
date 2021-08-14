namespace Kenc.Cloudflare.Core.Entities.Accounts
{
    using Kenc.Cloudflare.Core.Payloads;

    public class UpdateAccountPayload : ICloudflarePayload
    {
        public string? Name { get; set; }

        public AccountSettings? Settings { get; set; }
    }
}
