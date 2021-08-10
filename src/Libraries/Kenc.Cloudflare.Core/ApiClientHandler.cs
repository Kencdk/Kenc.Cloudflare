namespace Kenc.Cloudflare.Core
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Exceptions;

    /// <summary>
    /// Client handler that throws <see cref="CloudflareException"/> in case of error responses and fixes headers.
    /// </summary>
    public class ApiClientHandler : DelegatingHandler
    {
        public ApiClientHandler() : base()
        {
        }

        internal ApiClientHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content != null)
            {
                // cloudflare does not expect a charset in the content-type header.
                request.Content.Headers.ContentType.CharSet = string.Empty;
            }

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                Debug.WriteLine("Encountered a non-positive http status code {0}", httpResponseMessage.StatusCode);

                CloudflareResult result = await httpResponseMessage.Content.ReadAsAsync<CloudflareResult>();
                if (result.Errors != null && result.Errors.Count > 0)
                {
                    // we got a negative response back.
                    throw new CloudflareException(result.Errors);
                }
            }

            return httpResponseMessage;
        }
    }
}
