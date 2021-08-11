namespace Kenc.Cloudflare.Core.Clients.Enums
{
    using System.Runtime.Serialization;

    public enum MembershipStatus
    {
        [EnumMember(Value = "accepted")]
        Accepted,
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "rejected")]
        Rejected
    }
}
