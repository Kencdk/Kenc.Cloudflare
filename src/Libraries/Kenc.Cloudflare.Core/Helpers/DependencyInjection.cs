namespace Kenc.Cloudflare.Core
{
    using Kenc.Cloudflare.Core.Clients;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        /// <summary>
        /// Add <see cref="ICloudflareClient"/> to dependency injection.
        /// </summary>
        /// <param name="serviceCollection">Service collection to add it to</param>
        /// <param name="configuration">Configuration to parse <see cref="CloudflareClientOptions"/> from.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCloudflareClient(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddSingleton<ApiClientHandler>()
                .AddHttpClient("Cloudflare")
                .AddHttpMessageHandler<ApiClientHandler>();

            return serviceCollection.AddSingleton<ICloudflareClient, CloudflareClient>()
                .Configure<CloudflareClientOptions>(configuration);
        }
    }
}
