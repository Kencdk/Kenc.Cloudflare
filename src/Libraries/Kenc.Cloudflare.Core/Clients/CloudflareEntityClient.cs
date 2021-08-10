namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    public abstract class CloudflareEntityClient
    {
        private readonly HttpClient httpClient;

        private readonly JsonMediaTypeFormatter jsonMediaTypeFormatter = new() { Indent = true };

        protected CloudflareEntityClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Sends a GET request to the <paramref name="targetUri"/>.
        /// </summary>
        /// <typeparam name="T">Type of expected data to receive.</typeparam>
        /// <param name="targetUri">Endpoint to target.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="T"/></returns>
        /// <exception cref="CloudflareException"></exception>
        protected async Task<T> GetAsync<T>(Uri targetUri, CancellationToken cancellationToken) where T : class, ICloudflareEntity
        {
            var response = await httpClient.GetAsync(targetUri, cancellationToken);
            return (await DeserializeContentAsync<CloudflareResult<T>>(response)).Result;
        }

        /// <summary>
        /// Sends a PATCH request to the <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="TMessage">Type of data to send.</typeparam>
        /// <typeparam name="TResult">Type of expected data to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="message">Object to send of type <typeparamref name="TMessage"/></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="TResult"/></returns>
        /// <exception cref="CloudflareException"></exception>
        protected async Task<TResult> PatchAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            var strMessage = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PatchAsync(uri, strMessage, cancellationToken);

            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        /// <summary>
        /// Sends a POST request to the <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="TMessage">Type of data to send.</typeparam>
        /// <typeparam name="TResult">Type of expected data to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="message">Object to send of type <typeparamref name="TMessage"/></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="TResult"/></returns>
        /// <exception cref="CloudflareException"></exception>
        protected async Task<TResult> PostAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            // workaround for issue where json content is wrapped in extra characters
            var strMessage = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, strMessage, cancellationToken);

            // workaround for issue where deserialization doesn't take into account special naming (such as zone_id)
            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        protected async Task<TResult> DeleteAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(uri, cancellationToken);

            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        protected async Task<TResult> PutAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            HttpResponseMessage response = await httpClient.PutAsync(uri, null, cancellationToken);

            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        protected async Task<TResult> PutAsync<TMessage, TResult>(Uri uri, TMessage payload, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity where TMessage : class, ICloudflareEntity
        {
            HttpResponseMessage response = await httpClient.PutAsync(uri, payload, jsonMediaTypeFormatter, cancellationToken);
            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async Task<T> DeserializeContentAsync<T>(HttpResponseMessage response)
        {
#if DEBUG
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
#else
            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
#endif
        }
    }
}
