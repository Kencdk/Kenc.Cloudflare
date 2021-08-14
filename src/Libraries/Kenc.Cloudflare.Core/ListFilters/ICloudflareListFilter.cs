namespace Kenc.Cloudflare.Core.ListFilters
{
    using System.Collections.Generic;

    public interface ICloudflareListFilter
    {
        IReadOnlyList<string> GenerateParameters();
    }
}
