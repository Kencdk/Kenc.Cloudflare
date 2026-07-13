namespace Kenc.Cloudflare.Core.Tests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using FluentAssertions;
    using Kenc.Cloudflare.Core.Clients;
    using Kenc.Cloudflare.Core.Tests.Mocks;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CloudflareClientTests
    {
        [TestMethod]
        public void Constructor_ThrowsArgumentNullExceptionForNullHttpClientFactory()
        {
            var options = Options.Create(new CloudflareClientOptions
            {
                Endpoint = CloudflareAPIEndpoint.V4Endpoint,
                UserToken = "sometoken"
            });

            Action act = () => new CloudflareClient(null, options);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ThrowsArgumentNullExceptionForNullOptionsValue()
        {
            var factory = new FakeHttpClientFactory(new FakeHttpMessageHandler([]));
            var options = Options.Create<CloudflareClientOptions>(null);

            Action act = () => new CloudflareClient(factory, options);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ThrowsArgumentNullExceptionForMissingEndpoint()
        {
            var factory = new FakeHttpClientFactory(new FakeHttpMessageHandler([]));
            var options = Options.Create(new CloudflareClientOptions
            {
                UserToken = "sometoken"
            });

            Action act = () => new CloudflareClient(factory, options);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ThrowsArgumentNullExceptionWhenNeitherApiKeyNorUserTokenIsSpecified()
        {
            var factory = new FakeHttpClientFactory(new FakeHttpMessageHandler([]));
            var options = Options.Create(new CloudflareClientOptions
            {
                Endpoint = CloudflareAPIEndpoint.V4Endpoint
            });

            Action act = () => new CloudflareClient(factory, options);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ThrowsArgumentNullExceptionWhenApiKeySpecifiedWithoutUsername()
        {
            var factory = new FakeHttpClientFactory(new FakeHttpMessageHandler([]));
            var options = Options.Create(new CloudflareClientOptions
            {
                Endpoint = CloudflareAPIEndpoint.V4Endpoint,
                ApiKey = "somekey"
            });

            Action act = () => new CloudflareClient(factory, options);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_SetsBearerAuthorizationHeaderWhenUserTokenIsSpecified()
        {
            var httpClient = new HttpClient(new FakeHttpMessageHandler([]));
            var factory = new FakeHttpClientFactory(httpClient);
            var options = Options.Create(new CloudflareClientOptions
            {
                Endpoint = CloudflareAPIEndpoint.V4Endpoint,
                UserToken = "sometoken"
            });

            _ = new CloudflareClient(factory, options);

            httpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
            httpClient.DefaultRequestHeaders.Authorization.ToString().Should().Be("Bearer sometoken");
            httpClient.DefaultRequestHeaders.Contains("X-Auth-Key").Should().BeFalse();
        }

        [TestMethod]
        public void Constructor_SetsApiKeyHeadersWhenApiKeyAndUsernameAreSpecified()
        {
            var httpClient = new HttpClient(new FakeHttpMessageHandler([]));
            var factory = new FakeHttpClientFactory(httpClient);
            var options = Options.Create(new CloudflareClientOptions
            {
                Endpoint = CloudflareAPIEndpoint.V4Endpoint,
                ApiKey = "somekey",
                Username = "someuser"
            });

            _ = new CloudflareClient(factory, options);

            httpClient.DefaultRequestHeaders.GetValues("X-Auth-Key").Single().Should().Be("somekey");
            httpClient.DefaultRequestHeaders.GetValues("X-Auth-Email").Single().Should().Be("someuser");
            httpClient.DefaultRequestHeaders.Authorization.Should().BeNull();
        }

        [TestMethod]
        public void Constructor_ExposesSubClients()
        {
            var httpClient = new HttpClient(new FakeHttpMessageHandler([]));
            var factory = new FakeHttpClientFactory(httpClient);
            var options = Options.Create(new CloudflareClientOptions
            {
                Endpoint = CloudflareAPIEndpoint.V4Endpoint,
                UserToken = "sometoken"
            });

            var client = new CloudflareClient(factory, options);

            client.Zones.Should().NotBeNull();
            client.UserClient.Should().NotBeNull();
            client.ZoneDNSSettingsClient.Should().NotBeNull();
        }
    }
}
