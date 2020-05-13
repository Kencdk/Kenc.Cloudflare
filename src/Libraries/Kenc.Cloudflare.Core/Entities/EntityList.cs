namespace Kenc.Cloudflare.Core.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// A collection of entities of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of items.</typeparam>
    public class EntityList<T> : List<T>, ICloudflareEntity where T : ICloudflareEntity
    {
        // todo Kencdk: consider changing to IReadOnlyList.
    }
}
