namespace Kenc.Cloudflare.Core.IntegrationTests
{
    using System.Collections.Generic;
    using Kenc.Cloudflare.Core.Clients;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestCategory("IntegrationTests"), TestClass]
    public abstract class IntegrationTestBase
    {
        public TestContext TestContext { get; set; }

        protected ICloudflareClient CreateClient()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                { "ApiKey", TestContextSetting("cloudflareapikey") },
                { "Username", TestContextSetting("cloudflareusername")},
                { "Endpoint", CloudflareAPIEndpoint.V4Endpoint.ToString() }
            };

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            serviceCollection.AddCloudflareClient(configuration);
            ServiceProvider services = serviceCollection.BuildServiceProvider();
            return services.GetRequiredService<ICloudflareClient>();
        }

        protected string TestContextSetting(string name)
        {
            if (TestContext.Properties.Contains(name))
            {
                return (string)TestContext.Properties[name];
            }

            return System.Environment.GetEnvironmentVariable(name);
        }
    }
}