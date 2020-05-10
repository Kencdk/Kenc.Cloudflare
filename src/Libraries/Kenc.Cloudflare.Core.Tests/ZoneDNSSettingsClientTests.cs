namespace Kenc.Cloudflare.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients;
    using Kenc.Cloudflare.Core.Clients.EntityClients;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ZoneDNSSettingsClientTests
    {
        private static readonly string zoneIdentifier = "01a7362d577a6c3019a474fd6f485823";

        [TestMethod]
        public async Task ZoneDNSSettingsClient_GetCallsRestClient()
        {
            var zone = new DNSRecord { };
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<DNSRecord>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(zone);

            var client = new ZoneDNSSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await client.GetAsync(zoneIdentifier, "domain.invalid");

            // assert
            Assert.AreSame(zone, result, "The returned zone object should have been passed through");
            restClient.Verify(x => x.GetAsync<DNSRecord>(
                    It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{zoneIdentifier}/dns_records/domain.invalid"),
                    It.IsAny<CancellationToken>()),
                Times.Once, "Get should have been called on the REST client.");
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneDNSSettingsClient_GetDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<DNSRecord>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var client = new ZoneDNSSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            _ = await client.GetAsync(zoneIdentifier, "domain.invalid");
        }

        [DataTestMethod]
        [DataRow(null, "name")]
        [DataRow("", "name")]
        [DataRow("name", null)]
        [DataRow("name", "")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneDNSSettingsClient_GetThrowsArgumentExceptionForInvalidInputs(string identifier, string name)
        {
            var restClient = new Mock<IRestClient>();
            var client = new ZoneDNSSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            _ = await client.GetAsync(identifier, name);
        }

        [TestMethod]
        public async Task ZoneDNSSettingsClient_ListCallsRestClient()
        {
            var zone = new EntityList<DNSRecord>();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<EntityList<DNSRecord>>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(zone);

            var client = new ZoneDNSSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await client.ListAsync(zoneIdentifier);

            // assert
            Assert.AreSame(zone, result, "The returned zone object should have been passed through");
            restClient.Verify(x => x.GetAsync<EntityList<DNSRecord>>(
                    It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{zoneIdentifier}/dns_records"),
                    It.IsAny<CancellationToken>()),
                Times.Once, "Get should have been called on the REST client.");
        }

        [DataTestMethod]
        [DynamicData(nameof(ZoneDNSSettingsClient_ListPassesAppropriateParameters_Data), DynamicDataSourceType.Method)]
        public async Task ZoneDNSSettingsClient_ListPassesAppropriateParameters(string zoneIdentifier, DNSRecordType? type, string name, string content, int? page, int? perPage, string order, Direction? direction, Clients.Enums.Match? match, string expected)
        {
            var records = new EntityList<DNSRecord>();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<EntityList<DNSRecord>>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(records);

            var zoneClient = new ZoneDNSSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.ListAsync(zoneIdentifier, type, name, content, page, perPage, order, direction, match);

            // assert
            Assert.AreSame(records, result, "The returned zone object should have been passed through");
            restClient.Verify(x => x.GetAsync<EntityList<DNSRecord>>(
                    It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{zoneIdentifier}/dns_records{expected}"),
                    It.IsAny<CancellationToken>()),
                Times.Once, "Get should have been called on the REST client.");
        }

        public static IEnumerable<object[]> ZoneDNSSettingsClient_ListPassesAppropriateParameters_Data()
        {
            yield return new object[] { zoneIdentifier, null, null, null, null, null, null, null, null, "" };
            yield return new object[] { zoneIdentifier, DNSRecordType.A, null, null, null, null, null, null, null, "?type=A" };
            yield return new object[] { zoneIdentifier, DNSRecordType.A, "example.invalid", null, null, null, null, null, null, "?type=A&name=example.invalid" };
            yield return new object[] { zoneIdentifier, DNSRecordType.A, "example.invalid", "127.0.0.1", null, null, null, null, null, "?type=A&name=example.invalid&content=127.0.0.1" };
            yield return new object[] { zoneIdentifier, DNSRecordType.A, "example.invalid", "127.0.0.1", 1, 20, null, null, null, "?type=A&name=example.invalid&content=127.0.0.1&page=1&per_page=20" };
            yield return new object[] { zoneIdentifier, DNSRecordType.A, "example.invalid", "127.0.0.1", 1, 20, "type", null, null, "?type=A&name=example.invalid&content=127.0.0.1&page=1&per_page=20&order=type" };
            yield return new object[] { zoneIdentifier, DNSRecordType.A, "example.invalid", "127.0.0.1", 1, 20, "type", Direction.Asc, null, "?type=A&name=example.invalid&content=127.0.0.1&page=1&per_page=20&order=type&direction=asc" };
            yield return new object[] { zoneIdentifier, DNSRecordType.A, "example.invalid", "127.0.0.1", 1, 20, "type", Direction.Asc, Clients.Enums.Match.All, "?type=A&name=example.invalid&content=127.0.0.1&page=1&per_page=20&order=type&direction=asc&match=all" };
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneDNSSettingsClient_ListDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<EntityList<Zone>>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            _ = await zoneClient.ListAsync();
        }
    }
}
