namespace Kenc.Cloudflare.Core.Clients.Enums
{
    using System.Runtime.Serialization;

    public enum DNSRecordType
    {
        A, AAAA, CNAME, TXT, SRV, LOC, MX, NS, SPF, CERT, DNSKEY, DS, NAPTR, SMIMEA, SSHFP, TLSA,
        [EnumMember(Value = "URI read only")]
        URIReadOnly
    }
}
