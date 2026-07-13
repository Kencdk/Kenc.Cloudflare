namespace Kenc.Cloudflare.Core.IntegrationTests
{
    using System.Threading.Tasks;
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

            var client = CreateClient();
            var domain = await client.Zones.ListAsync(domainName, Clients.Enums.ZoneStatus.Active, cancellationToken: TestContext.CancellationToken);
            Assert.IsNotNull(domain);
            Assert.AreEqual(domainId, domain[0].Id);
        }

        [TestMethod]
        public async Task ListTxtRecords()
        {
            var domainId = TestContextSetting("domainId");

            var client = CreateClient();
            var dnsRecords = await client.Zones.DNSSettings.ListAsync(domainId, Clients.Enums.DNSRecordType.TXT, cancellationToken: TestContext.CancellationToken);
            Assert.IsNotNull(dnsRecords);
            Assert.IsNotEmpty(dnsRecords);
        }

        [TestMethod]
        public async Task CreateTextRecord()
        {
            var recordIdentifier = $"_intTest{System.DateTime.UtcNow:yyyymmddhhMMss}";
            var domainId = TestContextSetting("domainId");

            var client = CreateClient();
            var record = await client.Zones.DNSSettings.CreateRecordAsync(domainId, recordIdentifier, Clients.Enums.DNSRecordType.TXT, recordIdentifier, cancellationToken: TestContext.CancellationToken);
            Assert.IsNotNull(record);
            Assert.AreEqual(recordIdentifier, record.Content);

            // delete the record again
            await client.Zones.DNSSettings.DeleteRecordAsync(record, TestContext.CancellationToken);
        }
    }
}
