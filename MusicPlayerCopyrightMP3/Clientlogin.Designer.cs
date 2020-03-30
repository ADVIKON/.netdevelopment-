namespace MusicPlayerCopyright
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
            this.btnExtra = new System.Windows.Forms.Button();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.lblLicensedName23 = new System.Windows.Forms.Label();
            this.lblExpiryPlayer = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(125)))), ((int)(((byte)(176)))));
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(125)))), ((int)(((byte)(176)))));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Arial", 12F);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(594, 249);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(77, 38);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(125)))), ((int)(((byte)(176)))));
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(125)))), ((int)(((byte)(176)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Arial", 12F);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(511, 249);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(77, 38);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtLoginPassword
            // 
            this.txtLoginPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLoginPassword.Font = new System.Drawing.Font("Segoe UI", 12.25F);
            this.txtLoginPassword.Location = new System.Drawing.Point(347, 214);
            this.txtLoginPassword.Name = "txtLoginPassword";
            this.txtLoginPassword.PasswordChar = '*';
            this.txtLoginPassword.Size = new System.Drawing.Size(324, 29);
            this.txtLoginPassword.TabIndex = 1;
            this.txtLoginPassword.Text = "admin";
            this.txtLoginPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLoginPassword_KeyDown);
            // 
            // txtloginUserName
            // 
            this.txtloginUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtloginUserName.Font = new System.Drawing.Font("Segoe UI", 12.25F);
            this.txtloginUserName.Location = new System.Drawing.Point(347, 176);
            this.txtloginUserName.Name = "txtloginUserName";
            this.txtloginUserName.Size = new System.Drawing.Size(324, 29);
            this.txtloginUserName.TabIndex = 0;
            this.txtloginUserName.Text = "admin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12.75F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(239, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 40;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(239, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 39;
            this.label1.Text = "User Name";
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
            // chkRemember
            // 
            this.chkRemember.BackColor = System.Drawing.Color.Transparent;
            this.chkRemember.Checked = true;
            this.chkRemember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemember.Font = new System.Drawing.Font("Segoe UI", 10.75F);
            this.chkRemember.ForeColor = System.Drawing.Color.White;
            this.chkRemember.Location = new System.Drawing.Point(348, 249);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(130, 29);
            this.chkRemember.TabIndex = 64;
            this.chkRemember.Text = "Remember Me";
            this.chkRemember.UseVisualStyleBackColor = false;
            // 
            // lblLicensedName23
            // 
            this.lblLicensedName23.BackColor = System.Drawing.Color.Transparent;
            this.lblLicensedName23.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicensedName23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(185)))), ((int)(((byte)(198)))));
            this.lblLicensedName23.Location = new System.Drawing.Point(741, 181);
            this.lblLicensedName23.Name = "lblLicensedName23";
            this.lblLicensedName23.Size = new System.Drawing.Size(160, 22);
            this.lblLicensedName23.TabIndex = 82;
            this.lblLicensedName23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblLicensedName23.Visible = false;
            // 
            // lblExpiryPlayer
            // 
            this.lblExpiryPlayer.BackColor = System.Drawing.Color.Transparent;
            this.lblExpiryPlayer.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.lblExpiryPlayer.ForeColor = System.Drawing.Color.Red;
            this.lblExpiryPlayer.Location = new System.Drawing.Point(247, 312);
            this.lblExpiryPlayer.Name = "lblExpiryPlayer";
            this.lblExpiryPlayer.Size = new System.Drawing.Size(439, 107);
            this.lblExpiryPlayer.TabIndex = 85;
            this.lblExpiryPlayer.Text = "Please pay for your subscription to keep your music online.";
            this.lblExpiryPlayer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(798, 9);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(129, 23);
            this.label3.TabIndex = 39;
            this.label3.Text = "Player id: 21984";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(430, 23);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(115, 99);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 83;
            this.pictureBox2.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblName.ForeColor = System.Drawing.Color.White;
            this.lblName.Location = new System.Drawing.Point(327, 457);
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
            this.ClientSize = new System.Drawing.Size(932, 481);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblExpiryPlayer);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblLicensedName23);
            this.Controls.Add(this.chkRemember);
            this.Controls.Add(this.btnExtra);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtLoginPassword);
            this.Controls.Add(this.txtloginUserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Clientlogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "© Copyright Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Clientlogin_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Clientlogin_FormClosed);
            this.Load += new System.EventHandler(this.Clientlogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Clientlogin_KeyDown);
            this.Move += new System.EventHandler(this.Clientlogin_Move);
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
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.Label lblLicensedName23;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblExpiryPlayer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblName;
    }
}