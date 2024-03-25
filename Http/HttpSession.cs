using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObisoftNet.Http
{


    public class HttpSession {

        private CookieContainer _cookies;


        public bool AllowRedirect { get; set; } = true;
       

        public HttpSession(SecurityProtocolType security = SecurityProtocolType.Tls12)
        {
            ServicePointManager.SecurityProtocol = security;
            if (_cookies == null)
                _cookies = new CookieContainer();
        }
        public void SetHeaders(WebRequest request, Dictionary<string, string> headers = null)
        {
            MethodInfo method = typeof(WebHeaderCollection).GetMethod
                        ("AddWithoutValidate", BindingFlags.Instance | BindingFlags.NonPublic);
            if (headers != null)
                foreach (var h in headers)
                    method.Invoke(request.Headers, new object[] { h.Key, h.Value });
        }
        private HttpWebRequest MakeRequestDefault(string url,Dictionary<string,string> headers = null)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.AllowAutoRedirect = AllowRedirect;
            request.CookieContainer = _cookies;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0";
            if (headers != null)
                SetHeaders(request, headers);
            return request;
        }
        

        public WebResponse Get(string url, Dictionary<string, string> headers = null)
        {
            var request = MakeRequestDefault(url,headers:headers);
            request.Method = "GET";
            return request.GetResponse();
        }
        public string GetString(string url, Dictionary<string, string> headers = null)
        {
            string result = "";
            var request = MakeRequestDefault(url, headers: headers);
            request.Method = "GET";
            return GetStringFromResponse(request.GetResponse());
        }

        public string GetStringFromResponse(WebResponse resp)
        {
            try
            {
                using (var stream = resp.GetResponseStream())
                {
                    using (TextReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch { }
            return "";
        }

        public WebResponse Post(string url,object data=null,Dictionary<string,string> headers=null)
        {
            var request = MakeRequestDefault($"{url}", headers: headers);
            byte[] databytes = new byte[0];
            long datalen = 0;
            if (data != null)
            {
                if (data.GetType() == typeof(Dictionary<string, string>))
                {
                    string postdata = "";
                    foreach (var arg in data as Dictionary<string, string>)
                        if (postdata == "")
                            postdata += $"{arg.Key}={WebUtility.UrlEncode(arg.Value)}";
                        else
                            postdata += $"&{arg.Key}={WebUtility.UrlEncode(arg.Value)}";
                    databytes = Encoding.ASCII.GetBytes(postdata);
                    datalen = databytes.Length;
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                if (data.GetType() == typeof(Stream))
                {
                    Stream dataas = data as Stream;
                    using (BinaryReader reader = new BinaryReader(dataas))
                    {
                        data = reader.ReadBytes((int)dataas.Length);
                        datalen = dataas.Length;
                    }
                }
            }
            request.Method = "POST";
            request.ContentLength = datalen;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(databytes, 0, databytes.Length);
            }
            return request.GetResponse();
        }
        public WebResponse Put(string url, object data = null, Dictionary<string, string> headers = null)
        {
            var request = MakeRequestDefault($"{url}", headers: headers);
            byte[] databytes = new byte[0];
            long datalen = 0;
            if (data != null)
            {
                if (data.GetType() == typeof(Dictionary<string, string>))
                {
                    string postdata = "";
                    foreach (var arg in data as Dictionary<string, string>)
                        if (postdata == "")
                            postdata += $"{arg.Key}={WebUtility.UrlEncode(arg.Value)}";
                        else
                            postdata += $"&{arg.Key}={WebUtility.UrlEncode(arg.Value)}";
                    databytes = Encoding.ASCII.GetBytes(postdata);
                    datalen = databytes.Length;
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                if (data.GetType() == typeof(Stream))
                {
                    Stream dataas = data as Stream;
                    using (BinaryReader reader = new BinaryReader(dataas))
                    {
                        data = reader.ReadBytes((int)dataas.Length);
                        datalen = dataas.Length;
                    }
                }
            }
            request.Method = "PUT";
            request.ContentLength = datalen;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(databytes, 0, databytes.Length);
            }
            return request.GetResponse();
        }

        public WebRequest MakeRequest(string method,string url, Dictionary<string, string> headers = null)
        {
            var request = MakeRequestDefault($"{url}", headers: headers);
            request.Method = method;
            return request;
        }
        public WebRequest MakeGetRequest(string url, Dictionary<string, string> headers = null)
        {
            var request = MakeRequestDefault($"{url}", headers: headers);
            request.Method = "GET";
            return request;
        }
        public WebRequest MakePostRequest(string url, Dictionary<string, string> headers = null)
        {
            var request = MakeRequestDefault($"{url}", headers: headers);
            request.Method = "POST";
            return request;
        }
        public WebRequest MakePutRequest(string url, Dictionary<string, string> headers = null)
        {
            var request = MakeRequestDefault($"{url}", headers: headers);
            request.Method = "PUT";
            return request;
        }
    }
}
