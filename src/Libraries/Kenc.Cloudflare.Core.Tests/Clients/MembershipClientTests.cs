namespace Kenc.Cloudflare.Core.Tests.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Kenc.Cloudflare.Core.Clients.EntityClients;
    using Kenc.Cloudflare.Core.Entities.Memberships;
    using Kenc.Cloudflare.Core.Tests.Helpers;
    using Kenc.Cloudflare.Core.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MembershipClientTests
    {
        [TestMethod]
        public async Task GetMembershipHitsExpectedEndpoint()
        {
            var membershipId = "01a7362d577a6c3019a474fd6f485823";

            var membership = new Membership { };
            HttpResponseMessage responseMessage = HttpResponseMessageHelper.CreateApiResponse(membership);
            var mesageHandler = new FakeHttpMessageHandler(responseMessage, new Uri(Global.BaseUri, $"memberships/{membershipId}"));
            var httpClient = new HttpClient(mesageHandler);

            var client = new MembershipsClient(httpClient, Global.BaseUri);
            _ = await client.GetAsync(membershipId);
        }

        [DataTestMethod]
        [DynamicData(nameof(MembershipListFilterGeneratesExpectedParameters_Data), DynamicDataSourceType.Method)]
        public void MembershipListFilterGeneratesExpectedParameters(MembershipsListFilter filter, string expected)
        {
            var parameters = string.Join('&', filter.GenerateParameters());
            parameters.Should().Be(expected);
        }

        public static IEnumerable<object[]> MembershipListFilterGeneratesExpectedParameters_Data()
        {
            yield return new object[] { new MembershipsListFilter(), string.Empty };
            yield return new object[] { new MembershipsListFilter { AccountName = "demoaccount" }, "account.name=demoaccount" };
        }
    }
}
