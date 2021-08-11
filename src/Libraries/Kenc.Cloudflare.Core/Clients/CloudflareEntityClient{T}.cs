namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    public abstract class CloudflareEntityClient<T> : CloudflareEntityClient where T : class, ICloudflareEntity
    {
        protected abstract string EntityNamePlural { get; }

        protected readonly Uri baseUri;

        protected CloudflareEntityClient(HttpClient httpClient, Uri baseUri) : base(httpClient)
        {
            this.baseUri = baseUri;
        }

        public virtual async Task<IdResult> DeleteAsync(string identifier, CancellationToken cancellationToken = default)
        {

            var targetUri = new Uri(baseUri, $"/{EntityNamePlural}/{identifier}");
            return await DeleteAsync<IdResult>(targetUri, cancellationToken);
        }
    }
}
