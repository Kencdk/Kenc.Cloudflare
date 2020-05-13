namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Exceptions;
    using Newtonsoft.Json;

    public class CloudflareRestClient : IRestClient, IDisposable
    {
        private bool disposed = false;

        private const string AuthHeaderKey = "X-Auth-Key";
        private const string AuthHeaderUsername = "X-Auth-Email";
        private const string ApplicationJsonMime = "application/json";

        private readonly string UserAgent;

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly JsonMediaTypeFormatter jsonMediaTypeFormatter = new JsonMediaTypeFormatter { SerializerSettings = JsonSettings };

        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ACMERestClient"/> class.
        /// </summary>
        /// <param name="restClientFactory">Factory to create a rest client.</param>
        /// <param name="username">Cloudflare API username.</param>
        /// <param name="apiKey">Cloudflare API key.</param>
        public CloudflareRestClient(IHttpClientFactory httpClientFactory, string username, string apiKey)
        {
            _ = string.IsNullOrEmpty(apiKey) ? throw new ArgumentNullException(nameof(apiKey)) : apiKey;
            _ = string.IsNullOrEmpty(username) ? throw new ArgumentNullException(nameof(username)) : username;
            _ = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

            Type client = typeof(CloudflareRestClient);
            AssemblyFileVersionAttribute runtimeVersion = client.Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            UserAgent = $"{client.FullName}/{runtimeVersion.Version} ({RuntimeInformation.OSDescription} {RuntimeInformation.ProcessArchitecture})";

            httpClient = httpClientFactory.CreateClient("Cloudflare");
            httpClient.DefaultRequestHeaders.Add(AuthHeaderKey, apiKey);
            httpClient.DefaultRequestHeaders.Add(AuthHeaderUsername, username);
            httpClient.DefaultRequestHeaders.Add(HttpRequestHeader.UserAgent.ToString(), UserAgent);
            httpClient.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), ApplicationJsonMime);
        }

        /// <summary>
        /// Sends a GET request to the <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of expected data to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="TResult"/></returns>
        /// <exception cref="CloudflareException"></exception>
        public async Task<TResult> GetAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri, cancellationToken)
                .ConfigureAwait(false);

            return (await HandleResponse<TResult>(response).ConfigureAwait(false)).Result;
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
        public async Task<TResult> PatchAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            var objectContent = new ObjectContent<TMessage>(message, jsonMediaTypeFormatter);
            HttpResponseMessage response = await httpClient.PatchAsync(uri, objectContent, cancellationToken)
                .ConfigureAwait(false);

            return (await HandleResponse<TResult>(response).ConfigureAwait(false)).Result;
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
        public async Task<TResult> PostAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            HttpResponseMessage response = await httpClient.PostAsync(uri, message, jsonMediaTypeFormatter, cancellationToken)
                .ConfigureAwait(false);

            return (await HandleResponse<TResult>(response).ConfigureAwait(false)).Result;
        }

        /// <summary>
        /// Sends a DELETE request to the <paramref name="uri"/>
        /// </summary>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An async Task.</returns>
        /// <exception cref="CloudflareException"></exception>
        public async Task DeleteAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            await httpClient.DeleteAsync(uri, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TResult> DeleteAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(uri, cancellationToken)
                .ConfigureAwait(false);

            return (await HandleResponse<TResult>(response)
                    .ConfigureAwait(false))
                    .Result;
        }

        public async Task<TResult> PutAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity
        {
            HttpResponseMessage response = await httpClient.PutAsync(uri, null, cancellationToken)
                .ConfigureAwait(false);

            return (await HandleResponse<TResult>(response)
                    .ConfigureAwait(false))
                    .Result;
        }

        public async Task<TResult> PutAsync<TMessage, TResult>(Uri uri, TMessage payload, CancellationToken cancellationToken = default) where TResult : class, ICloudflareEntity where TMessage : class, ICloudflareEntity
        {
            HttpResponseMessage response = await httpClient.PutAsync(uri, payload, jsonMediaTypeFormatter, cancellationToken)
                .ConfigureAwait(false);

            return (await HandleResponse<TResult>(response)
                    .ConfigureAwait(false))
                    .Result;
        }

        private async Task<CloudflareResult<TResult>> HandleResponse<TResult>(HttpResponseMessage httpResponseMessage) where TResult : class, ICloudflareEntity
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                Debug.WriteLine("Encountered a non-positive http status code {0}", httpResponseMessage.StatusCode);
            }

            CloudflareResult<TResult> result = await httpResponseMessage.Content.ReadAsAsync<CloudflareResult<TResult>>();
            if (result.Errors != null && result.Errors.Count > 0)
            {
                // we got a negative response back.
                throw new CloudflareException(result.Errors);
            }

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    httpClient.Dispose();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Implementation of the disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}