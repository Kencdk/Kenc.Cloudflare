namespace Kenc.Cloudflare.Core.IntegrationTests.cs
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DNSScenarioTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task GetDomain()
        {
            var domainId = (string)TestContext.Properties["domainId"];
            var domainName = (string)TestContext.Properties["domainName"];

            var client = CreateClient();
            var domain = await client.Zones.ListAsync(domainName, Clients.Enums.ZoneStatus.Active);
            Assert.IsNotNull(domain);
            Assert.AreEqual(domainId, domain[0].Id);
        }

        [TestMethod]
        public async Task ListTxtRecords()
        {
            var domainId = (string)TestContext.Properties["domainId"];

            var client = CreateClient();
            var dnsRecords = await client.Zones.DNSSettings.ListAsync(domainId, Clients.Enums.DNSRecordType.TXT);
            Assert.IsNotNull(dnsRecords);
            Assert.AreNotEqual(0, dnsRecords.Count);
        }

        private Clients.CloudflareClient CreateClient()
        {
            var apiKey = (string)TestContext.Properties["cloudflareapikey"];
            var username = (string)TestContext.Properties["cloudflareusername"];

            var restFactory = new Clients.CloudflareRestClientFactory(Clients.CloudflareClient.V4Endpoint);
            return new Clients.CloudflareClient(restFactory, username, apiKey, Clients.CloudflareClient.V4Endpoint);
        }
    }
}
