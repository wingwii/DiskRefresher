namespace DiskRefresher
{
    partial class UcDriveRefresh
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LblTitle = new System.Windows.Forms.Label();
            this.TxtCurrentFile = new System.Windows.Forms.TextBox();
            this.PrgbCurrentFile = new System.Windows.Forms.ProgressBar();
            this.PrgbTotal = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnPause = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.LblSpeed = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblTitle
            // 
            this.LblTitle.BackColor = System.Drawing.Color.Black;
            this.LblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.LblTitle.ForeColor = System.Drawing.Color.White;
            this.LblTitle.Location = new System.Drawing.Point(0, 0);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(500, 24);
            this.LblTitle.TabIndex = 1;
            this.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtCurrentFile
            // 
            this.TxtCurrentFile.BackColor = System.Drawing.Color.White;
            this.TxtCurrentFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtCurrentFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.TxtCurrentFile.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtCurrentFile.ForeColor = System.Drawing.Color.Black;
            this.TxtCurrentFile.Location = new System.Drawing.Point(0, 24);
            this.TxtCurrentFile.Name = "TxtCurrentFile";
            this.TxtCurrentFile.ReadOnly = true;
            this.TxtCurrentFile.Size = new System.Drawing.Size(500, 19);
            this.TxtCurrentFile.TabIndex = 2;
            // 
            // PrgbCurrentFile
            // 
            this.PrgbCurrentFile.BackColor = System.Drawing.Color.Gray;
            this.PrgbCurrentFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.PrgbCurrentFile.ForeColor = System.Drawing.Color.LimeGreen;
            this.PrgbCurrentFile.Location = new System.Drawing.Point(0, 63);
            this.PrgbCurrentFile.Name = "PrgbCurrentFile";
            this.PrgbCurrentFile.Size = new System.Drawing.Size(500, 18);
            this.PrgbCurrentFile.TabIndex = 3;
            // 
            // PrgbTotal
            // 
            this.PrgbTotal.BackColor = System.Drawing.Color.Silver;
            this.PrgbTotal.Dock = System.Windows.Forms.DockStyle.Top;
            this.PrgbTotal.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.PrgbTotal.Location = new System.Drawing.Point(0, 81);
            this.PrgbTotal.Name = "PrgbTotal";
            this.PrgbTotal.Size = new System.Drawing.Size(500, 18);
            this.PrgbTotal.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnPause);
            this.panel1.Controls.Add(this.BtnStop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 99);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 30);
            this.panel1.TabIndex = 6;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // BtnPause
            // 
            this.BtnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnPause.Dock = System.Windows.Forms.DockStyle.Left;
            this.BtnPause.ForeColor = System.Drawing.Color.White;
            this.BtnPause.Location = new System.Drawing.Point(0, 0);
            this.BtnPause.Name = "BtnPause";
            this.BtnPause.Size = new System.Drawing.Size(250, 30);
            this.BtnPause.TabIndex = 7;
            this.BtnPause.Text = "Pause";
            this.BtnPause.UseVisualStyleBackColor = false;
            this.BtnPause.Click += new System.EventHandler(this.BtnPause_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnStop.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnStop.ForeColor = System.Drawing.Color.White;
            this.BtnStop.Location = new System.Drawing.Point(250, 0);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(250, 30);
            this.BtnStop.TabIndex = 6;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // LblSpeed
            // 
            this.LblSpeed.BackColor = System.Drawing.Color.Silver;
            this.LblSpeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.LblSpeed.Location = new System.Drawing.Point(0, 43);
            this.LblSpeed.Name = "LblSpeed";
            this.LblSpeed.Size = new System.Drawing.Size(500, 20);
            this.LblSpeed.TabIndex = 7;
            this.LblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UcDriveRefresh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PrgbTotal);
            this.Controls.Add(this.PrgbCurrentFile);
            this.Controls.Add(this.LblSpeed);
            this.Controls.Add(this.TxtCurrentFile);
            this.Controls.Add(this.LblTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UcDriveRefresh";
            this.Size = new System.Drawing.Size(500, 140);
            this.Load += new System.EventHandler(this.UcDriveRefresh_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.TextBox TxtCurrentFile;
        private System.Windows.Forms.ProgressBar PrgbCurrentFile;
        private System.Windows.Forms.ProgressBar PrgbTotal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnPause;
        private System.Windows.Forms.Label LblSpeed;
    }
}
