namespace Kenc.Cloudflare.Core.IntegrationTests
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("IntegrationTests")]
    public class DNSScenarioTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task GetDomain()
        {
            var domainId = TestContextSetting("domainId");
            var domainName = TestContextSetting("domainName");

            var client = CreateClient();
            var domain = await client.Zones.ListAsync(domainName, Clients.Enums.ZoneStatus.Active);
            Assert.IsNotNull(domain);
            Assert.AreEqual(domainId, domain[0].Id);
        }

        [TestMethod]
        public async Task ListTxtRecords()
        {
            var domainId = TestContextSetting("domainId");

            var client = CreateClient();
            var dnsRecords = await client.Zones.DNSSettings.ListAsync(domainId, Clients.Enums.DNSRecordType.TXT);
            Assert.IsNotNull(dnsRecords);
            Assert.AreNotEqual(0, dnsRecords.Count);
        }

        [TestMethod]
        public async Task CreateTextRecord()
        {
            var recordIdentifier = $"_intTest{System.DateTime.UtcNow.ToString("yyyymmddhhMM")}";
            var domainId = TestContextSetting("domainId");

            var client = CreateClient();
            var record = await client.Zones.DNSSettings.CreateRecordAsync(domainId, recordIdentifier, Clients.Enums.DNSRecordType.TXT, recordIdentifier);
            Assert.IsNotNull(record);
            Assert.AreEqual(recordIdentifier, record.Content);
        }

        private Clients.CloudflareClient CreateClient()
        {
            var apiKey = TestContextSetting("cloudflareapikey");
            var username = TestContextSetting("cloudflareusername");

            var restFactory = new Clients.CloudflareRestClientFactory(Clients.CloudflareClient.V4Endpoint);
            return new Clients.CloudflareClient(restFactory, username, apiKey, Clients.CloudflareClient.V4Endpoint);
        }

        private string TestContextSetting(string name)
        {
            if (TestContext.Properties.Contains(name))
            {
                return (string)TestContext.Properties[name];
            }

            return System.Environment.GetEnvironmentVariable(name);
        }
    }
}
