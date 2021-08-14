namespace Kenc.Cloudflare.Core.Entities.DnsRecords
{
    using System.Collections.Generic;
    using System.Linq;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Helpers;
    using Kenc.Cloudflare.Core.ListFilters;

    public class DnsRecordListFilter : CloudflareListFilter
    {
        public DnsRecordType? Type { get; set; }

        public string? Name { get; set; }

        public string? Content { get; set; }

        public bool? Proxied { get; set; }

        public Match? Match { get; set; }

        public string? Order { get; set; }

        public override IReadOnlyList<string> GenerateParameters()
        {
            var parameters = new List<string>();
            if (Type.HasValue)
            {
                parameters.Add($"type={Type.ConvertToString()}");
            }

            if (!string.IsNullOrEmpty(Name))
            {
                parameters.Add($"name={Name}");
            }

            if (!string.IsNullOrEmpty(Content))
            {
                parameters.Add($"content={Content}");
            }

            if (Proxied.HasValue)
            {
                parameters.Add($"proxied={Proxied.Value}");
            }

            if (Match.HasValue)
            {
                parameters.Add($"match={Match.ConvertToString()}");
            }

            if (!string.IsNullOrEmpty(Order))
            {
                parameters.Add($"order={Order}");
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
