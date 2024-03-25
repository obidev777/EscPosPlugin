using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using ObisoftNet.Http;
using System.Net;
using ESC_POS_USB_NET.Printer;
using EscPosPlugin;
using QRCoder;
using System.Drawing.Imaging;

public class ImpresoraTermica
{
    public static bool Runing = true;

    public static void KillOld()
    {
        foreach (var proc in Process.GetProcessesByName("EscPosPlugin"))
        {
            if (Process.GetCurrentProcess().Id != proc.Id)
            {
                proc.Kill();
            }
        }
    }
    [STAThread]
    public static void Main()
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode("TRANSFERMOVIL_ETECSA,TRANSFERENCIA,9224959873172751,100,51285812,", QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);
        qrCode.GetGraphic(10).Save("foto.png",ImageFormat.Png);

        KillOld();

        NotifyIcon notify = new NotifyIcon();
        notify.Icon = Icon.ExtractAssociatedIcon("EscPosPlugin.exe");

        Application.Run(new GUI(notify, (gui) => {

            


            notify.Visible = true;

            notify.BalloonTipIcon = ToolTipIcon.Info;

            notify.BalloonTipText = "Plugin Impresoras Termicas";

            notify.BalloonTipTitle = "Plugin Iniciado!";
            notify.ShowBalloonTip(1000);

            notify.DoubleClick += (s, e) => {
                if (MessageBox.Show("Desea Cerrar El Plugin?","Accion",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Process.GetCurrentProcess().Kill();
                }
            };

            gui.Hide();
            gui.MinimizeBox = true;
            gui.WindowState = FormWindowState.Minimized;


            HttpServer server = new HttpServer(BaseHandle);

            server.Run(4050);

        }));
    }

    public static Printer printer = null;

    private static Bitmap Create(Bitmap bmp,int w=50,int h= 50)
    {
        Bitmap result = new Bitmap(w, h);
        using(Graphics gr = Graphics.FromImage(result))
        {
            gr.Clear(Color.White);
            gr.DrawImage(bmp, 0, 0,w,h);
        }
        return result;
    }
    private static Bitmap CreateCenter(Bitmap bmp, int center = 600)
    {
        Bitmap result = new Bitmap(center, bmp.Height);
        using (Graphics gr = Graphics.FromImage(result))
        {
            gr.Clear(Color.White);
            int x = ((center / 2)-(bmp.Width/2));
            gr.DrawImage(bmp, x,0, bmp.Width, bmp.Height);
        }
        return result;
    }

    private static void BaseHandle(HttpListenerRequest request, HttpListenerResponse response, RouteResult result)
    {
        try
        {
            if (result.Route.Contains("escpos.js"))
            {
               using(Stream escpos = Assembly.GetExecutingAssembly().GetManifestResourceStream($"EscPosPlugin.Resources.escpos.js"))
                {
                    using (Stream jquery = Assembly.GetExecutingAssembly().GetManifestResourceStream($"EscPosPlugin.Resources.jquery.js"))
                    {
                       using(TextReader reader1 = new StreamReader(jquery))
                        {
                            using (TextReader reader2 = new StreamReader(escpos))
                            {
                                string code = reader2.ReadToEnd() + "\n\n" + reader1.ReadToEnd();
                                response.SendResp(code, "application/javascript");
                                GUI.Consolee.Items.Add($"Command: GetEscPosJS");
                            }
                        }
                    }
                }
            }
            else if (result.Route == "/printers")
            {
                GetPrinters(request, response, result);
                GUI.Consolee.Items.Add($"Command: GetPrinters");
            }
            else if (result.Route == "/init")
            {
                GUI.Consolee.Items.Add($"Command: Init");
                string jsonresp = "{\"status\":\"{0}\"}";
                try
                {
                    printer = new Printer(result.Args["printer"]);
                    jsonresp = jsonresp.Replace("{0}", "OK");
                }
                catch
                {
                    jsonresp = jsonresp.Replace("{0}", "ERROR");
                }
                response.SendJson(jsonresp);
            }
            else if (result.Route == "/commit")
            {
                string jsonresp = "{\"status\":\"{0}\"}";
                try
                {
                    string json = request.GetText();
                    var comands = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Command>>(json);

                    foreach (Command cmd in comands)
                    {
                        if (cmd.Name == "Image")
                        {
                            int w = 50;
                            int h = 50;
                            bool scale = false;
                            int center = 0;
                            if (cmd.Args.Count > 1)
                            {
                                w = int.Parse(string.Format("{0}", cmd.Args[1]));
                            }
                            if (cmd.Args.Count > 2)
                            {
                                h = int.Parse(string.Format("{0}", cmd.Args[2]));
                            }
                            if (cmd.Args.Count > 3)
                            {
                                scale = cmd.Args[3];
                            }
                            if (cmd.Args.Count > 4)
                            {
                                center = (int)cmd.Args[4];
                            }
                            string url = cmd.Args[0];
                            Bitmap bitmap = null;
                            if (url.Contains("http"))
                            {
                                HttpSession sess = new HttpSession();
                                sess.Get(url);
                                using (WebResponse resp = sess.Get(url))
                                {
                                    using (BinaryReader stream = new BinaryReader(resp.GetResponseStream()))
                                    {
                                        byte[] buff = new byte[1024];
                                        List<byte> bmp = new List<byte>();
                                        int read = 0;
                                        while ((read = stream.Read(buff, 0, buff.Length)) != 0)
                                        {
                                            byte[] newbuff = new byte[read];
                                            Array.Copy(buff, 0, newbuff, 0, read);
                                            bmp.AddRange(newbuff);
                                        }
                                        bitmap = Create(Bitmap.FromStream(new MemoryStream(bmp.ToArray())) as Bitmap, w, h);
                                    }
                                }
                            }
                            else
                            {
                                if (url.Contains("resource://"))
                                {
                                    url = url.Replace("resource://", "");
                                    bitmap = Create(Bitmap.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream($"EscPosPlugin.Resources.{url}")) as Bitmap, w,h) ;
                                }
                                if (url.Contains("file://"))
                                {
                                    url = url.Replace("file://", "");
                                    bitmap = Create(Bitmap.FromFile(url) as Bitmap,w,h);
                                }
                            }
                            cmd.Args.Clear();
                            if (center != 0)
                            {
                                cmd.Args.Add(CreateCenter(bitmap, center));
                                bitmap.Dispose();
                            }
                            else
                            {
                                cmd.Args.Add(bitmap);
                            }
                            cmd.Args.Add(scale);
                        }
                        if (cmd.Name == "QrCode")
                        {
                            cmd.Name = "Image";
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(cmd.Args[0].ToString(), QRCodeGenerator.ECCLevel.Q);
                            QRCode qrCode = new QRCode(qrCodeData);
                            int w = 30;
                            int h = 30;
                            int dpi = 1;
                            bool scale = true;
                            int center = 0;
                            if (cmd.Args.Count > 1)
                            {
                                w = int.Parse(string.Format("{0}", cmd.Args[1]));
                            }
                            if (cmd.Args.Count > 2)
                            {
                                h = int.Parse(string.Format("{0}", cmd.Args[2]));
                            }
                            if (cmd.Args.Count > 3)
                            {
                                dpi = int.Parse(string.Format("{0}", cmd.Args[3]));
                            }
                            if (cmd.Args.Count > 4)
                            {
                                scale = cmd.Args[4];
                            }
                            if (cmd.Args.Count > 5)
                            {
                                center = (int)cmd.Args[5];
                            }
                            Bitmap qrCodeImage = qrCode.GetGraphic(dpi);
                            cmd.Args.Clear();
                            Bitmap bitmap = Create(qrCodeImage, w, h);
                            qrCodeImage.Dispose();
                            if (center != 0)
                            {
                                cmd.Args.Add(CreateCenter(bitmap, center));
                                bitmap.Dispose();
                            }
                            else{
                                cmd.Args.Add(bitmap);
                            }
                            cmd.Args.Add(scale);
                        }
                        if (cmd.Name == "Init")
                        {
                            printer = new Printer(cmd.Args[0]);
                        }
                        else
                        {
                            bool breaking = false;
                            foreach (var method in printer.GetType().GetMethods())
                            {
                                if (breaking)
                                    break;
                                if (method.IsPublic)
                                {
                                    if (method.Name == cmd.Name)
                                    {
                                        object[] args = cmd.Args.ToArray();
                                        if (cmd.Name == "Font")
                                        {
                                            ESC_POS_USB_NET.Enums.Fonts foo = (ESC_POS_USB_NET.Enums.Fonts)Enum.ToObject(typeof(ESC_POS_USB_NET.Enums.Fonts), args[1]);
                                            args[1] = foo;
                                        }
                                        else if (cmd.Name == "ExpandedMode" || cmd.Name == "CondenseMode")
                                        {
                                            ESC_POS_USB_NET.Enums.PrinterModeState foo = (ESC_POS_USB_NET.Enums.PrinterModeState)Enum.ToObject(typeof(ESC_POS_USB_NET.Enums.PrinterModeState), args[0]);
                                            args[0] = foo;

                                        }

                                        if (cmd.Args.Count > 0)
                                            foreach (var m in printer.GetType().GetMethods())
                                            {
                                                if (m.Name == method.Name)
                                                {
                                                    var argumentos = m.GetParameters();
                                                    if (argumentos.Length == args.Length)
                                                    {
                                                        int complete = 0;
                                                        for (int i = 0; i < argumentos.Length; i++)
                                                        {
                                                            if (cmd.Name == "Separator")
                                                            {
                                                                char separator = args[i].ToString().ToCharArray()[0];
                                                                args[i] = separator;
                                                            }
                                                            if (argumentos[i].ParameterType == args[i].GetType())
                                                            {
                                                                complete++;
                                                            }
                                                        }
                                                        if (complete == args.Length)
                                                        {
                                                            GUI.Consolee.Items.Add($"Command: {cmd.Name}");
                                                            breaking = true;
                                                            m.Invoke(printer, args);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        else
                                        {
                                            GUI.Consolee.Items.Add($"Command: {cmd.Name}");
                                            method.Invoke(printer, null);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    jsonresp = jsonresp.Replace("{0}", "OK");
                }
                catch (Exception ex)
                {
                    jsonresp = jsonresp.Replace("{0}", "ERROR");
                    GUI.Consolee.Items.Add(ex.Message);
                }
                response.SendJson(jsonresp);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private static void GetPrinters(HttpListenerRequest request, HttpListenerResponse response, RouteResult result)
    {
        List<string> printers = new List<string>();
        foreach(var printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
        {
            printers.Add(printer.ToString());
        }
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(printers.ToArray());
        response.SendJson("{\"status\":\"OK\",\"listPrinter\":" + json+"}");
    }

}