using System.Net;
using System.Net.Http;

namespace Skidbladnir.Client.Freenom.Dns
{
    internal static class CookieHttpClientFactory
    {
        public static HttpClient Create()
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() {CookieContainer = cookieContainer, UseCookies = true};
            return new HttpClient(handler);
        }
    }
}