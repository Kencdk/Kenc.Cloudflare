namespace Kenc.Cloudflare.Core.Clients.Enums
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DnsRecordType
    {
        A, AAAA, CNAME, TXT, SRV, LOC, MX, NS, SPF, CERT, DNSKEY, DS, NAPTR, SMIMEA, SSHFP, TLSA,
        [EnumMember(Value = "URI read only")]
        URIReadOnly
    }
}
