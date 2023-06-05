using Microsoft.AspNetCore.Http;
using System.Net;

namespace Logic.Extensions
{
    public static class HttpContextExtensions
    {
        public static string DeriveIp(this HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress!.ToString();

            if (ip == "::1") 
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            }

            return ip;
        }
    }
}
