namespace SFVideoPlayer
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
            this.components = new System.ComponentModel.Container();
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
            this.lblName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.picDealer = new System.Windows.Forms.PictureBox();
            this.timAutoStart = new System.Windows.Forms.Timer(this.components);
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblPlayerId = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.picEpidemic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDealer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(768, 284);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(103, 47);
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
            this.btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(657, 284);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(103, 47);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtLoginPassword
            // 
            this.txtLoginPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(65)))), ((int)(((byte)(66)))));
            this.txtLoginPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLoginPassword.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtLoginPassword.ForeColor = System.Drawing.Color.White;
            this.txtLoginPassword.Location = new System.Drawing.Point(471, 242);
            this.txtLoginPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLoginPassword.Name = "txtLoginPassword";
            this.txtLoginPassword.PasswordChar = '*';
            this.txtLoginPassword.Size = new System.Drawing.Size(399, 34);
            this.txtLoginPassword.TabIndex = 1;
            this.txtLoginPassword.Text = "admin";
            this.txtLoginPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLoginPassword_KeyDown);
            // 
            // txtloginUserName
            // 
            this.txtloginUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(65)))), ((int)(((byte)(66)))));
            this.txtloginUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtloginUserName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtloginUserName.ForeColor = System.Drawing.Color.White;
            this.txtloginUserName.Location = new System.Drawing.Point(471, 202);
            this.txtloginUserName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtloginUserName.Name = "txtloginUserName";
            this.txtloginUserName.Size = new System.Drawing.Size(399, 34);
            this.txtloginUserName.TabIndex = 0;
            this.txtloginUserName.Text = "admin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(348, 246);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 28);
            this.label2.TabIndex = 40;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(348, 204);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 28);
            this.label1.TabIndex = 39;
            this.label1.Text = "User Name";
            // 
            // lblLicensedName23
            // 
            this.lblLicensedName23.BackColor = System.Drawing.Color.Transparent;
            this.lblLicensedName23.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicensedName23.ForeColor = System.Drawing.Color.White;
            this.lblLicensedName23.Location = new System.Drawing.Point(693, 457);
            this.lblLicensedName23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLicensedName23.Name = "lblLicensedName23";
            this.lblLicensedName23.Size = new System.Drawing.Size(521, 20);
            this.lblLicensedName23.TabIndex = 80;
            this.lblLicensedName23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLicensedName23.Visible = false;
            // 
            // btnExtra
            // 
            this.btnExtra.Location = new System.Drawing.Point(1104, 158);
            this.btnExtra.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExtra.Name = "btnExtra";
            this.btnExtra.Size = new System.Drawing.Size(123, 46);
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
            this.picEpidemic.Location = new System.Drawing.Point(37, 84);
            this.picEpidemic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picEpidemic.Name = "picEpidemic";
            this.picEpidemic.Size = new System.Drawing.Size(129, 100);
            this.picEpidemic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEpidemic.TabIndex = 63;
            this.picEpidemic.TabStop = false;
            this.picEpidemic.Visible = false;
            // 
            // chkRemember
            // 
            this.chkRemember.BackColor = System.Drawing.Color.Transparent;
            this.chkRemember.Checked = true;
            this.chkRemember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemember.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.chkRemember.ForeColor = System.Drawing.Color.White;
            this.chkRemember.Location = new System.Drawing.Point(471, 284);
            this.chkRemember.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(175, 28);
            this.chkRemember.TabIndex = 64;
            this.chkRemember.Text = "Remember Me";
            this.chkRemember.UseVisualStyleBackColor = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblName.ForeColor = System.Drawing.Color.White;
            this.lblName.Location = new System.Drawing.Point(567, 561);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(619, 23);
            this.lblName.TabIndex = 66;
            this.lblName.Text = "Licenced by  for Cloudcasting, Manage your Media and S&&S Solutions";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(8, 561);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(214, 23);
            this.label4.TabIndex = 84;
            this.label4.Text = "Editor:- Paras Technologies";
            this.label4.Visible = false;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // picDealer
            // 
            this.picDealer.BackColor = System.Drawing.Color.Transparent;
            this.picDealer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picDealer.Location = new System.Drawing.Point(36, 352);
            this.picDealer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picDealer.Name = "picDealer";
            this.picDealer.Size = new System.Drawing.Size(149, 124);
            this.picDealer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDealer.TabIndex = 88;
            this.picDealer.TabStop = false;
            // 
            // timAutoStart
            // 
            this.timAutoStart.Enabled = true;
            this.timAutoStart.Interval = 1000;
            this.timAutoStart.Tick += new System.EventHandler(this.timAutoStart_Tick);
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentTime.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCurrentTime.ForeColor = System.Drawing.Color.Yellow;
            this.lblCurrentTime.Location = new System.Drawing.Point(732, 338);
            this.lblCurrentTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(200, 26);
            this.lblCurrentTime.TabIndex = 91;
            this.lblCurrentTime.Text = "10";
            this.lblCurrentTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label5.ForeColor = System.Drawing.Color.Yellow;
            this.label5.Location = new System.Drawing.Point(479, 337);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(247, 28);
            this.label5.TabIndex = 91;
            this.label5.Text = "Automatic redirect with in :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label6.ForeColor = System.Drawing.Color.Yellow;
            this.label6.Location = new System.Drawing.Point(763, 337);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 26);
            this.label6.TabIndex = 91;
            this.label6.Text = "sec";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(276, 372);
            this.lblError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(688, 85);
            this.lblError.TabIndex = 92;
            this.lblError.Text = "The store and forward version is not activated on this token. Please contact your" +
    " dealer.";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(572, 30);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(168, 127);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 94;
            this.pictureBox2.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(835, 534);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(372, 23);
            this.label7.TabIndex = 66;
            this.label7.Text = "Alenka Media International Music Management";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Visible = false;
            // 
            // lblPlayerId
            // 
            this.lblPlayerId.AutoSize = true;
            this.lblPlayerId.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerId.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblPlayerId.ForeColor = System.Drawing.Color.White;
            this.lblPlayerId.Location = new System.Drawing.Point(1016, 6);
            this.lblPlayerId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlayerId.Name = "lblPlayerId";
            this.lblPlayerId.Size = new System.Drawing.Size(194, 23);
            this.lblPlayerId.TabIndex = 95;
            this.lblPlayerId.Text = "Your player id is: 655236";
            this.lblPlayerId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(871, 68);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(344, 28);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 96;
            this.progressBar1.Visible = false;
            // 
            // Clientlogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(65)))), ((int)(((byte)(66)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1243, 591);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblPlayerId);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.picEpidemic);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblCurrentTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.picDealer);
            this.Controls.Add(this.lblLicensedName23);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.chkRemember);
            this.Controls.Add(this.btnExtra);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtLoginPassword);
            this.Controls.Add(this.txtloginUserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Coral;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Clientlogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Clientlogin_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Clientlogin_FormClosed);
            this.Load += new System.EventHandler(this.Clientlogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Clientlogin_KeyDown);
            this.Move += new System.EventHandler(this.Clientlogin_Move);
            ((System.ComponentModel.ISupportInitialize)(this.picEpidemic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDealer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
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
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLicensedName23;
        private System.Windows.Forms.PictureBox picDealer;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblPlayerId;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timAutoStart;
    }
}