using System;

namespace Skidbladnir.Client.Freenom.Dns
{
    /// <summary>
    /// Registered domain in account
    /// </summary>
    public class Zone
    {
        /// <summary>
        /// Zone identifier
        /// </summary>
        public long ZoneId { get; set; }

        /// <summary>
        /// Zone name (domain name eg example.com)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Registration Date of domain
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Zone (domain) expiration date
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// Zone (domain) status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Zone (domain) type (Free or Paid)
        /// </summary>
        public string Type { get; set; }
    }
}