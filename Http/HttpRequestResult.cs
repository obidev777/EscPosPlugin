using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObisoftNet.Http
{
    public class HttpRequestResult
    {
        public string Method { get; private set; }
        public string Version { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Url { get; private set; }
        public Dictionary<string,string> Headers { get; private set; }


        public static HttpRequestResult FromText(string text)
        {
            HttpRequestResult result = new HttpRequestResult();
            string[] splitter = FixSplitter(text.Split(new char[] { '\r', '\n' }));
            if (splitter.Length > 0)
            {
                string[] datasplit = splitter[0].Split(' ');
                if (datasplit.Length > 0)
                    result.Method = datasplit[0];
                if (datasplit.Length > 1)
                {
                    string host = datasplit[1];
                    string[] hostsplitter = host.Split(':');
                    if(hostsplitter.Length>0)
                        result.Host = hostsplitter[0];
                    if (host.Contains("http") || host.Contains("https"))
                    {
                        result.Host = host;
                        if (host[host.Length-1]=='/')
                        result.Host = result.Host.Remove(host.Length - 1);
                        result.Host = result.Host.Replace("http://", "").Replace("https://", "").Split('/')[0];
                    }
                    if (hostsplitter.Length > 1)
                        try
                        {
                            result.Port = int.Parse(hostsplitter[1]);
                        }
                        catch { result.Port = 80; }
                    if (hostsplitter.Length > 0)
                        result.Url = datasplit[1];
                }
                if (datasplit.Length > 2)
                    result.Version = datasplit[2];
                result.Headers = new Dictionary<string, string>();
                for(int i = 1; i < splitter.Length; i++)
                {
                    string header = splitter[i];
                    string[] headersplitter = header.Replace(": ",":").Split(new char[] {':' },2);
                    if (headersplitter.Length > 1)
                        result.Headers.Add(headersplitter[0], headersplitter[1]);
                }
                return result;
            }
            return null;
        }
        private static string[] FixSplitter(string[] splitter)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < splitter.Length; i++)
            {
                string item = splitter[i];
                if (item != "")
                    result.Add(item);
            }
            return result.ToArray();
        }
    }

}
