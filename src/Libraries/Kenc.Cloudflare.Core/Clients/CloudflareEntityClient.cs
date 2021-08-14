namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.JsonConverters;
    using Kenc.Cloudflare.Core.ListFilters;
    using Kenc.Cloudflare.Core.Payloads;

    public abstract class CloudflareEntityClient
    {
        private const string ApplicationJsonMime = "application/json";

        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
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
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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
            HttpResponseMessage response = await httpClient.GetAsync(targetUri, cancellationToken);
            return (await DeserializeContentAsync<CloudflareResult<T>>(response)).Result;
        }

        protected async Task<IReadOnlyList<T>> ListAsync<T>(Uri targetUri, ICloudflareListFilter filter, CancellationToken cancellationToken)
        {
            IReadOnlyList<string>? parameters;
            var uri = new Uri(targetUri,
                filter != null && (parameters = filter.GenerateParameters()).Any() ?
                 $"?{string.Join('&', parameters)}" : string.Empty);

            return await GetAsync<IReadOnlyList<T>>(uri, cancellationToken);
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
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage, cancellationToken);
            return (await DeserializeContentAsync<CloudflareResult<T>>(response)).Result;
        }

        /// <summary>
        /// Sends a PATCH request to the <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="TPayload">Type of data to send.</typeparam>
        /// <typeparam name="TResult">Type of expected data to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="payload">Object to send of type <typeparamref name="TPayload"/></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="TResult"/></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        protected async Task<TResult> PatchAsync<TResult, TPayload>(Uri uri, TPayload payload, CancellationToken cancellationToken = default)
            where TPayload : class, ICloudflarePayload
        {
            StringContent strMessage = SerializeContent(payload);
            HttpResponseMessage response = await httpClient.PatchAsync(uri, strMessage, cancellationToken);

            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        /// <summary>
        /// Sends a POST request to the <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="TPayload">Type of data to send.</typeparam>
        /// <typeparam name="TResult">Type of expected data to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="payload">Object to send of type <typeparamref name="TPayload"/></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="TResult"/></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        protected async Task<TResult> PostAsync<TResult, TPayload>(Uri uri, TPayload payload, CancellationToken cancellationToken = default)
        {
            StringContent strMessage = SerializeContent(payload);
            HttpResponseMessage response = await httpClient.PostAsync(uri, strMessage, cancellationToken);

            // workaround for issue where deserialization doesn't take into account special naming (such as zone_id)
            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        protected async Task<TResult> DeleteAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(uri, cancellationToken);

            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        protected async Task<TResult> PutAsync<TResult>(Uri uri, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await httpClient.PutAsync(uri, null, cancellationToken);
            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        protected async Task<TResult> PutAsync<TResult, TPayload>(Uri uri, TPayload payload, CancellationToken cancellationToken = default) where TPayload : class, ICloudflarePayload
        {
            StringContent strMessage = SerializeContent(payload);
            HttpResponseMessage response = await httpClient.PutAsync(uri, strMessage, cancellationToken);

            return (await DeserializeContentAsync<CloudflareResult<TResult>>(response)).Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task<T> DeserializeContentAsync<T>(HttpResponseMessage response)
        {
#pragma warning disable CS8603 // Possible null reference return.
#if DEBUG
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
#else
            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), jsonSerializerOptions);
#endif
#pragma warning restore CS8603 // Possible null reference return.
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected StringContent SerializeContent<T>(T message)
        {
#if DEBUG
            var str = JsonSerializer.Serialize(message, typeof(T), jsonSerializerOptions);
            return new StringContent(str, Encoding.UTF8, ApplicationJsonMime);
#else
            // workaround for issue where json content is wrapped in extra characters
            return new StringContent(JsonSerializer.Serialize(message, jsonSerializerOptions), Encoding.UTF8, ApplicationJsonMime);
#endif
        }
    }
}
