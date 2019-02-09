namespace Kenc.Cloudflare.Core.Clients.Enums
{
    using System.Runtime.Serialization;

    public enum Direction
    {
        [EnumMember(Value = "asc")]
        Asc,
        [EnumMember(Value = "desc")]
        Desc
    }
}
