
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ObisoftNet.Http
{
    public static class HttpExtensions
    {

        public static HttpWebResponse GetResponseHttp(this WebRequest resp) => resp.GetResponse() as HttpWebResponse;
        public static string GetString(this WebResponse resp)
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
        public static byte[] GetBytes(this WebResponse resp)
        {
            List<byte> bytes = new List<byte>();
            try
            {
                using (Stream captchastream = resp.GetResponseStream())
                {
                    byte[] buff = new byte[1024];
                    int read = 0;
                    while ((read = captchastream.Read(buff, 0, buff.Length)) != 0)
                    {
                        Array.Resize(ref buff, read);
                        bytes.AddRange(buff);
                    }
                }
            }
            catch { }
            return bytes.ToArray();
        }
        public static Bitmap GetBitmap(this WebResponse resp)
        {
            Bitmap bmp = null;
            try
            {
                using (Stream stream = resp.GetResponseStream())
                {
                    using (Stream memstream = new MemoryStream())
                    {
                        byte[] buff = new byte[1024];
                        int read = 0;
                        while ((read = stream.Read(buff, 0, buff.Length)) != 0)
                        {
                            Array.Resize(ref buff, read);
                            memstream.Write(buff, 0, buff.Length);
                            memstream.Flush();
                        }
                        bmp = Bitmap.FromStream(memstream) as Bitmap;
                    }
                }
            }
            catch { }
            return bmp;
        }
        public static string GetText(this HttpListenerRequest req)
        {
            try
            {
                using(StreamReader stream = new StreamReader(req.InputStream,req.ContentEncoding))
                {
                    return stream.ReadToEnd();
                }
            }
            catch { }
            return null;
        }
        public static void SendFile(this HttpListenerResponse response,string filepath,string mimetype=null)
        {
            FileInfo fi = new FileInfo(filepath);
            if (fi.Exists)
            {
                using (Stream filestream = File.OpenRead(fi.FullName))
                {
                    response.SendFile(filestream, fi.Name, mimetype);
                }
            }
        }
        public static void SendJson(this HttpListenerResponse response, string json,string type="json")
        {
            response.SendResp(json, $"application/{type}");
        }
        public static void SendResp(this HttpListenerResponse response, string resp,string mimeType = "text/html")
        {
           
                using (Stream filestream = new MemoryStream(Encoding.UTF8.GetBytes(resp)))
                {
                    using (Stream outputstream = response.OutputStream)
                    {
                        response.ContentLength64 = filestream.Length;
                        response.ContentType = mimeType;
                        byte[] buff = new byte[1024];
                        int read = 0;
                        response.Headers.Add("Access-Control-Allow-Origin", "*");
                        response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                    while ((read = filestream.Read(buff, 0, buff.Length)) != 0)
                        {
                            byte[] buffwrite = new byte[read];
                            Array.Copy(buff, buffwrite, read);
                            outputstream.Write(buffwrite, 0, read);
                            outputstream.Flush();
                        }
                    }
                }
        }
        public static void SendFile(this HttpListenerResponse response, Stream filestream,string filename,string mimetype=null)
        {
            string mimeType = MimeMapping.GetMimeMapping(filename);
            if (mimetype != null)
            {
                mimeType = mimetype;
            }
            using (Stream outputstream = response.OutputStream)
                    {
                        response.ContentLength64 = filestream.Length;
                        response.ContentType = mimeType;
                        if (mimeType != "text/html" && mimeType != "application/javascript" && mimeType != "text/css")
                            response.Headers.Add("Content-Disposition", $"attachment; filename={filename}");
                        byte[] buff = new byte[1024];
                        int read = 0;
                        while ((read = filestream.Read(buff, 0, buff.Length)) != 0)
                        {
                            byte[] buffwrite = new byte[read];
                            Array.Copy(buff, buffwrite, read);
                            outputstream.Write(buffwrite, 0, read);
                            outputstream.Flush();
                        }
                    }
        }
    }
}
