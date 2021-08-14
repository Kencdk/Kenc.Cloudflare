namespace Kenc.Cloudflare.Core.Tests.Clients
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients.EntityClients;
    using Kenc.Cloudflare.Core.Entities.Accounts;
    using Kenc.Cloudflare.Core.Tests.Helpers;
    using Kenc.Cloudflare.Core.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AccountClientTests
    {
        [TestMethod]
        public async Task GetAccountHitsExpectedEndpoint()
        {
            var accountId = "01a7362d577a6c3019a474fd6f485823";

            var account = new Account { };
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(account);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"accounts/{accountId}"));
            var httpClient = new HttpClient(mesageHandler);

            var client = new AccountClient(httpClient, Global.BaseUri);
            _ = await client.GetAsync(accountId);
        }

    }
}
