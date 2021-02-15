using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DiskRefresher
{
    public partial class Form1 : Form
    {
        private int mWorkerCount = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.RefreshDriveList();
        }

        private void BtnRefreshDriveList_Click(object sender, EventArgs e)
        {
            this.RefreshDriveList();
        }

        private static long ConvertBytesToMBs(long num)
        {
            return (num / 0x100000);
        }

        private void RefreshDriveList()
        {
            this.DriveList.Items.Clear();
            var driveList = DriveInfo.GetDrives();
            foreach (var drive in driveList)
            {
                var rootPath = drive.RootDirectory.FullName;
                if (rootPath.Length > 2)
                {
                    rootPath = rootPath.Substring(0, 2);
                }

                var label = string.Empty;
                try { label = drive.VolumeLabel; }
                catch (Exception) { }

                var totalSize = (long)0;
                var availSpace = (long)0;
                try
                {
                    totalSize = drive.TotalSize;
                    availSpace = drive.TotalFreeSpace;
                }
                catch (Exception) { }
                
                var usedSpace = totalSize - availSpace;
                var li = new ListViewItem();
                li.Text = rootPath;
                li.SubItems.Add(label);
                li.SubItems.Add(ConvertBytesToMBs(usedSpace).ToString());
                li.SubItems.Add(ConvertBytesToMBs(totalSize).ToString());

                this.DriveList.Items.Add(li);
            }
            //
        }

        private void StopDriveRefreshing()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate ()
                {
                    this.StopDriveRefreshing();
                }));
                return;
            }

            this.BtnStart.Text = "Start";
            this.DriveList.Enabled = true;
            this.BtnRefreshDriveList.Enabled = true;
            this.BtnStart.Enabled = true;
        }

        private void StartDriveRefreshing()
        {
            this.mWorkerCount = 0;
            var selectedItems = this.DriveList.SelectedItems;
            if (null == selectedItems)
            {
                return;
            }

            var workerCount = selectedItems.Count;
            if (0 == workerCount)
            {
                return;
            }

            //this.BtnStart.Text = "Stop";
            this.BtnStart.Enabled = false;

            this.DriveList.Enabled = false;
            this.BtnRefreshDriveList.Enabled = false;

            this.mWorkerCount = workerCount;
            this.PnlWorkProgress.SuspendLayout();
            this.PnlWorkProgress.Controls.Clear();
            foreach (ListViewItem li in selectedItems)
            {
                var rootPath = li.Text;
                var title = "[Drive ";
                title += rootPath.Substring(0, 1);
                title += "] ";
                title += li.SubItems[1].Text;

                var uc = new UcDriveRefresh();
                uc.Dock = DockStyle.Top;
                uc.Title = title;
                uc.RootPath = rootPath;
                uc.OnStop += this.DriveRefreshing_OnStop;
                uc.Start();

                this.PnlWorkProgress.Controls.Add(uc);
            }
            this.PnlWorkProgress.ResumeLayout();
        }

        private void DriveRefreshing_OnStop(UcDriveRefresh sender)
        {
            var workerCount = Interlocked.Decrement(ref this.mWorkerCount);
            if (0 == workerCount)
            {
                this.StopDriveRefreshing();
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (this.BtnRefreshDriveList.Enabled)
            {
                this.StartDriveRefreshing();
            }
            else
            {
                //this.StopDriveRefreshing();
            }
        }

        //
    }
}
