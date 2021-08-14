namespace Kenc.Cloudflare.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients.EntityClients;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities.DnsRecords;
    using Kenc.Cloudflare.Core.Tests.Helpers;
    using Kenc.Cloudflare.Core.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ZoneDNSSettingsClientTests
    {
        private static readonly string zoneIdentifier = "01a7362d577a6c3019a474fd6f485823";

        [TestMethod]
        public async Task ZoneDNSSettingsClient_GetCallsRestClient()
        {
            var dnsRecord = new DnsRecord { };
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(dnsRecord);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}/dns_records/domain.invalid"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneDnsRecordsClient(httpClient, Global.BaseUri);
            await zoneClient.GetAsync(zoneIdentifier, "domain.invalid");
        }

        [DataTestMethod]
        [DataRow(null, "name")]
        [DataRow("", "name")]
        [DataRow("name", null)]
        [DataRow("name", "")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneDNSSettingsClient_GetThrowsArgumentExceptionForInvalidInputs(string identifier, string name)
        {
            var dnsRecord = new DnsRecord { };
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(dnsRecord);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}/dns_records/domain.invalid"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneDnsRecordsClient(httpClient, Global.BaseUri);
            _ = await zoneClient.GetAsync(identifier, name);
        }

        [DataTestMethod]
        [DynamicData(nameof(ZoneDNSSettingsClient_ListPassesAppropriateParameters_Data), DynamicDataSourceType.Method)]
        public async Task ZoneDNSSettingsClient_ListPassesAppropriateParameters(string zoneIdentifier, DnsRecordType? type, string name, string content, int? page, int? perPage, string order, Direction? direction, Match? match, string expected)
        {
            var entityList = new List<DnsRecord>();
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(entityList);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}/dns_records{expected}"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneDnsRecordsClient(httpClient, Global.BaseUri);
            IReadOnlyList<DnsRecord> result = await zoneClient.ListAsync(zoneIdentifier, type, name, null, content, page, perPage, order, direction, match);

            // assert
            Assert.IsNotNull(result);
        }

        public static IEnumerable<object[]> ZoneDNSSettingsClient_ListPassesAppropriateParameters_Data()
        {
            yield return new object[] { zoneIdentifier, null, null, null, null, null, null, null, null, "" };
            yield return new object[] { zoneIdentifier, DnsRecordType.A, null, null, null, null, null, null, null, "?type=A" };
            yield return new object[] { zoneIdentifier, DnsRecordType.A, "example.invalid", null, null, null, null, null, null, "?type=A&name=example.invalid" };
            yield return new object[] { zoneIdentifier, DnsRecordType.A, "example.invalid", "127.0.0.1", null, null, null, null, null, "?type=A&name=example.invalid&content=127.0.0.1" };
            yield return new object[] { zoneIdentifier, DnsRecordType.A, "example.invalid", "127.0.0.1", 1, 20, null, null, null, "?type=A&name=example.invalid&content=127.0.0.1&page=1&per_page=20" };
            yield return new object[] { zoneIdentifier, DnsRecordType.A, "example.invalid", "127.0.0.1", 1, 20, "type", null, null, "?type=A&name=example.invalid&content=127.0.0.1&order=type&page=1&per_page=20" };
            yield return new object[] { zoneIdentifier, DnsRecordType.A, "example.invalid", "127.0.0.1", 1, 20, "type", Direction.Asc, null, "?type=A&name=example.invalid&content=127.0.0.1&order=type&page=1&per_page=20&direction=asc" };
            yield return new object[] { zoneIdentifier, DnsRecordType.A, "example.invalid", "127.0.0.1", 1, 20, "type", Direction.Asc, Match.All, "?type=A&name=example.invalid&content=127.0.0.1&match=all&order=type&page=1&per_page=20&direction=asc" };
        }
    }
}
