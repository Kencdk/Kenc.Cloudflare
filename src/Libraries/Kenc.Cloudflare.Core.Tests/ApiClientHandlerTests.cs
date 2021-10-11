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

            Func<Task> act = async () => await httpClient.PutAsync(Global.BaseUri, new StringContent("foobar"));
            (await act.Should().ThrowAsync<CloudflareException>())
                .And
                .Errors[0].Should().Be("1003");
        }
    }
}
