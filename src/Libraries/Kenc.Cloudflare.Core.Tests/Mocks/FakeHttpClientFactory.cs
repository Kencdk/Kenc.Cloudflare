
namespace Kenc.Cloudflare.Core.Tests.Mocks
{
    using System.Net.Http;

    public class FakeHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public FakeHttpClientFactory(HttpMessageHandler handler)
        {
            _httpClient = new HttpClient(handler);
        }

        public FakeHttpClientFactory(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public HttpClient CreateClient(string name)
        {
            return _httpClient;
        }
    }
}
