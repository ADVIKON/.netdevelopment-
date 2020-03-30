namespace MusicPlayerCopyleft
{
    partial class Clientlogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Clientlogin));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtLoginPassword = new System.Windows.Forms.TextBox();
            this.txtloginUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLicensedName23 = new System.Windows.Forms.Label();
            this.btnExtra = new System.Windows.Forms.Button();
            this.picEpidemic = new System.Windows.Forms.PictureBox();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.bgAsianExe = new System.ComponentModel.BackgroundWorker();
            this.panConnect = new System.Windows.Forms.Panel();
            this.lblPercentage = new System.Windows.Forms.Label();
            this.pbar = new System.Windows.Forms.ProgressBar();
            this.lblExpiryPlayer = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picEpidemic)).BeginInit();
            this.panConnect.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Arial", 12F);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(584, 260);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(77, 38);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Arial", 12F);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(501, 260);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(77, 38);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtLoginPassword
            // 
            this.txtLoginPassword.BackColor = System.Drawing.Color.White;
            this.txtLoginPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLoginPassword.Font = new System.Drawing.Font("Arial", 12F);
            this.txtLoginPassword.ForeColor = System.Drawing.Color.Black;
            this.txtLoginPassword.Location = new System.Drawing.Point(342, 228);
            this.txtLoginPassword.Name = "txtLoginPassword";
            this.txtLoginPassword.PasswordChar = '*';
            this.txtLoginPassword.Size = new System.Drawing.Size(319, 26);
            this.txtLoginPassword.TabIndex = 1;
            this.txtLoginPassword.Text = "admin";
            this.txtLoginPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLoginPassword_KeyDown);
            // 
            // txtloginUserName
            // 
            this.txtloginUserName.BackColor = System.Drawing.Color.White;
            this.txtloginUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtloginUserName.Font = new System.Drawing.Font("Arial", 12F);
            this.txtloginUserName.ForeColor = System.Drawing.Color.Black;
            this.txtloginUserName.Location = new System.Drawing.Point(342, 195);
            this.txtloginUserName.Name = "txtloginUserName";
            this.txtloginUserName.Size = new System.Drawing.Size(319, 26);
            this.txtloginUserName.TabIndex = 0;
            this.txtloginUserName.Text = "admin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(236, 229);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 19);
            this.label2.TabIndex = 40;
            this.label2.Text = "Password   :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(236, 197);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 19);
            this.label1.TabIndex = 39;
            this.label1.Text = "User Name :";
            // 
            // lblLicensedName23
            // 
            this.lblLicensedName23.BackColor = System.Drawing.Color.Transparent;
            this.lblLicensedName23.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicensedName23.ForeColor = System.Drawing.Color.White;
            this.lblLicensedName23.Location = new System.Drawing.Point(727, 223);
            this.lblLicensedName23.Name = "lblLicensedName23";
            this.lblLicensedName23.Size = new System.Drawing.Size(94, 16);
            this.lblLicensedName23.TabIndex = 80;
            this.lblLicensedName23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExtra
            // 
            this.btnExtra.Location = new System.Drawing.Point(828, 128);
            this.btnExtra.Name = "btnExtra";
            this.btnExtra.Size = new System.Drawing.Size(92, 37);
            this.btnExtra.TabIndex = 62;
            this.btnExtra.Text = "Extra";
            this.btnExtra.UseVisualStyleBackColor = true;
            this.btnExtra.Visible = false;
            this.btnExtra.Click += new System.EventHandler(this.btnExtra_Click);
            // 
            // picEpidemic
            // 
            this.picEpidemic.BackColor = System.Drawing.Color.Transparent;
            this.picEpidemic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picEpidemic.Image = ((System.Drawing.Image)(resources.GetObject("picEpidemic.Image")));
            this.picEpidemic.Location = new System.Drawing.Point(423, 18);
            this.picEpidemic.Name = "picEpidemic";
            this.picEpidemic.Size = new System.Drawing.Size(115, 99);
            this.picEpidemic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEpidemic.TabIndex = 63;
            this.picEpidemic.TabStop = false;
            // 
            // chkRemember
            // 
            this.chkRemember.BackColor = System.Drawing.Color.Transparent;
            this.chkRemember.Checked = true;
            this.chkRemember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemember.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkRemember.ForeColor = System.Drawing.Color.White;
            this.chkRemember.Location = new System.Drawing.Point(342, 260);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(120, 23);
            this.chkRemember.TabIndex = 64;
            this.chkRemember.Text = "Remember Me";
            this.chkRemember.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(5, 455);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 19);
            this.label4.TabIndex = 84;
            this.label4.Text = "Editor : Paras Technologies";
            this.label4.Visible = false;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // OpenDialog
            // 
            this.OpenDialog.FileName = "OpenDialog";
            // 
            // lblUpdate
            // 
            this.lblUpdate.BackColor = System.Drawing.Color.Transparent;
            this.lblUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUpdate.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdate.ForeColor = System.Drawing.Color.Yellow;
            this.lblUpdate.Location = new System.Drawing.Point(0, 0);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(129, 178);
            this.lblUpdate.TabIndex = 86;
            this.lblUpdate.Text = "Please wait . We are downloading the supporting files";
            this.lblUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bgAsianExe
            // 
            this.bgAsianExe.WorkerReportsProgress = true;
            this.bgAsianExe.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgAsianExe_DoWork);
            this.bgAsianExe.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgAsianExe_ProgressChanged);
            this.bgAsianExe.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgAsianExe_RunWorkerCompleted);
            // 
            // panConnect
            // 
            this.panConnect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panConnect.Controls.Add(this.lblPercentage);
            this.panConnect.Controls.Add(this.lblUpdate);
            this.panConnect.Controls.Add(this.pbar);
            this.panConnect.Location = new System.Drawing.Point(46, 148);
            this.panConnect.Name = "panConnect";
            this.panConnect.Size = new System.Drawing.Size(131, 100);
            this.panConnect.TabIndex = 90;
            this.panConnect.Visible = false;
            // 
            // lblPercentage
            // 
            this.lblPercentage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPercentage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPercentage.ForeColor = System.Drawing.Color.Yellow;
            this.lblPercentage.Location = new System.Drawing.Point(0, 178);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(129, 18);
            this.lblPercentage.TabIndex = 90;
            this.lblPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbar
            // 
            this.pbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbar.ForeColor = System.Drawing.Color.Yellow;
            this.pbar.Location = new System.Drawing.Point(0, 74);
            this.pbar.Name = "pbar";
            this.pbar.Size = new System.Drawing.Size(129, 24);
            this.pbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbar.TabIndex = 89;
            // 
            // lblExpiryPlayer
            // 
            this.lblExpiryPlayer.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.lblExpiryPlayer.ForeColor = System.Drawing.Color.Red;
            this.lblExpiryPlayer.Location = new System.Drawing.Point(240, 302);
            this.lblExpiryPlayer.Name = "lblExpiryPlayer";
            this.lblExpiryPlayer.Size = new System.Drawing.Size(433, 107);
            this.lblExpiryPlayer.TabIndex = 91;
            this.lblExpiryPlayer.Text = "Please pay for your subscription to keep your music online.";
            this.lblExpiryPlayer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(779, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 19);
            this.label5.TabIndex = 92;
            this.label5.Text = "Player id: 20451";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblName.ForeColor = System.Drawing.Color.White;
            this.lblName.Location = new System.Drawing.Point(319, 455);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(600, 19);
            this.lblName.TabIndex = 96;
            this.lblName.Text = "Licenced by Jan Rooijakkers for Cloudcasting, Manage your Media, MYCLAUD and S&&S" +
    " Solutions";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Visible = false;
            // 
            // Clientlogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(125)))), ((int)(((byte)(176)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(932, 480);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblExpiryPlayer);
            this.Controls.Add(this.panConnect);
            this.Controls.Add(this.lblLicensedName23);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkRemember);
            this.Controls.Add(this.picEpidemic);
            this.Controls.Add(this.btnExtra);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtLoginPassword);
            this.Controls.Add(this.txtloginUserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Clientlogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Copyleft Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Clientlogin_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Clientlogin_FormClosed);
            this.Load += new System.EventHandler(this.Clientlogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Clientlogin_KeyDown);
            this.Move += new System.EventHandler(this.Clientlogin_Move);
            ((System.ComponentModel.ISupportInitialize)(this.picEpidemic)).EndInit();
            this.panConnect.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtLoginPassword;
        private System.Windows.Forms.TextBox txtloginUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExtra;
        private System.Windows.Forms.PictureBox picEpidemic;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLicensedName23;
        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.Label lblUpdate;
        private System.ComponentModel.BackgroundWorker bgAsianExe;
        private System.Windows.Forms.Panel panConnect;
        private System.Windows.Forms.Label lblPercentage;
        private System.Windows.Forms.ProgressBar pbar;
        private System.Windows.Forms.Label lblExpiryPlayer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblName;
    }
}