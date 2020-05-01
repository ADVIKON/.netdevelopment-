namespace SFVideoPlayer
{
    partial class frmStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStart));
            this.bgOggSetup = new System.ComponentModel.BackgroundWorker();
            this.timCheck = new System.Windows.Forms.Timer(this.components);
            this.pbar = new System.Windows.Forms.ProgressBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bgOggSetup
            // 
            this.bgOggSetup.WorkerReportsProgress = true;
            this.bgOggSetup.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgOggSetup_DoWork);
            this.bgOggSetup.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgOggSetup_ProgressChanged);
            this.bgOggSetup.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgOggSetup_RunWorkerCompleted);
            // 
            // timCheck
            // 
            this.timCheck.Interval = 1000;
            this.timCheck.Tick += new System.EventHandler(this.timCheck_Tick);
            // 
            // pbar
            // 
            this.pbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbar.ForeColor = System.Drawing.Color.Yellow;
            this.pbar.Location = new System.Drawing.Point(0, 109);
            this.pbar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbar.Name = "pbar";
            this.pbar.Size = new System.Drawing.Size(525, 21);
            this.pbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbar.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(525, 52);
            this.label8.TabIndex = 155;
            this.label8.Text = "Please wait . We are downloading the supporting files";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(525, 38);
            this.label1.TabIndex = 156;
            this.label1.Text = "Thanks for your patience";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(65)))), ((int)(((byte)(66)))));
            this.ClientSize = new System.Drawing.Size(525, 130);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "frmStart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStart_FormClosing);
            this.Load += new System.EventHandler(this.frmStart_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgOggSetup;
        private System.Windows.Forms.Timer timCheck;
        private System.Windows.Forms.ProgressBar pbar;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
    }
}