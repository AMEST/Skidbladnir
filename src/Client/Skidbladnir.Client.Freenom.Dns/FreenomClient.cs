using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skidbladnir.Client.Freenom.Dns
{
    /// <inheritdoc />
    internal class FreenomClient : IFreenomClient
    {
        private readonly HttpClient _client;

        public FreenomClient()
        {
            _client = CookieHttpClientFactory.Create();
        }

        /// <inheritdoc />
        public async Task SignIn(string email, string password)
        {
            using var signInPage = await _client.GetAsync(FreenomUrls.ClientArea);
            var signInPageHtml = await signInPage.Content.ReadAsStringAsync();
            var securityToken = HtmlParser.GetSecurityToken(signInPageHtml);

            using var postContent = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"username", email},
                {"password", password},
                {"token", securityToken}
            });
            _client.DefaultRequestHeaders.Add("Referer", FreenomUrls.ClientArea);
            using var request = await _client.PostAsync(new Uri(FreenomUrls.SignIn), postContent);
            request.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task<bool> IsAuthenticated()
        {
            using var clientAreaPage = await _client.GetAsync(FreenomUrls.ClientArea);
            var clientAreaHtml = await clientAreaPage.Content.ReadAsStringAsync();
            return !HtmlParser.HasLoginSection(clientAreaHtml);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Zone>> GetZones()
        {
            using var page = await _client.GetAsync(FreenomUrls.ZonesList);
            var pageHtml = await page.Content.ReadAsStringAsync();

            return HtmlParser.GetZones(pageHtml);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DnsRecord>> GetDnsRecords(Zone zone)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));
            var dnsPageUrl = string.Format(FreenomUrls.DnsManage, zone.Name, zone.ZoneId);
            using var page = await _client.GetAsync(dnsPageUrl);
            var pageHtml = await page.Content.ReadAsStringAsync();
            return HtmlParser.GetRecords(pageHtml);
        }

        /// <inheritdoc />
        public async Task RemoveDnsRecord(Zone zone, DnsRecord record)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            using var getConect = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"managedns", zone.Name},
                {"records", record.Type.ToString().ToUpper()},
                {"dnsaction", "delete"},
                {"name", record.Name},
                {"value", record.Value},
                {"line", ""},
                {"ttl", $"{record.Ttl}"},
                {"priority", $"{record.Priority}"},
                {"weight", ""},
                {"port", ""},
                {"domainid", $"{zone.ZoneId}"}
            });
            var query = await getConect.ReadAsStringAsync();
            var request = await _client.GetAsync(new Uri($"{FreenomUrls.ClientArea}?{query}"));
            request.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task AddDnsRecord(Zone zone, DnsRecord record)
        {
            if (zone == null)
                throw new ArgumentNullException(nameof(zone));
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            using var page = await _client.GetAsync(FreenomUrls.ZonesList);
            var pageHtml = await page.Content.ReadAsStringAsync();

            var securityToken = HtmlParser.GetSecurityToken(pageHtml);
            using var postConect = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"dnsaction", "add"},
                {"token", securityToken},
                {"addrecord[0][name]", record.Name},
                {"addrecord[0][type]", record.Type.ToString().ToUpper()},
                {"addrecord[0][ttl]", $"{record.Ttl}"},
                {"addrecord[0][value]", record.Value},
                {"addrecord[0][priority]", record.Type == DnsRecordType.MX ? $"{record.Priority}" : ""},
                {"addrecord[0][port]", ""},
                {"addrecord[0][weight]", ""},
                {"addrecord[0][forward_type]", "1"},
            });
            var requestUrl = string.Format(FreenomUrls.DnsManage, zone.Name, zone.ZoneId);
            var request = await _client.PostAsync(requestUrl, postConect);
            request.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}