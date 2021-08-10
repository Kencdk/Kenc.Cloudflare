# Kenc.Cloudflare
![Build Status](https://dev.azure.com/kenc/Cloudflare/_apis/build/status/Kencdk.Kenc.Cloudflare?branchName=main)
[![Feature Requests](https://img.shields.io/github/issues/Kencdk/Kenc.Cloudflare/feature-request.svg)](https://github.com/Kencdk/Kenc.Cloudflare/issues?q=is%3Aopen+is%3Aissue+label%3Afeature-request+sort%3Areactions-%2B1-desc)
[![Bugs](https://img.shields.io/github/issues/Kencdk/Kenc.Cloudflare/bug.svg)](https://github.com/Kencdk/Kenc.Cloudflare/issues?utf8=✓&q=is%3Aissue+is%3Aopen+label%3Abug)
[![NuGet](https://img.shields.io/nuget/v/Kenc.Cloudflare.svg)](NuGet)

Free and open implementation of the Cloudflare API prtocol v4 in C# .net core, allowing cross-platform automated interaction with Cloudflare.

# Getting Started #
Install the [Nuget] package.
Register with dependency injection

````C#
serviceCollection.AddHttpClient();
serviceCollection.AddCloudflareClient(configuration);
````

````C#
private readonly ICloudflareClient cloudflareClient;
public SomeConstructor(ICloudflareClient cloudflareClient) {
  this.cloudflareClient = cloudflareClient ?? throw new ArgumentNullException(nameof(cloudflareClient));
}

public void DoSomething() {
  var zone = await cloudflareClient.Zones.GetAsync("kenc.dk");
}
````

_Not using dependency injection?_ See [IntegrationTests.cs](src\Libraries\Kenc.Cloudflare.Core.IntegrationTests\IntegrationTests.cs) for an example on how to create a client.

Get a specific Zone
````C#
var zone = await cloudflareClient.Zones.GetAsync("kenc.dk");
````
Add a TXT record to the zone
````C#
DNSRecord dnsRecord;
try
{
  dnsRecord = await cloudflareClient.ZoneDNSSettingsClient.CreateRecordAsync(zone.Id, "_dummyrecord", DNSRecordType.TXT, "TXT record entry value", 3600);
  return dnsRecord.Id;
}
catch (CloudflareException exception) when (exception.Errors[0].Code == "81057")
{
    Program.LogLine("The DNS entry already exists.");
    return string.Empty;
}
````

## Feedback

* Request a new feature using [GitHub Issues][].
* File a bug in [GitHub Issues][].
* [Tweet](https://twitter.com/kenmandk) with any other feedback.

## Related Projects

[Kenc.AcmeLib](https://github.com/kencdk/kenc.acmelib) Library for interacting with ACME-based certificate providers, incl. LetsEncrypt.

## License

Licensed under the [MIT](LICENSE) License.

[GitHub Issues]: https://github.com/Kencdk/Kenc.Cloudflare/issues
[NuGet]: https://www.nuget.org/packages/Kenc.Cloudflare/ "Kenc.ACMELib NuGet package"