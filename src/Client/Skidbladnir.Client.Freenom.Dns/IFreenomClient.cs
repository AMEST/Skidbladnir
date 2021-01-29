using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skidbladnir.Client.Freenom.Dns
{
    /// <summary>
    /// Client Abstraction for Freenom dns
    /// </summary>
    public interface IFreenomClient : IDisposable
    {
        /// <summary>
        /// SignIn inside freenom
        /// </summary>
        Task SignIn(string email, string password);

        /// <summary>
        /// Check is client authenticated
        /// </summary>
        Task<bool> IsAuthenticated();

        /// <summary>
        /// Get list zones (domains) registered in account
        /// </summary>
        Task<IEnumerable<Zone>> GetZones();

        /// <summary>
        /// Get dns records from zone
        /// </summary>
        Task<IEnumerable<DnsRecord>> GetDnsRecords(Zone zone);

        /// <summary>
        /// Remove dns record from zone
        /// </summary>
        Task RemoveDnsRecord(Zone zone, DnsRecord record);

        /// <summary>
        /// Add dns record to zone
        /// </summary>
        Task AddDnsRecord(Zone zone, DnsRecord record);
    }
}