namespace Kenc.Cloudflare.Core.Tests
{
    using System.Net.Http;

    class MyHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return HttpClientFactory.Create();
        }
    }
}
