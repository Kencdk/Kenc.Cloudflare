namespace Kenc.Cloudflare.Core.Tests.Helpers
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Exceptions;

    public static class HttpResponseMessageHelper
    {
        public static HttpResponseMessage CreateApiResponse<T>(T entity) where T : class, ICloudflareEntity
        {
            var response = new CloudflareResult<T>
            {
                Result = entity
            };

            var serializedContent = JsonSerializer.Serialize(response);
            var content = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            return new HttpResponseMessage
            {
                Content = content,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static HttpResponseMessage CreateErrorResponse(string errorCode, string message)
        {
            var response = new CloudflareResult
            {
                Errors =
                [
                    new CloudflareApiError(errorCode, message)
                ]
            };

            var serializedContent = JsonSerializer.Serialize(response);
            var content = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            return new HttpResponseMessage
            {
                Content = content,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }
}
