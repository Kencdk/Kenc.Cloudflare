namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.Payloads;

    public abstract class CloudflareEntityClient<DataClass, UpdatePayload> : CloudflareEntityClient
        where DataClass : class, ICloudflareEntity
        where UpdatePayload : class, ICloudflarePayload
    {
        protected abstract string EntityNamePlural { get; }

        protected readonly Uri baseUri;

        protected CloudflareEntityClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            this.baseUri = baseUri;
        }

        public virtual async Task<DataClass> GetAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"/{EntityNamePlural}/{identifier}");
            return await GetAsync<DataClass>(targetUri, cancellationToken);
        }

        public virtual async Task<DataClass> UpdateAsync(string identifier, UpdatePayload payload, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"/{EntityNamePlural}/{identifier}");
            return await PutAsync<UpdatePayload, DataClass>(targetUri, payload, cancellationToken);
        }

        public virtual async Task<IdResult> DeleteAsync(string identifier, CancellationToken cancellationToken = default)
        {

            var targetUri = new Uri(baseUri, $"/{EntityNamePlural}/{identifier}");
            return await DeleteAsync<IdResult>(targetUri, cancellationToken);
        }
    }
}
