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
    public class UserClientTests
    {
        [TestMethod]
        public void Constructor_ThrowsArgumentNullExceptionForNullBaseUri()
        {
            var httpClient = new HttpClient(new FakeHttpMessageHandler([]));
            Action act = () => new UserClient(httpClient, null);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetUserAsync_CallsRestClient()
        {
            var user = new User { Id = "userid" };
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(user);
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, "user"));
            var httpClient = new HttpClient(messageHandler);

            var userClient = new UserClient(httpClient, Global.BaseUri);
            var result = await userClient.GetUserAsync(TestContext.CancellationToken);

            result.Should().NotBeNull();
            result.Id.Should().Be("userid");
        }

        [TestMethod]
        public async Task GetUserAsync_DoesntSwallowExceptions()
        {
            var responseMessage = HttpResponseMessageHelper.CreateErrorResponse("1000", "Invalid request headers");
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, "user"));
            var apiClientHandler = new ApiClientHandler(messageHandler);
            var httpClient = new HttpClient(apiClientHandler);

            var userClient = new UserClient(httpClient, Global.BaseUri);
            Func<Task> act = async () => await userClient.GetUserAsync(TestContext.CancellationToken);

            (await act.Should().ThrowAsync<Exceptions.CloudflareException>())
                .And.Errors[0].Code.Should().Be("1000");
        }

        [TestMethod]
        public async Task PatchUserAsync_CallsRestClient()
        {
            var user = new User { Id = "userid", FirstName = "Jane" };
            var responseMessage = HttpResponseMessageHelper.CreateApiResponse(user);
            var messageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, "user"));
            var httpClient = new HttpClient(messageHandler);

            var userClient = new UserClient(httpClient, Global.BaseUri);
            var result = await userClient.PatchUserAsync(firstName: "Jane", cancellationToken: TestContext.CancellationToken);

            result.Should().NotBeNull();
            result.FirstName.Should().Be("Jane");
        }

        [TestMethod]
        public void UserTokenClient_IsExposed()
        {
            var httpClient = new HttpClient(new FakeHttpMessageHandler([]));
            var userClient = new UserClient(httpClient, Global.BaseUri);

            userClient.UserTokenClient.Should().NotBeNull();
        }

        public TestContext TestContext { get; set; }
    }
}
