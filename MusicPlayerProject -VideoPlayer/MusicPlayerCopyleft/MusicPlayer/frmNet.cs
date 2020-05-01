using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SFVideoPlayer
{
    public partial class frmNet : Form
    {
        public static int NetNumber = 20;
        gblClass objMainClass = new gblClass();
        public frmNet()
        {
            InitializeComponent();
            
        }

        private void frmNet_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.more;
            NetNumber = 10;
            timGetNet.Enabled = true;
        }

        private void timGetNet_Tick(object sender, EventArgs e)
        {
            NetNumber = NetNumber - 1;
            if (NetNumber == 0)
            {
                if (objMainClass.CheckForInternetConnection() == true)
                {
                    
                    Application.Restart();
                }
                else
                {
                    NetNumber = 10;
                }

            }
        }
    }
}
