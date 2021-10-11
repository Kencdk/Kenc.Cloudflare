namespace Kenc.Cloudflare.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Kenc.Cloudflare.Core.Clients.EntityClients;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Exceptions;
    using Kenc.Cloudflare.Core.Tests.Helpers;
    using Kenc.Cloudflare.Core.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Match = Clients.Enums.Match;

    [TestClass]
    public class ZoneClientTests
    {
        private static readonly string zoneIdentifier = "01a7362d577a6c3019a474fd6f485823";

        [TestMethod]
        public async Task ZoneClient_GetCallsRestClient()
        {
            var zone = new Zone { };
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(zone);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            Zone result = await zoneClient.GetAsync(zoneIdentifier);

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ZoneClient_GetDoesntSwallowExceptions()
        {
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateErrorResponse("1049", "<domain> is not a registered domain");
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);

            Func<Task> act = async () => await zoneClient.GetAsync(zoneIdentifier);

            (await act.Should().ThrowAsync<CloudflareException>())
                .And.Errors[0].Code.Should().Be("1049");
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ZoneClient_GetThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var messageHandler = new FakeHttpMessageHandler(new Dictionary<Uri, HttpResponseMessage>());
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            _ = await zoneClient.GetAsync(identifier);
        }

        [TestMethod]
        public async Task ZoneClient_ListCallsRestClient()
        {
            var zone = new EntityList<Zone>();
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(zone);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            IList<Zone> result = await zoneClient.ListAsync();

            // assert
            Assert.AreEqual(zone.Count, result.Count, "The returned zone object should have been passed through");
        }

        [DataTestMethod]
        [DynamicData(nameof(ZoneClient_ListPassesAppropriateParameters_Data), DynamicDataSourceType.Method)]
        public async Task ZoneClient_ListPassesAppropriateParameters(string name, ZoneStatus? status, int? page, int? perPage, string order, Direction? direction, Match? match, string expected)
        {
            var zone = new EntityList<Zone>();
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(zone);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones?{expected}"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            IList<Zone> result = await zoneClient.ListAsync(name, status, page, perPage, order, direction, match);

            // assert
            Assert.AreEqual(zone.Count, result.Count, "The returned zone object should have been passed through");
        }

        public static IEnumerable<object[]> ZoneClient_ListPassesAppropriateParameters_Data()
        {
            yield return new object[] { "example.invalid", null, null, null, null, null, null, "name=example.invalid" };
            yield return new object[] { "example.invalid", ZoneStatus.Active, null, null, null, null, null, "name=example.invalid&status=active" };
            yield return new object[] { "example.invalid", ZoneStatus.Active, 1, 20, "status", Direction.Desc, Match.All, "name=example.invalid&status=active&page=1&per_page=20&order=status&direction=desc&match=all" };
            yield return new object[] { null, ZoneStatus.Deactivated, 1, 20, null, null, null, "status=deactivated&page=1&per_page=20" };
        }

        [TestMethod]
        public async Task ZoneClient_ListDoesntSwallowExceptions()
        {
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateErrorResponse("1049", "<domain> is not a registered domain");
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);

            Func<Task> act = async () => await zoneClient.ListAsync();

            (await act.Should().ThrowAsync<CloudflareException>())
                .And.Errors[0].Code.Should().Be("1049");
        }

        [TestMethod]
        public async Task ZoneClient_CreateCallsRestClient()
        {
            var zone = new Zone { };
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(zone);
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            var account = new Account
            {
                Id = "01a7362d577a6c3019a474fd6f485823",
                Name = "Demo Account"
            };
            Zone result = await zoneClient.CreateAsync("example.invalid", account);

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ZoneClient_CreateDoesntSwallowExceptions()
        {
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateErrorResponse("1049", "<domain> is not a registered domain");
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);

            var account = new Account
            {
                Id = "01a7362d577a6c3019a474fd6f485823",
                Name = "Demo Account"
            };

            Func<Task> act = async () => await zoneClient.CreateAsync("example.invalid", account);

            (await act.Should().ThrowAsync<CloudflareException>())
                .And.Errors[0].Code.Should().Be("1049");
        }

        [DataTestMethod]
        [DataRow("name", "accountId", "")]
        [DataRow("name", "accountId", null)]
        [DataRow("name", "", "accountName")]
        [DataRow("name", null, "accountName")]
        [DataRow("", "accountId", "accountName")]
        [DataRow(null, "accountId", "accountName")]
        public async Task ZoneClient_CreateThrowsArgumentExceptionForInvalidIdentifierInputs(string name, string accountId, string accountName)
        {
            var messageHandler = new FakeHttpMessageHandler(new Dictionary<Uri, HttpResponseMessage>());
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);

            var account = new Account
            {
                Id = accountId,
                Name = accountName
            };

            Func<Task> act = async () => await zoneClient.CreateAsync(name, account);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task ZoneClient_DeleteCallsRestClient()
        {
            var identifier = "1235678";

            HttpResponseMessage messageResponse = HttpResponseMessageHelper.CreateApiResponse(new IdResult { Id = identifier });
            var messageHandler = new FakeHttpMessageHandler(messageResponse, new Uri(Global.BaseUri, $"zones/{identifier}"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            _ = await zoneClient.DeleteAsync(identifier);
        }

        [TestMethod]
        public async Task ZoneClient_DeleteDoesntSwallowExceptions()
        {
            var identifier = "1235678";

            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateErrorResponse("1049", "<domain> is not a registered domain");
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{identifier}"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await zoneClient.DeleteAsync(identifier);

            (await act.Should().ThrowAsync<CloudflareException>())
                .And.Errors[0].Code.Should().Be("1049");
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public async Task ZoneClient_DeleteThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var messageHandler = new FakeHttpMessageHandler(new Dictionary<Uri, HttpResponseMessage>());
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await zoneClient.DeleteAsync(identifier);

            (await act.Should().ThrowAsync<ArgumentNullException>())
                .And.ParamName.Should().Be("identifier");
        }

        [TestMethod]
        public async Task ZoneClient_InitiateZoneActivationCheckCallsRestClient()
        {
            var identifier = "1235678";

            HttpResponseMessage messageResponse = HttpResponseMessageHelper.CreateApiResponse(new IdResult { Id = identifier });
            var messageHandler = new FakeHttpMessageHandler(messageResponse, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}/activation_check"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            _ = await zoneClient.InitiateZoneActivationCheckAsync(zoneIdentifier);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public async Task ZoneClient_InitiateZoneActivationCheckThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var messageHandler = new FakeHttpMessageHandler(new Dictionary<Uri, HttpResponseMessage>());
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await zoneClient.InitiateZoneActivationCheckAsync(identifier);

            (await act.Should().ThrowAsync<ArgumentNullException>())
                .And.ParamName.Should().Be("identifier");
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task ZoneClient_PurgeAllFilesCallsRestClient(bool purgeAll)
        {
            var identifier = "1235678";

            HttpResponseMessage messageResponse = HttpResponseMessageHelper.CreateApiResponse(new IdResult { Id = identifier });
            var messageHandler = new FakeHttpMessageHandler(messageResponse, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}/purge_cache"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            _ = await zoneClient.PurgeAllFiles(zoneIdentifier, purgeAll);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public async Task ZoneClient_PurgeAllFilesThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var messageHandler = new FakeHttpMessageHandler(new Dictionary<Uri, HttpResponseMessage>());
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await zoneClient.PurgeAllFiles(identifier, true);

            (await act.Should().ThrowAsync<ArgumentNullException>())
                .And.ParamName.Should().Be("identifier");
        }

        [DataTestMethod]
        [DynamicData(nameof(ZoneClient_PurgeFilesByTagsOrHostsCallsRestClient_Data), DynamicDataSourceType.Method)]
        public async Task ZoneClient_PurgeFilesByTagsOrHostsCallsRestClient(string[] tags, string[] hosts)
        {
            var identifier = "1235678";

            HttpResponseMessage messageResponse = HttpResponseMessageHelper.CreateApiResponse(new IdResult { Id = identifier });
            var messageHandler = new FakeHttpMessageHandler(messageResponse, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}/purge_cache"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            _ = await zoneClient.PurgeFilesByTagsOrHosts(zoneIdentifier, tags, hosts);

            // check the request for content.
        }

        public static IEnumerable<object[]> ZoneClient_PurgeFilesByTagsOrHostsCallsRestClient_Data()
        {
            yield return new object[] { new string[] { "some-tag", "another-tag" }, new string[] { "www.example.invalid", "images.example.invalid" } };
            yield return new object[] { Array.Empty<string>(), new string[] { "www.example.invalid", "images.example.invalid" } };
            yield return new object[] { new string[] { "some-tag", "another-tag" }, Array.Empty<string>() };
        }

        [TestMethod]
        public async Task ZoneClient_PurgeFilesByTagsOrHostsThrowsArgumentExceptionForInvalidIdentifierInputs()
        {
            var messageHandler = new FakeHttpMessageHandler(new Dictionary<Uri, HttpResponseMessage>());
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await zoneClient.PurgeFilesByTagsOrHosts(string.Empty, new string[] { "tags" }, new string[] { "hosts" });

            (await act.Should().ThrowAsync<ArgumentNullException>())
                .And.ParamName.Should().Be("identifier");
        }

        [DataTestMethod]
        [DynamicData(nameof(ZoneClient_PurgeFilesByTagsOrHostsThrowsArgumentExceptionForInvalidInputs_Data), DynamicDataSourceType.Method)]
        public async Task ZoneClient_PurgeFilesByTagsOrHostsThrowsArgumentExceptionForInvalidInputs(string[] tags, string[] hosts)
        {
            var messageHandler = new FakeHttpMessageHandler(new Dictionary<Uri, HttpResponseMessage>());
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await zoneClient.PurgeFilesByTagsOrHosts(zoneIdentifier, tags, hosts);
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        public static IEnumerable<object[]> ZoneClient_PurgeFilesByTagsOrHostsThrowsArgumentExceptionForInvalidInputs_Data()
        {
            yield return new object[] { null, null };
            yield return new object[] { Array.Empty<string>(), Array.Empty<string>() };
        }
    }
}
