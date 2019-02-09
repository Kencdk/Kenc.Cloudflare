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
    using Kenc.Cloudflare.Core.PayloadEntities;
    using Kenc.Cloudflare.Core.Payloads;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Match = Clients.Enums.Match;

    [TestClass]
    public class ZoneClientTests
    {
        private static readonly string zoneIdentifier = "01a7362d577a6c3019a474fd6f485823";

        [TestMethod]
        public async Task ZoneClient_GetCallsRestClient()
        {
            var zone = new Zone { };
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<Zone>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(zone);

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.GetAsync(zoneIdentifier);

            // assert
            Assert.AreSame(zone, result, "The returned zone object should have been passed through");
            restClient.Verify(x => x.GetAsync<Zone>(It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{zoneIdentifier}"), It.IsAny<CancellationToken>()), Times.Once, "Post should have been called on the REST client.");
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneClient_GetDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<Zone>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.GetAsync(zoneIdentifier);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneClient_GetThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var restClient = new Mock<IRestClient>();
            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.GetAsync(identifier);
        }

        [TestMethod]
        public async Task ZoneClient_ListCallsRestClient()
        {
            var zone = new EntityList<Zone>();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<EntityList<Zone>>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(zone);

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.ListAsync();

            // assert
            Assert.AreSame(zone, result, "The returned zone object should have been passed through");
            restClient.Verify(x => x.GetAsync<EntityList<Zone>>(It.Is<Uri>(y => y.PathAndQuery == "/client/v4/zones"), It.IsAny<CancellationToken>()), Times.Once, "Get should have been called on the REST client.");
        }

        [DataTestMethod]
        [DynamicData(nameof(ZoneClient_ListPassesAppropriateParameters_Data), DynamicDataSourceType.Method)]
        public async Task ZoneClient_ListPassesAppropriateParameters(string name, ZoneStatus? status, int? page, int? perPage, string order, Direction? direction, Match? match, string expected)
        {
            var zone = new EntityList<Zone>();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<EntityList<Zone>>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(zone);

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.ListAsync(name, status, page, perPage, order, direction, match);

            // assert
            Assert.AreSame(zone, result, "The returned zone object should have been passed through");
            restClient.Verify(x => x.GetAsync<EntityList<Zone>>(It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones?{expected}"), It.IsAny<CancellationToken>()), Times.Once, "Get should have been called on the REST client.");
        }

        public static IEnumerable<object[]> ZoneClient_ListPassesAppropriateParameters_Data()
        {
            yield return new object[] { "example.invalid", null, null, null, null, null, null, "name=example.invalid" };
            yield return new object[] { "example.invalid", ZoneStatus.Active, null, null, null, null, null, "name=example.invalid&status=active" };
            yield return new object[] { "example.invalid", ZoneStatus.Active, 1, 20, "status", Direction.Desc, Match.All, "name=example.invalid&status=active&page=1&per_page=20&order=status&direction=desc&match=all" };
            yield return new object[] { null, ZoneStatus.Deactivated, 1, 20, null, null, null, "status=deactivated&page=1&per_page=20" };
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneClient_ListDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.GetAsync<EntityList<Zone>>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.ListAsync();
        }

        [TestMethod]
        public async Task ZoneClient_CreateCallsRestClient()
        {
            var zone = new Zone { };
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.PostAsync<CreateZonePayload, Zone>(It.IsAny<Uri>(), It.IsAny<CreateZonePayload>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(zone);

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);

            var account = new Account
            {
                Id = "01a7362d577a6c3019a474fd6f485823",
                Name = "Demo Account"
            };
            var result = await zoneClient.CreateAsync("example.invalid", account);

            // assert
            Assert.AreSame(zone, result, "The returned zone object should have been passed through");
            restClient.Verify(x => x.PostAsync<CreateZonePayload, Zone>(It.Is<Uri>(y => y.PathAndQuery == "/client/v4/zones"), It.IsAny<CreateZonePayload>(), It.IsAny<CancellationToken>()), Times.Once, "Post should have been called on the REST client.");
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneClient_CreateDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.PostAsync<CreateZonePayload, Zone>(It.IsAny<Uri>(), It.IsAny<CreateZonePayload>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);

            var account = new Account
            {
                Id = "01a7362d577a6c3019a474fd6f485823",
                Name = "Demo Account"
            };
            var result = await zoneClient.CreateAsync("example.invalid", account);
        }

        [DataTestMethod]
        [DataRow("name", "accountId", "")]
        [DataRow("name", "accountId", null)]
        [DataRow("name", "", "accountName")]
        [DataRow("name", null, "accountName")]
        [DataRow("", "accountId", "accountName")]
        [DataRow(null, "accountId", "accountName")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneClient_CreateThrowsArgumentExceptionForInvalidIdentifierInputs(string name, string accountId, string accountName)
        {
            var restClient = new Mock<IRestClient>();
            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);

            var account = new Account
            {
                Id = accountId,
                Name = accountName
            };
            var result = await zoneClient.CreateAsync(name, account);
        }

        [TestMethod]
        public async Task ZoneClient_DeleteCallsRestClient()
        {
            var idResult = new IdResult { };
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.DeleteAsync<IdResult>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(idResult);

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);

            var identifier = "1235678";
            var result = await zoneClient.DeleteAsync(identifier);

            // assert
            Assert.AreSame(idResult, result, "The returned IdResult object should have been passed through");
            restClient.Verify(x => x.DeleteAsync<IdResult>(It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{identifier}"), It.IsAny<CancellationToken>()), Times.Once, "delete should have been called on the REST client.");
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneClient_DeleteDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.DeleteAsync<IdResult>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1001", "Invalid zone identifier")
                }));

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);

            var identifier = "1235678";
            var result = await zoneClient.DeleteAsync(identifier);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneClient_DeleteThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var restClient = new Mock<IRestClient>();
            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.DeleteAsync(identifier);
        }

        [TestMethod]
        public async Task ZoneClient_InitiateZoneActivationCheckCallsRestClient()
        {
            var idResult = new IdResult();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.PutAsync<IdResult>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(idResult);

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.InitiateZoneActivationCheckAsync(zoneIdentifier);

            // assert
            Assert.AreSame(idResult, result, "The returned zone object should have been passed through");
            restClient.Verify(x =>
                x.PutAsync<IdResult>(
                        It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{zoneIdentifier}/activation_check"),
                        It.IsAny<CancellationToken>()),
                    Times.Once,
                    "Put should have been called on the REST client.");
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneClient_InitiateZoneActivationCheckDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(x => x.PutAsync<IdResult>(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.InitiateZoneActivationCheckAsync(zoneIdentifier);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneClient_InitiateZoneActivationCheckThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var restClient = new Mock<IRestClient>();
            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.InitiateZoneActivationCheckAsync(identifier);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task ZoneClient_PurgeAllFilesCallsRestClient(bool purgeAll)
        {
            var idResult = new IdResult();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(
                x => x.PostAsync<PurgeCachePayload, IdResult>(
                    It.IsAny<Uri>(), It.IsAny<PurgeCachePayload>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(idResult);

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.PurgeAllFiles(zoneIdentifier, purgeAll);

            // assert
            Assert.AreSame(idResult, result, "The returned zone object should have been passed through");
            restClient.Verify(x =>
                x.PostAsync<PurgeCachePayload, IdResult>(
                        It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{zoneIdentifier}/purge_cache"),
                        It.Is<PurgeCachePayload>(y => y.PurgeEverything == purgeAll),
                        It.IsAny<CancellationToken>()),
                    Times.Once,
                    "Post should have been called on the REST client.");
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneClient_PurgeAllFilesDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(
                x => x.PostAsync<PurgeCachePayload, IdResult>(
                    It.IsAny<Uri>(), It.IsAny<PurgeCachePayload>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.PurgeAllFiles(zoneIdentifier, true);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneClient_PurgeAllFilesThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var restClient = new Mock<IRestClient>();
            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.PurgeAllFiles(identifier, true);
        }

        [DataTestMethod]
        [DynamicData(nameof(ZoneClient_PurgeFilesByTagsOrHostsCallsRestClient_Data), DynamicDataSourceType.Method)]
        public async Task ZoneClient_PurgeFilesByTagsOrHostsCallsRestClient(string[] tags, string[] hosts)
        {
            var idResult = new IdResult();
            var restClient = new Mock<IRestClient>();
            restClient.Setup(
                x => x.PostAsync<PurgeFilesByTagsOrHostsPayload, IdResult>(
                    It.IsAny<Uri>(), It.IsAny<PurgeFilesByTagsOrHostsPayload>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(idResult);

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.PurgeFilesByTagsOrHosts(zoneIdentifier, tags, hosts);

            // assert
            Assert.AreSame(idResult, result, "The returned zone object should have been passed through");
            restClient.Verify(x =>
                x.PostAsync<PurgeFilesByTagsOrHostsPayload, IdResult>(
                        It.Is<Uri>(y => y.PathAndQuery == $"/client/v4/zones/{zoneIdentifier}/purge_cache"),
                        It.Is<PurgeFilesByTagsOrHostsPayload>(y => y.Hosts == hosts && y.Tags == tags),
                        It.IsAny<CancellationToken>()),
                    Times.Once,
                    "Post should have been called on the REST client.");
        }

        public static IEnumerable<object[]> ZoneClient_PurgeFilesByTagsOrHostsCallsRestClient_Data()
        {
            yield return new object[] { new string[] { "some-tag", "another-tag" }, new string[] { "www.example.invalid", "images.example.invalid" } };
            yield return new object[] { new string[] { }, new string[] { "www.example.invalid", "images.example.invalid" } };
            yield return new object[] { new string[] { "some-tag", "another-tag" }, new string[] { } };
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task ZoneClient_PurgeFilesByTagsOrHostsDoesntSwallowExceptions()
        {
            var restClient = new Mock<IRestClient>();
            restClient.Setup(
                x => x.PostAsync<PurgeFilesByTagsOrHostsPayload, IdResult>(
                    It.IsAny<Uri>(), It.IsAny<PurgeFilesByTagsOrHostsPayload>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new CloudflareException(new List<CloudflareAPIError> {
                    new CloudflareAPIError("1049", "<domain> is not a registered domain")
                }));

            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.PurgeFilesByTagsOrHosts(zoneIdentifier, new string[] { "some-tag" }, new string[] { "example.invalid" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneClient_PurgeFilesByTagsOrHostsThrowsArgumentExceptionForInvalidIdentifierInputs()
        {
            var restClient = new Mock<IRestClient>();
            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.PurgeFilesByTagsOrHosts(string.Empty, new string[] { "tags" }, new string[] { "hosts" });
        }

        [DataTestMethod]
        [DynamicData(nameof(ZoneClient_PurgeFilesByTagsOrHostsThrowsArgumentExceptionForInvalidInputs_Data), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task ZoneClient_PurgeFilesByTagsOrHostsThrowsArgumentExceptionForInvalidInputs(string[] tags, string[] hosts)
        {
            var restClient = new Mock<IRestClient>();
            var zoneClient = new ZoneClient(restClient.Object, CloudflareClient.V4Endpoint);
            var result = await zoneClient.PurgeFilesByTagsOrHosts(zoneIdentifier, tags, hosts);
        }

        public static IEnumerable<object[]> ZoneClient_PurgeFilesByTagsOrHostsThrowsArgumentExceptionForInvalidInputs_Data()
        {
            yield return new object[] { null, null };
            yield return new object[] { new string[] { }, new string[] { } };
        }
    }
}
