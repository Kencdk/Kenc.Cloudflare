namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.JsonConverters;

    public abstract class CloudflareEntityClient
    {
        private const string ApplicationJsonMime = "application/json";

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = {
                new DateTimeConverter(),
                new DateTimeOffsetConverter(),
                new NullableDateTimeConverter(),
                new NullableDateTimeOffsetConverter(),
                new JsonStringEnumConverter()
            }
        };

        protected CloudflareEntityClient(HttpClient httpClient)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Sends a GET request to the <paramref name="targetUri"/>.
        /// </summary>
        /// <typeparam name="T">Type of expected data to receive.</typeparam>
        /// <param name="targetUri">Endpoint to target.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="T"/></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        protected async Task<T> GetAsync<T>(Uri targetUri, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(targetUri, cancellationToken);
            return (await DeserializeContentAsync<CloudflareResult<T>>(response)).Result;
        }

        /// <summary>
        /// Sends a <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <typeparam name="T">Type of expected data to receive.</typeparam>
        /// <param name="httpRequestMessage">Request to send.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="T"/></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        protected async Task<T> SendMessage<T>(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
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
        /// <exception cref="Exceptions.CloudflareException"></exception>
        protected async Task<TResult> PatchAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default)
        {
            var strMessage = SerializeContent(message);
            var response = await _httpClient.PatchAsync(uri, strMessage, cancellationToken);

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
        /// <exception cref="Exceptions.CloudflareException"></exception>
        protected async Task<TResult> PostAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default)
        {
            var strMessage = SerializeContent(message);
            var response = await _httpClient.PostAsync(uri, strMessage, cancellationToken);

            // workaround for issue where deserialization doesn't take into account special naming (such as zone_id)
            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        protected async Task<TResult> DeleteAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            var response = await _httpClient.DeleteAsync(uri, cancellationToken);

            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        protected async Task<TResult> PutAsync<TResult>(Uri uri, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PutAsync(uri, null, cancellationToken);
            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        protected async Task<TResult> PutAsync<TMessage, TResult>(Uri uri, TMessage payload, CancellationToken cancellationToken = default) where TMessage : class, ICloudflareEntity
        {
            var strMessage = SerializeContent(payload);
            var response = await _httpClient.PutAsync(uri, strMessage, cancellationToken);

            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task<T> DeserializeContentAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions)
                ?? throw new JsonException($"Failed to deserialize response body into {typeof(T).Name}.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected StringContent SerializeContent<T>(T message)
        {
            return new StringContent(JsonSerializer.Serialize(message, _jsonSerializerOptions), Encoding.UTF8, ApplicationJsonMime);
        }
    }
}
