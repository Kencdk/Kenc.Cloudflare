namespace Kenc.Cloudflare.Core.Clients.Enums
{
    using System.Runtime.Serialization;

    public enum ZoneStatus
    {
        [EnumMember(Value = "active")]
        Active,
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "initializing")]
        Initializing,
        [EnumMember(Value = "moved")]
        Moved,
        [EnumMember(Value = "deleted")]
        Deleted,
        [EnumMember(Value = "deactivated")]
        Deactivated,
        [EnumMember(Value = "read-only")]
        ReadOnly
    }
}
