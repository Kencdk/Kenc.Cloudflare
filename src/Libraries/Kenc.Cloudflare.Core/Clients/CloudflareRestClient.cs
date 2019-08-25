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

    public class CloudflareRestClient : IRestClient
    {
        private const string AuthHeaderKey = "X-Auth-Key";
        private const string AuthHeaderUsername = "X-Auth-Email";
        private const string ApplicationJsonMime = "application/json";

        private readonly string apiKey;
        private readonly string username;
        private readonly string UserAgent;

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly JsonMediaTypeFormatter jsonMediaTypeFormatter;

        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ACMERestClient"/> class.
        /// </summary>
        /// <param name="restClientFactory">Factory to create a rest client.</param>
        /// <param name="username">Cloudflare API username.</param>
        /// <param name="apiKey">Cloudflare API key.</param>
        public CloudflareRestClient(IHttpClientFactory httpClientFactory, string username, string apiKey)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            this.apiKey = apiKey;
            this.username = username;
            this.httpClientFactory = httpClientFactory;

            jsonMediaTypeFormatter = new JsonMediaTypeFormatter()
            {
                SerializerSettings = JsonSettings
            };

            var client = typeof(CloudflareRestClient);
            var runtimeVersion = client.Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            UserAgent = $"{client.FullName}/{runtimeVersion.Version.ToString()} ({RuntimeInformation.OSDescription} {RuntimeInformation.ProcessArchitecture})";
        }

        /// <summary>
        /// Sends a GET request to the <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of expected data to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="TResult"/></returns>
        /// <exception cref="CloudflareException"></exception>
        public async Task<TResult> GetAsync<TResult>(Uri uri, CancellationToken cancellationToken = default(CancellationToken)) where TResult : ICloudflareEntity
        {
            using (var client = GetClient())
            {
                var response = await client.GetAsync(uri, cancellationToken);
                return (await HandleResponse<TResult>(response)).Result;
            }
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
        public async Task<TResult> PatchAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default(CancellationToken)) where TResult : ICloudflareEntity
        {
            using (var client = GetClient())
            {
                var objectContent = new ObjectContent<TMessage>(message, jsonMediaTypeFormatter);
                var response = await client.PatchAsync(uri, objectContent, cancellationToken);

                return (await HandleResponse<TResult>(response)).Result;
            }
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
        public async Task<TResult> PostAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default(CancellationToken)) where TResult : ICloudflareEntity
        {
            using (var client = GetClient())
            {
                var response = await client.PostAsync(uri, message, jsonMediaTypeFormatter, cancellationToken);
                return (await HandleResponse<TResult>(response)).Result;
            }
        }

        /// <summary>
        /// Sends a DELETE request to the <paramref name="uri"/>
        /// </summary>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An async Task.</returns>
        /// <exception cref="CloudflareException"></exception>
        public async Task DeleteAsync(Uri uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var client = GetClient())
            {
                await client.DeleteAsync(uri, cancellationToken);
            }
        }

        public async Task<TResult> DeleteAsync<TResult>(Uri uri, CancellationToken cancellationToken = default(CancellationToken)) where TResult : ICloudflareEntity
        {
            using (var client = GetClient())
            {
                var response = await client.DeleteAsync(uri, cancellationToken);
                return (await HandleResponse<TResult>(response))
                        .Result;
            }
        }

        public async Task<TResult> PutAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : ICloudflareEntity
        {
            using (var client = GetClient())
            {
                var response = await client.PutAsync(uri, new StringContent(string.Empty));
                return (await HandleResponse<TResult>(response)).Result;
            }
        }

        private async Task<CloudflareResult<TResult>> HandleResponse<TResult>(HttpResponseMessage httpResponseMessage) where TResult : ICloudflareEntity
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                Debug.WriteLine("Encountered a non-positive http status code {0}", httpResponseMessage.StatusCode);
            }

            var result = await httpResponseMessage.Content.ReadAsAsync<CloudflareResult<TResult>>();
            if (result.Errors != null && result.Errors.Count > 0)
            {
                // we got a negative response back.
                throw new CloudflareException(result.Errors);
            }

            return result;
        }

        private HttpClient GetClient()
        {
            var client = httpClientFactory.CreateClient("Cloudflare");
            client.DefaultRequestHeaders.Add(AuthHeaderKey, apiKey);
            client.DefaultRequestHeaders.Add(AuthHeaderUsername, username);
            client.DefaultRequestHeaders.Add(HttpRequestHeader.UserAgent.ToString(), UserAgent);
            client.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), ApplicationJsonMime);

            return client;
        }
    }
}