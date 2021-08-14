namespace Kenc.Cloudflare.Core.IntegrationTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Kenc.Cloudflare.Core.Clients;
    using Kenc.Cloudflare.Core.Entities.AccountMembers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AccountMembersClientTests : IntegrationTestBase
    {

        [TestMethod]
        public async Task Foobar()
        {
            var accountId = TestContextSetting("accountId");
            ICloudflareClient client = CreateClient();

            IReadOnlyList<AccountMember> memberships = await client.Accounts.AccountMembersClient.ListMemberships(accountId);

            memberships.Should().NotBeNull();
            memberships[0].Id.Should().NotBeNullOrEmpty();
            memberships[0].User.Should().NotBeNull();
            memberships[0].Roles.Should().NotBeNull();
        }
    }
}
