namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.ListFilters;
    using Kenc.Cloudflare.Core.Payloads;

    public abstract class CloudflareEntityClient<DataClass, ListFilter, UpdatePayload> : CloudflareEntityClient<DataClass, ListFilter>
        where DataClass : class, ICloudflareEntity
        where ListFilter : class, ICloudflareListFilter
        where UpdatePayload : class, ICloudflarePayload
    {
        protected CloudflareEntityClient(HttpClient httpClient, Uri baseUri) : base(httpClient, baseUri)
        {
        }

        public virtual async Task<DataClass> UpdateAsync(string identifier, UpdatePayload payload, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/{identifier}");
            return await PutAsync<DataClass, UpdatePayload>(targetUri, payload, cancellationToken);
        }
    }
}
