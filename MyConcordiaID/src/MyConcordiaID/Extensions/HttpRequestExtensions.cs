using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyConcordiaID.Helper
{
    public static class HttpRequestExtensions
    {

        /// <summary>
        /// http://stackoverflow.com/questions/31617345/what-is-the-asp-net-core-mvc-equivalent-to-request-requesturi
        /// 
        /// ASP.NET Core MVC equivalent to Request.RequestURI
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Uri ToUri(this HttpRequest request)
        {
            var hostComponents = request.Host.ToUriComponent().Split(':');

            var builder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = hostComponents[0],
                Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };

            if (hostComponents.Length == 2)
            {
                builder.Port = Convert.ToInt32(hostComponents[1]);
            }

            return builder.Uri;
        }
    }
}
