namespace Kenc.Cloudflare.Core.IntegrationTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Kenc.Cloudflare.Core.Clients;
    using Kenc.Cloudflare.Core.Entities.Accounts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AccountClientIntegrationTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task ListAccounts()
        {
            ICloudflareClient client = CreateClient();
            IReadOnlyList<Account> accountsList = await client.Accounts.ListAsync();
            accountsList.Count.Should().BeGreaterThan(0);

            Account firstAccount = accountsList[0];
            firstAccount.Id.Should().NotBeNullOrEmpty();
            firstAccount.Name.Should().NotBeNullOrEmpty();
            firstAccount.Settings.Should().NotBeNull();
        }

        [TestMethod]
        public async Task GetAccount()
        {
            var accountId = base.TestContextSetting("accountId");
            ICloudflareClient client = CreateClient();
            Account account = await client.Accounts.GetAsync(accountId);

            account.Id.Should().Be(accountId);
            account.Name.Should().NotBeNullOrEmpty();
            account.Settings.Should().NotBeNull();
        }
    }
}
