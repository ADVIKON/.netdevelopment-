using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
namespace MusicPlayerCopyright
{
    public partial class frmStart : Form
    {
        Timer timStart = new Timer();
        public frmStart()
        {
            InitializeComponent();
        }

        private void bgOggSetup_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string sUrlToReadFileFrom = "http://www.manageyourclaudio.eu/manageyourclaudioSetup/PlayerFiles/XiphOrgOpenCodecs08517777.exe";
                string sFilePathToWriteFileTo = Application.StartupPath + "\\XiphOrgOpenCodecs08517777.exe";
                Uri url = new Uri(sUrlToReadFileFrom);
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                response.Close();
                Int64 iSize = response.ContentLength;
                Int64 iRunningByteTotal = 0;
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    using (System.IO.Stream streamRemote = client.OpenRead(new Uri(sUrlToReadFileFrom)))
                    {
                        using (Stream streamLocal = new FileStream(sFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            int iByteSize = 0;
                            byte[] byteBuffer = new byte[iSize];
                            while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                            {
                                streamLocal.Write(byteBuffer, 0, iByteSize);
                                iRunningByteTotal += iByteSize;
                                double dIndex = (double)(iRunningByteTotal);
                                double dTotal = (double)byteBuffer.Length;
                                double dProgressPercentage = (dIndex / dTotal);
                                int iProgressPercentage = (int)(dProgressPercentage * 100);
                                bgOggSetup.ReportProgress(iProgressPercentage);
                            }
                            streamLocal.Close();
                        }
                        streamRemote.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Application.Restart();
                Console.WriteLine(ex.Message);
            }
        }

        private void bgOggSetup_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbar.Value = e.ProgressPercentage;
        }

        private void bgOggSetup_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GC.Collect();
            this.Hide();
            string VersionApplicationPath = "";
            VersionApplicationPath = Application.StartupPath + "\\XiphOrgOpenCodecs08517777.exe";
            System.Diagnostics.Process.Start(VersionApplicationPath);
            timStart.Start();
            timStart.Enabled = true;
           // timSessionTime.Enabled = true;
        }

        private void timCheck_Tick(object sender, EventArgs e)
        {
            gblClass ObjClass = new gblClass();
            if (ObjClass.CheckForInternetConnection() == false)
            {
                timCheck.Enabled = false;
                timCheck.Stop();
                timStart.Enabled = false;
                timStart.Stop();
                MessageBox.Show("Please check your Internet connection.", "Music Player", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            timStart.Tick += new EventHandler(timStart_Tick);
            timStart.Interval = 10;
            try
            {
                if (checkInstalled("Xiph.Org Open Codecs 0.85.17777"))
                {
                    timStart.Enabled = false;
                    timStart.Stop();
                    userlogin objUserlogin = new userlogin();
                    objUserlogin.Show();
                    timCheck.Enabled = false;
                    timCheck.Stop();
                    timStart.Enabled = false;
                    timStart.Stop();
                    this.Hide();
                    return;
                }
                else
                {
                    timCheck.Enabled = false;
                    timCheck.Stop();
                    if (File.Exists(Application.StartupPath + "\\XiphOrgOpenCodecs08517777.exe"))
                    {
                        string VersionApplicationPath = "";
                        VersionApplicationPath = Application.StartupPath + "\\XiphOrgOpenCodecs08517777.exe";
                        FileInfo fi = new FileInfo(VersionApplicationPath);
                        var size = fi.Length;
                        if (size >= 2653944)
                        {
                            this.Hide();
                            System.Diagnostics.Process.Start(VersionApplicationPath);
                            timStart.Start();
                            timStart.Enabled = true;
                        }
                        else
                        {
                            bgOggSetup.RunWorkerAsync();
                        }
                    }
                    else
                    {
                        bgOggSetup.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void timStart_Tick(object sender, EventArgs e)
        {
            try
            {
                if (checkInstalled("Xiph.Org Open Codecs 0.85.17777"))
                {
                    Process[] prs = Process.GetProcesses();
                    foreach (Process pr in prs)
                    {
                        if (pr.ProcessName == "XiphOrgOpenCodecs08517777")
                            pr.Kill();
                    }
                    userlogin objUserlogin = new userlogin();
                    objUserlogin.Show();
                    timStart.Enabled = false;
                    timStart.Stop();
                    timCheck.Enabled = false;
                    timCheck.Stop();
                    this.Hide();
                    return;
                }
                else
                {
                    string proc = "XiphOrgOpenCodecs08517777";
                    Process[] processes = Process.GetProcessesByName(proc);
                    if (processes.Length >= 1)
                    {
                        
                    }
                    else
                    {
                        Process[] prs = Process.GetProcesses();
                        foreach (Process pr in prs)
                        {
                            if (pr.ProcessName == "MusicPlayerCopyright")
                                pr.Kill();
                        }
                        
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public static bool checkInstalled(string c_name)
        {
            return true;
            string displayName;

            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains(c_name))
                    {
                        return true;
                    }
                }
                key.Close();
            }

            registryKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            key = Registry.LocalMachine.OpenSubKey(registryKey);
            if (key != null)
            {
                foreach (RegistryKey subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
                {
                    displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName != null && displayName.Contains(c_name))
                    {
                        return true;
                    }
                }
                key.Close();
            }
            return false;
        }

        private void frmStart_Load(object sender, EventArgs e)
        {

        }

        private void frmStart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = false;
                Application.Exit();
            }
        }

        
    }
}
