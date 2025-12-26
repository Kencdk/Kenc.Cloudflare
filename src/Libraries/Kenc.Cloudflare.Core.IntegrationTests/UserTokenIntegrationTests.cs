namespace Kenc.Cloudflare.Core.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients;
    using Kenc.Cloudflare.Core.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("IntegrationTests")]
    public class UserTokenIntegrationTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task CRUDUserToken()
        {
            var userTokenName = $"inttoken{DateTime.UtcNow:yyyymmddhhMMss}";

            ICloudflareClient client = CreateClient();
            await client.UserClient.UserTokenClient.ListTokensAsync(cancellationToken: TestContext.CancellationToken);

            var policy = new Policy
            {
                Effect = "allow",
                Resources = new Dictionary<string, string> {
                    { $"com.cloudflare.api.account.zone.{TestContextSetting("domainId")}", "*" }
                },
                PermissionGroups =
                [
                    new() { Id = "4755a26eedb94da69e1066d98aa820be"}
                ],
            };

            UserToken token = await client.UserClient.UserTokenClient.CreateTokenAsync(
                userTokenName,
                [policy],
                notBefore: DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(1)),
                expiresOn: DateTimeOffset.UtcNow.AddHours(1), cancellationToken: TestContext.CancellationToken);

            // get the token
            await client.UserClient.UserTokenClient.GetUserToken(token.Id, TestContext.CancellationToken);

            // verify token
            await client.UserClient.UserTokenClient.VerifyTokenAsync(token.Value, TestContext.CancellationToken);

            var newToken = await client.UserClient.UserTokenClient.RollTokenAsync(token.Id, TestContext.CancellationToken);
            await client.UserClient.UserTokenClient.VerifyTokenAsync(newToken, TestContext.CancellationToken);

            // delete token again.
            await client.UserClient.UserTokenClient.DeleteTokenAsync(token.Id, TestContext.CancellationToken);
        }
    }
}
