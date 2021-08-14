﻿namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;
    using Kenc.Cloudflare.Core.ListFilters;
    using Kenc.Cloudflare.Core.Payloads;

    public abstract class CloudflareEntityClient<DataClass, ListFilter, UpdatePayload, CreatePayload> : CloudflareEntityClient<DataClass, ListFilter, UpdatePayload>
        where DataClass : class, ICloudflareEntity
        where ListFilter : class, ICloudflareListFilter
        where UpdatePayload : class, ICloudflarePayload
        where CreatePayload : class, ICloudflarePayload
    {
        protected CloudflareEntityClient(HttpClient httpClient, Uri baseUri) : base(httpClient, baseUri)
        {
        }

        public virtual async Task<DataClass> CreateAsync(CreatePayload payload, CancellationToken cancellationToken = default)
        {
            var targetUri = new Uri(baseUri, $"{EntityNamePlural}");
            return await PutAsync<DataClass, CreatePayload>(targetUri, payload, cancellationToken);
        }
    }
}
