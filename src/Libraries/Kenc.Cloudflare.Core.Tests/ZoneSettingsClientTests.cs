namespace Kenc.Cloudflare.Core.Tests
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Kenc.Cloudflare.Core.Clients.EntityClients;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Tests.Helpers;
    using Kenc.Cloudflare.Core.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ZoneSettingsClientTests
    {
        private static readonly string ZoneIdentifier = "01a7362d577a6c3019a474fd6f485823";

        [TestMethod]
        public async Task ZoneClient_GetCallsRestClient()
        {
            var zoneSetting = new ZoneSetting { };
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(zoneSetting);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{ZoneIdentifier}/settings/setting"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneSettingsClient(httpClient, Global.BaseUri);
            await zoneClient.GetAsync(ZoneIdentifier, "setting", TestContext.CancellationToken);
        }

        [TestMethod]
        [DataRow(null, "name")]
        [DataRow("", "name")]
        [DataRow("name", null)]
        [DataRow("name", "")]
        public async Task ZoneClient_GetThrowsArgumentExceptionForInvalidInputs(string identifier, string name)
        {
            var messageHandler = new FakeHttpMessageHandler([]);
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneSettingsClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await zoneClient.GetAsync(identifier, name, TestContext.CancellationToken);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task ZoneClient_ListCallsRestClient()
        {
            var zone = new EntityList<ZoneSetting>();
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(zone);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{ZoneIdentifier}/settings"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneSettingsClient(httpClient, Global.BaseUri);
            await zoneClient.ListAsync(ZoneIdentifier, TestContext.CancellationToken);
        }

        [DataRow("")]
        [DataRow(null)]
        [TestMethod]
        public async Task ZoneClient_ListThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var messageHandler = new FakeHttpMessageHandler([]);
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneSettingsClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await zoneClient.ListAsync(identifier, TestContext.CancellationToken);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        public TestContext TestContext { get; set; }
    }
}
