namespace Kenc.Cloudflare.Core.Entities.AccountMembers
{
    using System.Runtime.Serialization;

    public enum AccountMemberStatus
    {
        [EnumMember(Value = "accepted")]
        Accepted,
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "rejected")]
        Rejected
    }
}
