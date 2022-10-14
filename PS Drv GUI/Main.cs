using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS_Drv_GUI
{
    public partial class Main : Form
    {
        //Set dark mode title bar and bypass wow64 redirection

        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        protected override void OnHandleCreated(EventArgs e)
        {
            DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4);
            DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
            DwmSetWindowAttribute(Handle, 35, new[] { 1 }, 4);
            DwmSetWindowAttribute(Handle, 38, new[] { 1 }, 4);
            IntPtr wow64Value = IntPtr.Zero;
            Wow64DisableWow64FsRedirection(ref wow64Value);
        }


        public Main()
        {
            InitializeComponent();
        }

        private void bb_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbb.Text = fbd.SelectedPath + "\\Drv_" + DateTime.Now.ToString("HH-mm_dd-MM-yy");
                }
            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            if (tbb.Text == "")
            {

            }
            else
            {
                Directory.CreateDirectory(tbb.Text);
                string arg = "Export-WindowsDriver -Online -Destination " + "\"" + tbb.Text + "\"";

                Process p = new Process();
                p.StartInfo.FileName = "powershell.exe";
                p.StartInfo.Arguments = arg;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                p.Start();
                p.WaitForExit();

                using(WebClient wc = new WebClient())
                {
                    wc.DownloadFile("https://raw.githubusercontent.com/STY1001/PS-Drv-GUI/master/Restore.exe", tbb.Text + "\\Restore.exe");
                }
            }
        }

        private void br_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbr.Text = fbd.SelectedPath + "\\Restore.exe";
                }
            }
        }

        private void r_Click(object sender, EventArgs e)
        {
            if (tbr.Text == "")
            {

            }
            else
            {
                System.Diagnostics.Process.Start(tbr.Text);
            }
        }
    }
}
