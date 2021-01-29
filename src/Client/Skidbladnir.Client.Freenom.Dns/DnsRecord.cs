namespace Skidbladnir.Client.Freenom.Dns
{
    /// <summary>
    /// Dns record object
    /// </summary>
    public class DnsRecord
    {
        public DnsRecord(string name, DnsRecordType type = DnsRecordType.A)
        {
            Id = 0;
            Name = name;
            Type = type;
        }

        public DnsRecord(int id, string name, DnsRecordType type = DnsRecordType.A)
        {
            Id = id;
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Dns Record id
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Dns Record name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Dns Record value (target)
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Dns Time to live
        /// </summary>
        public int Ttl { get; set; }

        /// <summary>
        /// Dns Record type eg A,AAAA,TXT,MX
        /// </summary>
        public DnsRecordType Type { get; }
        /// <summary>
        /// Dns Record priority (Only for MX records)
        /// </summary>
        public int Priority { get; set; }
    }
}