namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Kenc.Cloudflare.Core.Clients.EntityClients;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Implementation of <see cref="ICloudflareClient"/> for interacting with the Cloudflare API.
    /// </summary>
    /// <inheritdoc/>
    public class CloudflareClient : ICloudflareClient
    {
        private const string AuthHeaderKey = "X-Auth-Key";
        private const string AuthHeaderUsername = "X-Auth-Email";
        private const string ApplicationJsonMime = "application/json";

        private readonly HttpClient _httpClient;

        public ZoneClient Zones { get; private set; }

        public UserClient UserClient { get; private set; }

        public ZoneDNSSettingsClient ZoneDNSSettingsClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudflareClient"/> class.
        /// </summary>
        /// <param name="restClientFactory">RestClient factory to use.</param>
        /// <param name="username">Cloudflare Username</param>
        /// <param name="apiKey">Cloudflare APIKey</param>
        /// <param name="endpoint">Cloudflare Endpoint. <see cref="CloudflareAPIEndpoint"/></param>
        /// <exception cref="ArgumentNullException">Throws when any of the parameters are null or <see cref="string.Empty"/></exception>
        public CloudflareClient(IHttpClientFactory httpClientFactory, IOptions<CloudflareClientOptions> options)
        {
            var cloudflareOptions = options.Value ?? throw new ArgumentNullException($"{nameof(options)}.{nameof(options.Value)}");
            if (cloudflareOptions.Endpoint == null)
            {
                throw new ArgumentNullException($"{nameof(options)}.{nameof(options.Value)}.{nameof(options.Value.Endpoint)}");
            }
            _ = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

            _httpClient = httpClientFactory.CreateClient("Cloudflare");
            var client = typeof(CloudflareClient);
            var runtimeVersion = client.Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
            var userAgent = $"{client.FullName}/{runtimeVersion?.Version} ({RuntimeInformation.OSDescription} {RuntimeInformation.ProcessArchitecture})";

            // are we using ApiKey or UserToken?
            if (!string.IsNullOrEmpty(cloudflareOptions.UserToken))
            {
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {cloudflareOptions.UserToken}");
            }
            else
            {
                if (string.IsNullOrEmpty(cloudflareOptions.ApiKey))
                {
                    // must specify either a user token or an API key.
                    throw new ArgumentNullException($"{nameof(options)}.{nameof(options.Value)}.{nameof(cloudflareOptions.ApiKey)}");
                }
                else if (string.IsNullOrEmpty(cloudflareOptions.Username))
                {
                    throw new ArgumentNullException($"{nameof(options)}.{nameof(options.Value)}.{nameof(cloudflareOptions.Username)}");
                }

                _httpClient.DefaultRequestHeaders.Add(AuthHeaderKey, cloudflareOptions.ApiKey);
                _httpClient.DefaultRequestHeaders.Add(AuthHeaderUsername, cloudflareOptions.Username);
            }

            _httpClient.DefaultRequestHeaders.Add(HttpRequestHeader.UserAgent.ToString(), userAgent);
            _httpClient.DefaultRequestHeaders.Add(HttpRequestHeader.ContentType.ToString(), ApplicationJsonMime);

            Zones = new ZoneClient(_httpClient, cloudflareOptions.Endpoint);
            UserClient = new UserClient(_httpClient, cloudflareOptions.Endpoint);
            ZoneDNSSettingsClient = new ZoneDNSSettingsClient(_httpClient, cloudflareOptions.Endpoint);
        }
    }
}
