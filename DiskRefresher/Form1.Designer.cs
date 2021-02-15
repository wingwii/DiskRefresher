namespace DiskRefresher
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
            this.PnlDriveList = new System.Windows.Forms.Panel();
            this.DriveList = new System.Windows.Forms.ListView();
            this.ColDriveLetter = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColVolumnLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColDriveUsed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColDriveTotal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BtnRefreshDriveList = new System.Windows.Forms.Button();
            this.BtnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PnlWorkProgress = new System.Windows.Forms.Panel();
            this.PnlDriveList.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlDriveList
            // 
            this.PnlDriveList.BackColor = System.Drawing.Color.White;
            this.PnlDriveList.Controls.Add(this.DriveList);
            this.PnlDriveList.Controls.Add(this.BtnRefreshDriveList);
            this.PnlDriveList.Controls.Add(this.BtnStart);
            this.PnlDriveList.Controls.Add(this.label1);
            this.PnlDriveList.Location = new System.Drawing.Point(17, 13);
            this.PnlDriveList.Name = "PnlDriveList";
            this.PnlDriveList.Size = new System.Drawing.Size(370, 420);
            this.PnlDriveList.TabIndex = 0;
            // 
            // DriveList
            // 
            this.DriveList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColDriveLetter,
            this.ColVolumnLabel,
            this.ColDriveUsed,
            this.ColDriveTotal});
            this.DriveList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DriveList.FullRowSelect = true;
            this.DriveList.HideSelection = false;
            this.DriveList.Location = new System.Drawing.Point(0, 32);
            this.DriveList.Name = "DriveList";
            this.DriveList.Size = new System.Drawing.Size(370, 324);
            this.DriveList.TabIndex = 2;
            this.DriveList.UseCompatibleStateImageBehavior = false;
            this.DriveList.View = System.Windows.Forms.View.Details;
            // 
            // ColDriveLetter
            // 
            this.ColDriveLetter.Text = "Drive";
            this.ColDriveLetter.Width = 40;
            // 
            // ColVolumnLabel
            // 
            this.ColVolumnLabel.Text = "Label";
            this.ColVolumnLabel.Width = 120;
            // 
            // ColDriveUsed
            // 
            this.ColDriveUsed.Text = "Used";
            this.ColDriveUsed.Width = 100;
            // 
            // ColDriveTotal
            // 
            this.ColDriveTotal.Text = "Total";
            this.ColDriveTotal.Width = 100;
            // 
            // BtnRefreshDriveList
            // 
            this.BtnRefreshDriveList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnRefreshDriveList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BtnRefreshDriveList.ForeColor = System.Drawing.Color.White;
            this.BtnRefreshDriveList.Location = new System.Drawing.Point(0, 356);
            this.BtnRefreshDriveList.Name = "BtnRefreshDriveList";
            this.BtnRefreshDriveList.Size = new System.Drawing.Size(370, 32);
            this.BtnRefreshDriveList.TabIndex = 1;
            this.BtnRefreshDriveList.Text = "Refresh";
            this.BtnRefreshDriveList.UseVisualStyleBackColor = false;
            this.BtnRefreshDriveList.Click += new System.EventHandler(this.BtnRefreshDriveList_Click);
            // 
            // BtnStart
            // 
            this.BtnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnStart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BtnStart.ForeColor = System.Drawing.Color.White;
            this.BtnStart.Location = new System.Drawing.Point(0, 388);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(370, 32);
            this.BtnStart.TabIndex = 3;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = false;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(370, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Drives";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PnlWorkProgress
            // 
            this.PnlWorkProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlWorkProgress.AutoScroll = true;
            this.PnlWorkProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlWorkProgress.Location = new System.Drawing.Point(403, 13);
            this.PnlWorkProgress.Name = "PnlWorkProgress";
            this.PnlWorkProgress.Size = new System.Drawing.Size(500, 500);
            this.PnlWorkProgress.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 527);
            this.Controls.Add(this.PnlWorkProgress);
            this.Controls.Add(this.PnlDriveList);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Disk Refresh";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.PnlDriveList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlDriveList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnRefreshDriveList;
        private System.Windows.Forms.ListView DriveList;
        private System.Windows.Forms.ColumnHeader ColDriveLetter;
        private System.Windows.Forms.ColumnHeader ColVolumnLabel;
        private System.Windows.Forms.ColumnHeader ColDriveUsed;
        private System.Windows.Forms.ColumnHeader ColDriveTotal;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Panel PnlWorkProgress;
    }
}

