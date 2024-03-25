using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Text.RegularExpressions;


namespace ObisoftNet.Http
{
    public class RouteResult
    {
        public string Url { get; private set; }
        public string Host { get; private set; }
        public string Route { get; private set; }
        public Dictionary<string, string> Args { get; private set; }


        public static RouteResult ResultFrom(HttpListenerRequest request)
        {
            RouteResult result = new RouteResult();
            result.Url = request.Url.ToString();
            result.Host = request.Url.Host;
            result.Route = request.Url.AbsolutePath;
            result.Args = new Dictionary<string, string>();
            foreach(var data in ParseQuery(request.Url))
            {
                result.Args.Add(data.Key, Uri.UnescapeDataString(data.Value));
            }
            return result;
        }
        private static Dictionary<string, string> ParseQuery(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (uri.Query.Length == 0)
                return new Dictionary<string, string>();

            return uri.Query.TrimStart('?')
                            .Split(new[] { '&', ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(parameter => parameter.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                            .GroupBy(parts => parts[0],
                                     parts => parts.Length > 2 ? string.Join("=", parts, 1, parts.Length - 1) : (parts.Length > 1 ? parts[1] : ""))
                            .ToDictionary(grouping => grouping.Key,
                                          grouping => string.Join(",", grouping));
        }
    }
}
