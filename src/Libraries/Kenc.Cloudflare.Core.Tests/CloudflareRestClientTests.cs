namespace Kenc.Cloudflare.Core.Tests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Clients;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Exceptions;
    using Kenc.Cloudflare.Core.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    [TestClass]
    public class CloudflareRestClientTests
    {
        [TestMethod]
        public async Task CloudflareRestClient_ShouldUseHttpClientFactoryToCreateClientForGet()
        {
            var result = new CloudflareResult<User>
            {
                Result = new User()
            };

            var fakeHandler = new FakeHttpMessageHandler(CreateResponseMessage(HttpStatusCode.OK, result));
            var httpClient = new HttpClient(fakeHandler);

            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var cloudflareRestClient = new CloudflareRestClient(httpClientFactory.Object, "username", "apikey");
            var user = await cloudflareRestClient.GetAsync<User>(new System.Uri(CloudflareAPIEndpoint.V4Endpoint, "user"));

            // assert
            httpClientFactory.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Once, $"{nameof(CloudflareRestClient)} should have called httpClientFactory.CreateClient())");
        }

        [TestMethod]
        [ExpectedException(typeof(CloudflareException))]
        public async Task CloudflareRestClient_ShouldThrowCloudflareException()
        {
            var result = new CloudflareResult<User>
            {
                Errors = new List<CloudflareAPIError>
                {
                    new CloudflareAPIError("1001", "Invalid request")
                }
            };

            var fakeHandler = new FakeHttpMessageHandler(CreateResponseMessage(HttpStatusCode.BadRequest, result));
            var httpClient = new HttpClient(fakeHandler);

            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var cloudflareRestClient = new CloudflareRestClient(httpClientFactory.Object, "username", "apikey");
            var user = await cloudflareRestClient.GetAsync<User>(new System.Uri(CloudflareAPIEndpoint.V4Endpoint, "user"));
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK)]
        [DataRow(HttpStatusCode.TooManyRequests)]
        [DataRow(HttpStatusCode.ServiceUnavailable)]
        [ExpectedException(typeof(CloudflareException))]
        public async Task CloudflareRestClient_ShouldThrowCloudflareExceptionRegardlessOfHttpStatusCode(HttpStatusCode httpStatusCode)
        {
            var result = new CloudflareResult<User>
            {
                Errors = new List<CloudflareAPIError>
                {
                    new CloudflareAPIError("1001", "Invalid request")
                }
            };

            var fakeHandler = new FakeHttpMessageHandler(CreateResponseMessage(httpStatusCode, result));
            var httpClient = new HttpClient(fakeHandler);

            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var cloudflareRestClient = new CloudflareRestClient(httpClientFactory.Object, "username", "apikey");
            var user = await cloudflareRestClient.GetAsync<User>(new System.Uri(CloudflareAPIEndpoint.V4Endpoint, "user"));
        }

        private HttpResponseMessage CreateResponseMessage<TResponse>(HttpStatusCode statuscode, CloudflareResult<TResponse> response) where TResponse : class, ICloudflareEntity
        {
            return new HttpResponseMessage()
            {
                StatusCode = statuscode,
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
            };
        }
    }
}
