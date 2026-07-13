namespace Kenc.Cloudflare.Core.Tests
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Kenc.Cloudflare.Core;
    using Kenc.Cloudflare.Core.Clients.EntityClients;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Tests.Helpers;
    using Kenc.Cloudflare.Core.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UserTokensClientTests
    {
        private static readonly Uri TokensBaseUri = new(Global.BaseUri, "user/");
        private static readonly string TokenId = "ed17574386854bf78a67040be0a770b0";

        [TestMethod]
        public void Constructor_ThrowsArgumentNullExceptionForNullBaseUri()
        {
            var httpClient = new HttpClient(new FakeHttpMessageHandler([]));
            Action act = () => new UserTokensClient(httpClient, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task CreateTokenAsync_CallsRestClient()
        {
            var token = new UserToken { Id = TokenId, Name = "my token" };
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(token);
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(TokensBaseUri, "tokens"));
            var httpClient = new HttpClient(messageHandler);

            var client = new UserTokensClient(httpClient, TokensBaseUri);
            var result = await client.CreateTokenAsync("my token", [], null, null, cancellationToken: TestContext.CancellationToken);

            result.Should().NotBeNull();
            result.Id.Should().Be(TokenId);
        }

        [TestMethod]
        public async Task DeleteTokenAsync_CallsRestClient()
        {
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(new IdResult { Id = TokenId });
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(TokensBaseUri, $"tokens/{TokenId}"));
            var httpClient = new HttpClient(messageHandler);

            var client = new UserTokensClient(httpClient, TokensBaseUri);
            var result = await client.DeleteTokenAsync(TokenId, TestContext.CancellationToken);

            result.Id.Should().Be(TokenId);
        }

        [TestMethod]
        public async Task GetUserToken_CallsRestClient()
        {
            var token = new UserToken { Id = TokenId };
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(token);
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(TokensBaseUri, $"tokens/{TokenId}"));
            var httpClient = new HttpClient(messageHandler);

            var client = new UserTokensClient(httpClient, TokensBaseUri);
            var result = await client.GetUserToken(TokenId, TestContext.CancellationToken);

            result.Id.Should().Be(TokenId);
        }

        [TestMethod]
        public async Task ListTokensAsync_CallsRestClient()
        {
            var list = new EntityList<UserToken>();
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(list);
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(TokensBaseUri, "tokens?page=1&per_page=20&direction=asc"));
            var httpClient = new HttpClient(messageHandler);

            var client = new UserTokensClient(httpClient, TokensBaseUri);
            var result = await client.ListTokensAsync(cancellationToken: TestContext.CancellationToken);

            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task RollTokenAsync_CallsRestClient()
        {
            var response = new CloudflareResult<string> { Result = "new-secret-value" };
            var serialized = System.Text.Json.JsonSerializer.Serialize(response);
            var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(serialized, System.Text.Encoding.UTF8, "application/json")
            };
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(TokensBaseUri, $"tokens/{TokenId}/value"));
            var httpClient = new HttpClient(messageHandler);

            var client = new UserTokensClient(httpClient, TokensBaseUri);
            var result = await client.RollTokenAsync(TokenId, TestContext.CancellationToken);

            result.Should().Be("new-secret-value");
        }

        [TestMethod]
        public async Task VerifyTokenAsync_SendsBearerHeaderForSuppliedToken()
        {
            var tokenResult = new TestUserTokenResult { Id = TokenId };
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(tokenResult);
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(TokensBaseUri, "tokens/verify"));
            var httpClient = new HttpClient(messageHandler);

            var client = new UserTokensClient(httpClient, TokensBaseUri);
            var result = await client.VerifyTokenAsync("arbitrary-caller-token", TestContext.CancellationToken);

            result.Id.Should().Be(TokenId);
        }

        [TestMethod]
        public async Task UpdateTokenAsync_CallsRestClient()
        {
            var token = new UserToken { Id = TokenId, Name = "renamed" };
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(token);
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(TokensBaseUri, $"tokens/{TokenId}"));
            var httpClient = new HttpClient(messageHandler);

            var client = new UserTokensClient(httpClient, TokensBaseUri);
            var result = await client.UpdateTokenAsync(TokenId, name: "renamed", cancellationToken: TestContext.CancellationToken);

            result.Name.Should().Be("renamed");
        }

        public TestContext TestContext { get; set; }
    }
}
