namespace Kenc.Cloudflare.Core.IntegrationTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [TestCategory("IntegrationTests")]
    public class DNSScenarioTests : IntegrationTestBase
    {
        [TestMethod]
        public async Task GetDomain()
        {
            var domainId = TestContextSetting("domainId");
            var domainName = TestContextSetting("domainName");

            ICloudflareClient client = CreateClient();
            IList<Entities.Zone> domain = await client.Zones.ListAsync(domainName, Clients.Enums.ZoneStatus.Active);
            Assert.IsNotNull(domain);
            Assert.AreEqual(domainId, domain[0].Id);
        }

        [TestMethod]
        public async Task ListTxtRecords()
        {
            var domainId = TestContextSetting("domainId");

            ICloudflareClient client = CreateClient();
            Entities.EntityList<Entities.DNSRecord> dnsRecords = await client.Zones.DNSSettings.ListAsync(domainId, Clients.Enums.DNSRecordType.TXT);
            Assert.IsNotNull(dnsRecords);
            Assert.AreNotEqual(0, dnsRecords.Count);
        }

        [TestMethod]
        public async Task CreateTextRecord()
        {
            var recordIdentifier = $"_intTest{System.DateTime.UtcNow:yyyymmddhhMMss}";
            var domainId = TestContextSetting("domainId");

            ICloudflareClient client = CreateClient();
            Entities.DNSRecord record = await client.Zones.DNSSettings.CreateRecordAsync(domainId, recordIdentifier, Clients.Enums.DNSRecordType.TXT, recordIdentifier);
            Assert.IsNotNull(record);
            Assert.AreEqual(recordIdentifier, record.Content);

            // delete the record again
            await client.Zones.DNSSettings.DeleteRecordAsync(record);
        }
    }
}
