namespace Kenc.Cloudflare.Core.Entities.Memberships
{
    using System.Collections.Generic;
    using System.Linq;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Helpers;
    using Kenc.Cloudflare.Core.ListFilters;

    public class MembershipsListFilter : CloudflareListFilter
    {
        public MembershipStatus? Status { get; set; }

        public string? AccountName { get; set; }

        public override IReadOnlyList<string> GenerateParameters()
        {
            var parameters = new List<string>();
            if (Status.HasValue)
            {
                parameters.Add($"status={Status.ConvertToString()}");
            }
            if (!string.IsNullOrEmpty(AccountName))
            {
                parameters.Add($"account.name={AccountName}");
            }

            IReadOnlyList<string>? baseParameters = base.GenerateParameters();
            if (baseParameters.Any())
            {
                parameters.AddRange(baseParameters);
            }
            return parameters;
        }
    }
}
