namespace Kenc.Cloudflare.Core.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class FakeHttpMessageHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> _results;

        public FakeHttpMessageHandler(HttpResponseMessage responseMessage, Uri uri)
        {
            _results = new Dictionary<Uri, HttpResponseMessage>()
            {
                { uri, responseMessage }
            };
        }

        public FakeHttpMessageHandler(Dictionary<Uri, HttpResponseMessage> results)
        {
            _results = results;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_results.Remove(request.RequestUri, out var responseMessage))
            {
                return Task.FromResult(responseMessage);
            }

            throw new InvalidOperationException($"No response are configured for the requested Uri {request.RequestUri}");
        }
    }
}
