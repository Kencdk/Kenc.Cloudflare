namespace Kenc.Cloudflare.Core
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Exceptions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    /// <summary>
    /// Client handler that throws <see cref="CloudflareException"/> in case of error responses and fixes headers.
    /// </summary>
    public class ApiClientHandler : DelegatingHandler
    {
        private readonly ILogger<ApiClientHandler> _logger;

        public ApiClientHandler() : this(NullLogger<ApiClientHandler>.Instance)
        {
        }

        public ApiClientHandler(ILogger<ApiClientHandler> logger) : base()
        {
            this._logger = logger ?? NullLogger<ApiClientHandler>.Instance;
        }

        internal ApiClientHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
            _logger = NullLogger<ApiClientHandler>.Instance;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // cloudflare does not expect a charset in the content-type header.
            if (request.Content?.Headers.ContentType != null)
            {
                request.Content.Headers.ContentType.CharSet = string.Empty;
            }

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogWarning("Encountered a non-positive http status code {StatusCode}", httpResponseMessage.StatusCode);

                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                CloudflareResult? result = null;
                try
                {
                    result = JsonSerializer.Deserialize<CloudflareResult>(content);
                }
                catch (JsonException)
                {
                    // response body wasn't the expected Cloudflare error envelope; fall through to the synthetic error below.
                }

                if (result?.Errors is { Count: > 0 })
                {
                    throw new CloudflareException(result.Errors);
                }

                // Non-success response with no parseable Cloudflare error payload.
                var errors = new List<CloudflareApiError>
                {
                    new(((int)httpResponseMessage.StatusCode).ToString(), string.IsNullOrWhiteSpace(content) ? httpResponseMessage.ReasonPhrase ?? httpResponseMessage.StatusCode.ToString() : content)
                };

                throw new CloudflareException(errors);
            }

            return httpResponseMessage;
        }
    }
}
