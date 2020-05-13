namespace Kenc.Cloudflare.Core.IntegrationTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients;
    using Microsoft.Extensions.DependencyInjection;
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

            CloudflareClient client = CreateClient();
            IList<Entities.Zone> domain = await client.Zones.ListAsync(domainName, Clients.Enums.ZoneStatus.Active);
            Assert.IsNotNull(domain);
            Assert.AreEqual(domainId, domain[0].Id);
        }

        [TestMethod]
        public async Task ListTxtRecords()
        {
            var domainId = TestContextSetting("domainId");

            CloudflareClient client = CreateClient();
            Entities.EntityList<Entities.DNSRecord> dnsRecords = await client.Zones.DNSSettings.ListAsync(domainId, Clients.Enums.DNSRecordType.TXT);
            Assert.IsNotNull(dnsRecords);
            Assert.AreNotEqual(0, dnsRecords.Count);
        }

        [TestMethod]
        public async Task CreateTextRecord()
        {
            var recordIdentifier = $"_intTest{System.DateTime.UtcNow:yyyymmddhhMM}";
            var domainId = TestContextSetting("domainId");

            CloudflareClient client = CreateClient();
            Entities.DNSRecord record = await client.Zones.DNSSettings.CreateRecordAsync(domainId, recordIdentifier, Clients.Enums.DNSRecordType.TXT, recordIdentifier);
            Assert.IsNotNull(record);
            Assert.AreEqual(recordIdentifier, record.Content);
        }

        private CloudflareClient CreateClient()
        {
            var apiKey = TestContextSetting("cloudflareapikey");
            var username = TestContextSetting("cloudflareusername");

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            var services = serviceCollection.BuildServiceProvider();
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();

            var restClientFactory = new CloudflareRestClientFactory(httpClientFactory);
            return new CloudflareClient(restClientFactory, username, apiKey, CloudflareAPIEndpoint.V4Endpoint);
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
