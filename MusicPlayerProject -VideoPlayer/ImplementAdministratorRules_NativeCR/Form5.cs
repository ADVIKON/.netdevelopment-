using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using NetFwTypeLib;
namespace ImplementAdministratorRules_Player
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            try
            {
                SetPermissions(Application.StartupPath);
                INetFwPolicy2 firewallPolicyDel = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                for (int i = 0; i <= 20; i++)
                {
                    firewallPolicyDel.Rules.Remove("SFVideoPlayer");
                }

                INetFwPolicy2 firewallPolicyDel1 = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                for (int i = 0; i <= 20; i++)
                {
                    firewallPolicyDel1.Rules.Remove("UpdateSFVideoPlayer");
                }

                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                firewallRule.Description = "Allow notepad";
                firewallRule.ApplicationName = Application.StartupPath + "\\SFVideoPlayer.exe";
                firewallRule.Enabled = true;
                firewallRule.InterfaceTypes = "All";
                firewallRule.Name = "SFVideoPlayer";

                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Add(firewallRule);


                INetFwRule firewallRule1 = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule1.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                firewallRule1.Description = "Allow notepad1";
                firewallRule1.ApplicationName = Application.StartupPath + "\\UpdateSFVideoPlayer.exe";
                firewallRule1.Enabled = true;
                firewallRule1.InterfaceTypes = "All";
                firewallRule1.Name = "UpdateSFVideoPlayer";

                INetFwPolicy2 firewallPolicy1 = (INetFwPolicy2)Activator.CreateInstance(
                    Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Add(firewallRule1);

                Application.Exit();
            }
            catch (Exception ex)
            {
                 Application.Exit();
            }
        }
        private static void SetPermissions(string dirPath)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(dirPath);
                WindowsIdentity self = System.Security.Principal.WindowsIdentity.GetCurrent();
                DirectorySecurity ds = info.GetAccessControl();
                ds.AddAccessRule(new FileSystemAccessRule(self.Name,
                FileSystemRights.FullControl,
                InheritanceFlags.ObjectInherit |
                InheritanceFlags.ContainerInherit,
                PropagationFlags.None,
                AccessControlType.Allow));
                info.SetAccessControl(ds);
            }
            catch (Exception ex)
            {
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}


 
