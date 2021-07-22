namespace Kenc.Cloudflare.Core.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class FakeHttpMessageHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> results;

        public FakeHttpMessageHandler(HttpResponseMessage responseMessage, Uri uri)
        {
            results = new Dictionary<Uri, HttpResponseMessage>()
            {
                { uri, responseMessage }
            };
        }

        public FakeHttpMessageHandler(Dictionary<Uri, HttpResponseMessage> results)
        {
            this.results = results;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (results.Remove(request.RequestUri, out HttpResponseMessage responseMessage))
            {
                return Task.FromResult(responseMessage);
            }

            throw new InvalidOperationException($"No response are configured for the requested Uri {request.RequestUri}");
        }
    }
}
