namespace Kenc.Cloudflare.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients;
    using Kenc.Cloudflare.Core.Clients.EntityClients;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ZoneSettingsClientTests
    {
        private static readonly string zoneIdentifier = "01a7362d577a6c3019a474fd6f485823";

        [TestMethod]
        public async Task ZoneClient_GetCallsRestClient()
        {
            var zone = new ZoneSetting { };
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<ZoneSetting>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(zone);

            var zoneClient = new ZoneSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.GetAsync(zoneIdentifier, "setting");

            // assert
            Assert.AreSame(zone, result, "The returned zone object should have been passed through");
            restClient.Verify(x => x.GetAsync<ZoneSetting>(
                    It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{zoneIdentifier}/settings/setting"),
                    It.IsAny<CancellationToken>()),
                Times.Once, "Get should have been called on the REST client.");
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneClient_GetDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<ZoneSetting>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var zoneClient = new ZoneSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.GetAsync(zoneIdentifier, "setting");
        }

        [DataTestMethod]
        [DataRow(null, "name")]
        [DataRow("", "name")]
        [DataRow("name", null)]
        [DataRow("name", "")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneClient_GetThrowsArgumentExceptionForInvalidInputs(string identifier, string name)
        {
            var restClient = new Mock<IRestClient>();
            var zoneClient = new ZoneSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.GetAsync(identifier, name);
        }

        [TestMethod]
        public async Task ZoneClient_ListCallsRestClient()
        {
            var zone = new EntityList<ZoneSetting>();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<EntityList<ZoneSetting>>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(zone);

            var zoneClient = new ZoneSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.ListAsync(zoneIdentifier);

            // assert
            Assert.AreSame(zone, result, "The returned zone object should have been passed through");
            restClient.Verify(x => x.GetAsync<EntityList<ZoneSetting>>(
                    It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{zoneIdentifier}/settings"),
                    It.IsAny<CancellationToken>()),
                Times.Once, "Get should have been called on the REST client.");
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneClient_ListThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var restClient = new Mock<IRestClient>();
            var zoneClient = new ZoneSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.ListAsync(identifier);
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneClient_ListDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<EntityList<ZoneSetting>>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var zoneClient = new ZoneSettingsClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.ListAsync(zoneIdentifier);
        }
    }
}
