using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace Skidbladnir.Client.Freenom.Dns
{
    internal static class HtmlParser
    {
        private static readonly Regex RecordIdRegex = new Regex(@"\[(\d*)\]", RegexOptions.Compiled);

        public static string GetSecurityToken(string html)
        {
            if(string.IsNullOrEmpty(html))
                return null;
            var htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(html);
            return htmlSnippet.DocumentNode.SelectNodes("//input[@type=\"hidden\" and @name=\"token\"]")
                .FirstOrDefault()?.GetAttributeValue("value", null);
        }

        public static bool HasLoginSection(string html)
        {
            var htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(html);
            return htmlSnippet.DocumentNode.SelectNodes("//section[@class=\"login\"]") != null;
        }

        public static IEnumerable<Zone> GetZones(string html)
        {
            var listZones = new List<Zone>();
            if(string.IsNullOrEmpty(html))
                return listZones;
            var htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(html);
            var tableRows = htmlSnippet.DocumentNode.SelectNodes("//table//tbody//tr");
            if(tableRows == null)
                return listZones;
            foreach (var tableRow in tableRows)
            {
                if(tableRow == null)
                    continue;
                var zone = new Zone();
                zone.Name = tableRow.SelectNodes(".//td[@class=\"second\"]//a")?.FirstOrDefault()?.InnerText?.Trim();
                var registrationDateString =
                    tableRow.SelectNodes(".//td[@class=\"third\"]")?.FirstOrDefault()?.InnerText?.Trim();
                zone.RegistrationDate = string.IsNullOrEmpty(registrationDateString)
                    ? DateTime.MinValue
                    : DateTime.Parse(registrationDateString);
                var expireDate = tableRow.SelectNodes(".//td[@class=\"fourth\"]")?.FirstOrDefault()?.InnerText?.Trim();
                zone.ExpireDate = string.IsNullOrEmpty(expireDate)
                    ? DateTime.MinValue
                    : DateTime.Parse(expireDate);
                zone.Status = tableRow.SelectNodes(".//td[@class=\"fifth\"]//span")?.FirstOrDefault()?.InnerText
                    ?.Trim();
                zone.Type = tableRow.SelectNodes(".//td[@class=\"sixth\"]")?.FirstOrDefault()?.InnerText?.Trim();
                var zoneIdHref = tableRow.SelectNodes(".//td[@class=\"seventh\"]//a[@href]")?.FirstOrDefault()
                    ?.GetAttributeValue("href", null);
                if (!string.IsNullOrEmpty(zoneIdHref))
                {
                    var tempHrefUri = new Uri($"http://{zoneIdHref}");
                    var parsedQuery = HttpUtility.ParseQueryString(tempHrefUri.Query);
                    zone.ZoneId = long.Parse(parsedQuery["id"]);
                }

                listZones.Add(zone);
            }

            return listZones;
        }

        public static IEnumerable<DnsRecord> GetRecords(string html)
        {
            var htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(html);
            var tableRows = htmlSnippet.DocumentNode.SelectNodes("//form[@id=\"recordslistform\"]//table//tbody//tr");
            var list = new List<DnsRecord>();

            if (tableRows == null)
                return list;

            foreach (var tableRow in tableRows)
            {
                var nameField = tableRow.SelectNodes(".//td[@class=\"name_column\"]//input[@type=\"text\"]")
                    .FirstOrDefault();
                if (nameField == null) continue;
                var idMatch = RecordIdRegex.Match(nameField?.GetAttributeValue("name", "") ?? "");
                if (!idMatch.Success || idMatch.Groups.Count < 2) continue;
                var id = int.Parse(idMatch.Groups[1].Value);
                var name = nameField.GetAttributeValue("value", null);
                var type = (DnsRecordType) Enum.Parse(typeof(DnsRecordType),
                    tableRow.SelectNodes(".//td[@class=\"type_column\"]//strong")?.FirstOrDefault()?.InnerText
                        ?.Trim() ?? "", true);
                list.Add(new DnsRecord(id, name, type)
                {
                    Ttl = int.Parse(tableRow.SelectNodes(".//td[@class=\"ttl_column\"]//input")
                                        ?.FirstOrDefault()
                                        ?.GetAttributeValue("value", "0")
                                        ?.Trim() ?? "0"),
                    Value = tableRow
                        .SelectNodes($".//td[@class=\"value_column\"]//input[@name=\"records[{id}][value]\"]")
                        ?.FirstOrDefault()
                        ?.GetAttributeValue("value", null)
                        ?.Trim(),
                    Priority = int.Parse(tableRow.SelectNodes(".//td[@class=\"value_column\"]//input[@name=\"records[{id}][priority]\"]")
                                             ?.FirstOrDefault()
                                             ?.GetAttributeValue("value", "0")
                                             ?.Trim() ?? "0")
                });
            }

            return list;
        }

        private static void ClearParent(HtmlNode node)
        {
            node.SetParent(node);
        }
    }
}