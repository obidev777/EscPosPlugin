using ESC_POS_USB_NET.Printer;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscPosPlugin
{
    public partial class GUI : Form

    {
        public static GUI Instance;
        public static ListBox Consolee;
        public delegate void LoadAll(GUI gui);

        public static NotifyIcon notify;

        public static WebBrowser Navigator;

        public GUI(NotifyIcon _notify, LoadAll load =null)
        {
            Instance = this;
            notify = _notify;
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Consolee = Console;
            _load = load;
            Navigator = Browser;
        }
        public static LoadAll _load;
        private void GUI_Load(object sender, EventArgs e)
        {
            GUI.Consolee.Items.Add($"Plugin Iniciado!");
            if (_load!=null) _load(this);

            Navigator = new WebBrowser();
            Navigator.ScriptErrorsSuppressed = true;
            Navigator.DocumentCompleted += Fin;
        }

        private void Fin(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (true)
            {
                string json = GUI.Navigator.DocumentText;
                Dictionary<string, object> data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (data.TryGetValue("status", out object status))
                {
                    if (status.ToString() == "notify")
                        if (data.TryGetValue("msg", out object msg))
                        {
                            if (data.TryGetValue("title", out object title))
                            {
                                GUI.notify.ShowBalloonTip(3000, title.ToString(), msg.ToString(), ToolTipIcon.Info);
                            }
                        }
                }
            }
        }

        private Printer ConnectPrinter(string name) => new Printer(name);

        private void printQR_Click(object sender, EventArgs e)
        {
            
        }

        private void logoYekaPizzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        public Form AccountGUI; 

        private void wNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountGUI = new AccountGUI();
            AccountGUI.Show();
        }
    }
}
