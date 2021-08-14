namespace Kenc.Cloudflare.Core.ListFilters
{
    using System.Collections.Generic;
    using Kenc.Cloudflare.Core.Clients.Enums;
    using Kenc.Cloudflare.Core.Helpers;

    public class CloudflareListFilter : ICloudflareListFilter
    {
        public int? Page { get; set; }

        public int? PerPage { get; set; }

        public Direction? Direction { get; set; }

        public virtual IReadOnlyList<string> GenerateParameters()
        {
            var list = new List<string>();
            if (Page != null)
            {
                list.Add($"page={Page}");
            }

            if (PerPage != null)
            {
                list.Add($"per_page={PerPage}");
            }

            if (Direction != null)
            {
                list.Add($"direction={Direction.ConvertToString()}");
            }

            return list;
        }
    }
}
