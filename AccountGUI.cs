using ObisoftNet.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscPosPlugin
{
    public partial class AccountGUI : Form
    {
        public static string Api = "http://yekapizza.free.nf/";
        public static string User = "";
        public static string Pass = "";
        public static Thread UpdateNotify;


        public AccountGUI()
        {
            InitializeComponent();

        }

        private void AccountGUI_Load(object sender, EventArgs e)
        {
            txtApi.Text = Api;
            txtUser.Text = User;
            txtPass.Text = Pass;
        }

        private void btnRegistre_Click(object sender, EventArgs e)
        {
            Api = txtApi.Text;
            User = txtUser.Text;
            Pass = txtPass.Text;
            try
            {
                if (UpdateNotify != null)
                    UpdateNotify.Abort();
            }
            catch { }
            UpdateNotify = new Thread(new ThreadStart(UPD));
            UpdateNotify.Start();
            Close();
        }

        private void UPD()
        {
                while (true)
                {
                    Thread.Sleep(5000);
                    try
                    {
                        GUI.Navigator.Navigate($"{Api}notification.php?user={User}&pass={Pass}");
                    }
                    catch (Exception ex) { }
                }
        }
    }
}
