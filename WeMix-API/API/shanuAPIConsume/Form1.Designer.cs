namespace WeMixApi
{
	partial class Form1
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
            this.btnRunAPI = new System.Windows.Forms.Button();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.lblMsg = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSongDownloadLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRunAPI
            // 
            this.btnRunAPI.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnRunAPI.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunAPI.Location = new System.Drawing.Point(271, 177);
            this.btnRunAPI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRunAPI.Name = "btnRunAPI";
            this.btnRunAPI.Size = new System.Drawing.Size(275, 56);
            this.btnRunAPI.TabIndex = 0;
            this.btnRunAPI.Text = "Run WeMix API";
            this.btnRunAPI.UseVisualStyleBackColor = false;
            this.btnRunAPI.Click += new System.EventHandler(this.btnRunAPI_Click);
            // 
            // pBar
            // 
            this.pBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBar.Location = new System.Drawing.Point(0, 508);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(868, 26);
            this.pBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pBar.TabIndex = 1;
            // 
            // lblMsg
            // 
            this.lblMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMsg.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(0, 444);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(868, 64);
            this.lblMsg.TabIndex = 2;
            this.lblMsg.Text = "label1";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(581, 359);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(275, 56);
            this.button1.TabIndex = 3;
            this.button1.Text = "Run WeMix API 2";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSongDownloadLocation
            // 
            this.txtSongDownloadLocation.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSongDownloadLocation.Location = new System.Drawing.Point(256, 24);
            this.txtSongDownloadLocation.Name = "txtSongDownloadLocation";
            this.txtSongDownloadLocation.Size = new System.Drawing.Size(571, 34);
            this.txtSongDownloadLocation.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 34);
            this.label1.TabIndex = 5;
            this.label1.Text = "Song Download Location";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(868, 534);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSongDownloadLocation);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pBar);
            this.Controls.Add(this.btnRunAPI);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hit API";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
        private System.Windows.Forms.Button btnRunAPI;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSongDownloadLocation;
        private System.Windows.Forms.Label label1;
    }
}

