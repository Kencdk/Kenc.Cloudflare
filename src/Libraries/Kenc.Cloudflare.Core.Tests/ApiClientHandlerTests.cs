namespace Kenc.Cloudflare.Core.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Kenc.Cloudflare.Core.Exceptions;
    using Kenc.Cloudflare.Core.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ApiClientHandlerTests
    {
        [TestMethod]
        public async Task TestExceptionHandling()
        {
            var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(@"{
                  ""result"": null,
                  ""success"": false,
                  ""errors"": [{ ""code"":1003,""message"":""Invalid or missing zone id.""}],
                  ""messages"": []
                }", Encoding.UTF8, "application/json")
            };

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(message, Global.BaseUri);
            var apiHandler = new ApiClientHandler(fakeHttpMessageHandler);
            var httpClient = new HttpClient(apiHandler);

            Func<Task> act = async () => await httpClient.PutAsync(Global.BaseUri, new StringContent("foobar"), TestContext.CancellationToken);
            (await act.Should().ThrowAsync<CloudflareException>())
                .And.Errors[0].Code.Should().Be("1003");
        }

        /// <summary>
        /// Tests that a non-JSON error response (e.g., HTML) still results in a CloudflareException being thrown, rather than being swallowed or treated as a success.
        /// </summary>
        [TestMethod]
        public async Task TestNonJsonErrorBodyStillThrowsInsteadOfSwallowing()
        {
            var message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("<html>Internal Server Error</html>", Encoding.UTF8, "text/html")
            };

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(message, Global.BaseUri);
            var apiHandler = new ApiClientHandler(fakeHttpMessageHandler);
            var httpClient = new HttpClient(apiHandler);

            Func<Task> act = async () => await httpClient.PutAsync(Global.BaseUri, new StringContent("foobar"), TestContext.CancellationToken);
            (await act.Should().ThrowAsync<CloudflareException>())
                .And.Errors[0].Code.Should().Be("500");
        }

        [TestMethod]
        public async Task TestEmptyErrorBodyStillThrowsInsteadOfSwallowing()
        {
            var message = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
            {
                Content = new StringContent(string.Empty)
            };

            var fakeHttpMessageHandler = new FakeHttpMessageHandler(message, Global.BaseUri);
            var apiHandler = new ApiClientHandler(fakeHttpMessageHandler);
            var httpClient = new HttpClient(apiHandler);

            Func<Task> act = async () => await httpClient.PutAsync(Global.BaseUri, new StringContent("foobar"), TestContext.CancellationToken);
            await act.Should().ThrowAsync<CloudflareException>();
        }

        public TestContext TestContext { get; set; }
    }
}
