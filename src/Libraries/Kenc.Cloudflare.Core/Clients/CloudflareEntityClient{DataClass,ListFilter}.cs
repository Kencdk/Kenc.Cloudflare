namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.ListFilters;

    public abstract class CloudflareEntityClient<DataClass, ListFilter> : CloudflareEntityClient<DataClass>
        where DataClass : class, ICloudflareEntity
        where ListFilter : class, ICloudflareListFilter
    {
        protected CloudflareEntityClient(HttpClient httpClient, Uri baseUri) : base(httpClient, baseUri)
        {
        }

        public virtual async Task<DataClass> GetAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}/{identifier}");
            return await GetAsync<DataClass>(targetUri, cancellationToken);
        }

        public virtual async Task<IReadOnlyList<DataClass>> ListAsync(ListFilter? filter = null, CancellationToken cancellationToken = default)
        {
            IReadOnlyList<string>? parameters;
            var targetUri = new Uri(baseUri,
                filter != null && (parameters = filter.GenerateParameters()).Any() ?
                 $"{EntityNamePlural}?{string.Join('&', parameters)}" : $"{EntityNamePlural}");

            return await GetAsync<IReadOnlyList<DataClass>>(targetUri, cancellationToken);
        }
    }
}
