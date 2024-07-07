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
        private static readonly string zoneIdentifier = "01a7362d577a6c3019a474fd6f485823";

        [TestMethod]
        public async Task ZoneClient_GetCallsRestClient()
        {
            var zoneSetting = new ZoneSetting { };
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(zoneSetting);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}/settings/setting"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneSettingsClient(httpClient, Global.BaseUri);
            await zoneClient.GetAsync(zoneIdentifier, "setting");
        }

        [DataTestMethod]
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
            Func<Task> act = async () => await zoneClient.GetAsync(identifier, name);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task ZoneClient_ListCallsRestClient()
        {
            var zone = new EntityList<ZoneSetting>();
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(zone);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"zones/{zoneIdentifier}/settings"));
            var httpClient = new HttpClient(mesageHandler);

            var zoneClient = new ZoneSettingsClient(httpClient, Global.BaseUri);
            await zoneClient.ListAsync(zoneIdentifier);
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public async Task ZoneClient_ListThrowsArgumentExceptionForInvalidIdentifierInputs(string identifier)
        {
            var messageHandler = new FakeHttpMessageHandler([]);
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var zoneClient = new ZoneSettingsClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await zoneClient.ListAsync(identifier);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
