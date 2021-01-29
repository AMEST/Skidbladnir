namespace Skidbladnir.Client.Freenom.Dns
{
    internal static class FreenomUrls
    {
        public static string SignIn = "https://my.freenom.com/dologin.php";
        public static string ClientArea = "https://my.freenom.com/clientarea.php";
        public static string ZonesList = "https://my.freenom.com/clientarea.php?action=domains&itemlimit=all";
        public static string DnsManage = "https://my.freenom.com/clientarea.php?managedns={0}&domainid={1}";
    }
}