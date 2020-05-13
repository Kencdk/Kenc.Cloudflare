namespace Kenc.Cloudflare.Core.Clients.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Whether to match all search requirements or at least one(any)
    /// </summary>
    public enum Match
    {
        [EnumMember(Value = "all")]
        All,
        [EnumMember(Value = "any")]
        Any
    }
}
