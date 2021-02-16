using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Management;

namespace DiskRefresher
{
    public partial class UcDriveRefresh : UserControl
    {
        private const byte TEMPERATURE_ATTRIBUTE = 194;


        private long mMinHashFileSize = 5 * 0x100000;
        private int mMaxFileYearsOld = -1;
        private int mMaxHddTemperature = 45;

        private string mHashFile = string.Empty;
        private string mRootPath = string.Empty;
        private string mTmpFile = string.Empty;
        private int mCurrentYear = 0;
        private string mPnpDevID = string.Empty;

        private bool mRunning = false;
        private bool mIsFinished = false;
        private bool mIsPaused = false;
        private bool mIsCoolDown = false;
        private long mTotalBytes = 0;
        private long mBytesTransfered = 0;
        private byte[] mBuffer = new byte[16 * 0x100000];


        [StructLayout(LayoutKind.Sequential)]
        private struct FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            [MarshalAs(UnmanagedType.U2)] public short Year;
            [MarshalAs(UnmanagedType.U2)] public short Month;
            [MarshalAs(UnmanagedType.U2)] public short DayOfWeek;
            [MarshalAs(UnmanagedType.U2)] public short Day;
            [MarshalAs(UnmanagedType.U2)] public short Hour;
            [MarshalAs(UnmanagedType.U2)] public short Minute;
            [MarshalAs(UnmanagedType.U2)] public short Second;
            [MarshalAs(UnmanagedType.U2)] public short Milliseconds;
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool MoveFileEx(
             [MarshalAs(UnmanagedType.LPTStr)] string lpExistingFileName,
             [MarshalAs(UnmanagedType.LPTStr)] string lpNewFileName,
             [MarshalAs(UnmanagedType.U4)] uint dwFlags
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr CreateFile(
             [MarshalAs(UnmanagedType.LPTStr)] string filename,
             [MarshalAs(UnmanagedType.U4)] uint access,
             [MarshalAs(UnmanagedType.U4)] FileShare share,
             IntPtr securityAttributes,
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
             [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
             IntPtr templateFile
        );

        [DllImport("kernel32.dll", SetLastError = false)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetFileSizeEx(IntPtr hFile, out long lpFileSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadFile(
            IntPtr handle,
            byte[] buffer,
            uint nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead,
            IntPtr lpOverlapped
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile(
            IntPtr handle,
            byte[] buffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            IntPtr lpOverlapped
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetFileTime(
            IntPtr hFile,
            ref FILETIME lpCreationTime,
            ref FILETIME lpLastAccessTime,
            ref FILETIME lpLastWriteTime
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetFileTime(
            IntPtr hFile,
            ref FILETIME lpCreationTime,
            IntPtr lpLastAccessTime,
            IntPtr lpLastWriteTime
        );

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        private static extern bool FileTimeToSystemTime(
           [In] ref FILETIME lpFileTime,
           out SYSTEMTIME lpSystemTime
        );


        public delegate void StopEvtCallback(UcDriveRefresh sender);
        public event StopEvtCallback OnStop = null;


        public UcDriveRefresh()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return this.LblTitle.Text; }
            set { this.LblTitle.Text = value; }
        }

        public string RootPath
        {
            get { return this.mRootPath; }
            set
            {
                this.mRootPath = value;
                if (this.mRootPath.Length > 0)
                {
                    this.mRootPath = this.mRootPath.Substring(0, 1) + ":\\";
                }
            }
        }

        public long MinHashFileSize
        {
            get { return this.mMinHashFileSize; }
            set { this.mMinHashFileSize = value; }
        }

        public int FileYearsOld
        {
            get { return this.mMaxFileYearsOld; }
            set { this.mMaxFileYearsOld = value; }
        }

        private void UcDriveRefresh_Load(object sender, EventArgs e)
        {
            //
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            var w = this.panel1.ClientSize.Width;
            w = ((w / 2) - 2);
            this.BtnPause.Width = w;
            this.BtnStop.Width = w;

            //
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (this.mIsPaused)
            {
                this.BtnPause.Text = "Pause";
            }
            else
            {
                this.BtnPause.Text = "Resume";
            }
            this.mIsPaused = !this.mIsPaused;
        }

        private class ProgressBarValues
        {
            public long value = 0;
            public long max = 0;
        }

        public void Start()
        {
            var now = DateTime.Now;
            this.mCurrentYear = now.Year;

            this.mHashFile = Path.Combine(this.mRootPath, "FileDataHashes.txt");
            //this.mTmpFile = Path.Combine(this.mRootPath, "0390c03e-8178-4fb8-907c-33ebe259a384.tmp");
            this.mTmpFile = Path.Combine(this.mRootPath, Guid.NewGuid().ToString() + ".tmp");

            this.PreparePnpDevID();

            this.PrgbTotal.Maximum = 100;
            this.PrgbCurrentFile.Maximum = 100;

            var pv = new ProgressBarValues();
            this.PrgbTotal.Tag = pv;
            this.PrgbCurrentFile.Tag = new ProgressBarValues();

            var di = new DriveInfo(this.mRootPath);
            var usedSpace = di.TotalSize - di.TotalFreeSpace;
            pv.max = usedSpace;
            this.mTotalBytes = usedSpace;

            this.mRunning = true;
            var thread = new Thread(this.ThreadRefreshDrive);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Stop()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    this.Stop();
                }));
                return;
            }

            var prevRunning = this.mRunning;
            this.mRunning = false;
            this.BtnStop.Enabled = false;

            try { File.Delete(this.mTmpFile); }
            catch (Exception) { }

            if (prevRunning)
            {
                var callback = this.OnStop;
                if (callback != null)
                {
                    callback(this);
                }
            }
            //
        }

        public void TimerTick()
        {
            var nBytes = (long)Interlocked.Exchange(ref this.mBytesTransfered, 0);
            //nBytes *= 1;

            if (this.mIsCoolDown)
            {
                this.CheckHddTemperature();
            }

            var nKBytes = nBytes / 1024;
            if (nBytes > 0 && 0 == nKBytes)
            {
                nKBytes = 1;
            }

            var nSec = (long)-1;
            if (nBytes > 0)
            {
                var pv = this.PrgbTotal.Tag as ProgressBarValues;
                nSec = (pv.max - pv.value) / nBytes;
                ++nSec;
            }

            var sb = new StringBuilder();
            sb.Append("Speed: ");
            sb.Append(nKBytes.ToString());
            sb.Append(" KBs/s");
            if (nSec >= 0)
            {
                var seconds = nSec;
                var hours = seconds / 3600;
                seconds = seconds % 3600;
                var minutes = seconds / 60;
                seconds = seconds % 60;

                sb.Append(", ETA: ");
                sb.Append(hours.ToString());
                sb.Append("h ");
                sb.Append(minutes.ToString());
                sb.Append("m ");
                sb.Append(seconds.ToString());
                sb.Append("s");
            }
            this.LblSpeed.Text = sb.ToString();
        }

        private static ManagementObjectCollection ExecuteWmiQuery(string scope, string query)
        {
            var result = (ManagementObjectCollection)null;
            var searcher = (ManagementObjectSearcher)null;
            try
            {
                if (null == scope)
                {
                    searcher = new ManagementObjectSearcher(query);
                }
                else
                {
                    searcher = new ManagementObjectSearcher(scope, query);
                }
                result = searcher.Get();
            }
            catch (Exception) { }
            if (searcher != null)
            {
                searcher.Dispose();
            }
            return result;
        }

        private void PreparePnpDevID()
        {
            var query = string.Empty;
            var dicDev2PnpDevID = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            query = "SELECT DeviceID, PNPDeviceID FROM Win32_DiskDrive";
            var queryObjects = ExecuteWmiQuery(null, query);
            if (null != queryObjects)
            {
                foreach (ManagementObject queryObj in queryObjects)
                {
                    try
                    {
                        var devID = queryObj.GetPropertyValue("DeviceID").ToString();
                        var pnpDevID = queryObj.GetPropertyValue("PNPDeviceID").ToString();
                        dicDev2PnpDevID[devID] = pnpDevID;
                    }
                    catch (Exception) { }
                }
                queryObjects.Dispose();
            }

            var dicPartitionID2PnpDevID = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in dicDev2PnpDevID)
            {
                var pnpDevID = kv.Value;
                query = "ASSOCIATORS OF {Win32_DiskDrive.DeviceID=\'";
                query += kv.Key;
                query += "\'} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
                queryObjects = ExecuteWmiQuery(null, query);
                if (null != queryObjects)
                {
                    foreach (ManagementObject queryObj in queryObjects)
                    {
                        try
                        {
                            var devID = queryObj.GetPropertyValue("DeviceID").ToString();
                            dicPartitionID2PnpDevID[devID] = pnpDevID;
                        }
                        catch (Exception) { }
                    }
                    queryObjects.Dispose();
                }
                //
            }

            var dicLogicalDisk2PnpDevID = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in dicPartitionID2PnpDevID)
            {
                var pnpDevID = kv.Value;
                query = "ASSOCIATORS OF {Win32_DiskPartition.DeviceID=\'";
                query += kv.Key;
                query += "\'} WHERE AssocClass = Win32_LogicalDiskToPartition";
                queryObjects = ExecuteWmiQuery(null, query);
                if (null != queryObjects)
                {
                    foreach (ManagementObject queryObj in queryObjects)
                    {
                        try
                        {
                            var devID = queryObj.GetPropertyValue("DeviceID").ToString();
                            dicLogicalDisk2PnpDevID[devID] = pnpDevID;
                        }
                        catch (Exception) { }
                    }
                    queryObjects.Dispose();
                }
                //
            }

            var logicalDiskID = this.mRootPath;
            if (logicalDiskID.Length > 2)
            {
                logicalDiskID = logicalDiskID.Substring(0, 2);
            }

            var assocPnpDevID = string.Empty;
            if (!dicLogicalDisk2PnpDevID.TryGetValue(logicalDiskID, out assocPnpDevID))
            {
                assocPnpDevID = null;
            }
            if (null == assocPnpDevID)
            {
                assocPnpDevID = string.Empty;
            }
            this.mPnpDevID = assocPnpDevID;
        }

        private void ThreadRefreshDrive()
        {
            try { this.RefreshDrive(); }
            catch (Exception) { }

            if (this.mIsFinished)
            {
                this.SetProgressBarFinished(this.PrgbTotal);
            }
            this.Stop();
        }

        private void RefreshDrive()
        {
            try { File.Delete(this.mTmpFile); }
            catch (Exception) { }
            this.RefreshDir(1, this.mRootPath);
        }

        private void SetCurrentFile(string path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    this.SetCurrentFile(path);
                }));
                return;
            }
            this.TxtCurrentFile.Text = path;
        }

        private void SetProgressBarMaxValue(ProgressBar prgb, long value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    this.SetProgressBarMaxValue(prgb, value);
                }));
                return;
            }

            prgb.Value = 0;
            var pv = prgb.Tag as ProgressBarValues;
            pv.value = 0;
            pv.max = value;
        }

        private void IncreaseProgressBarValue(ProgressBar prgb, long value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    this.IncreaseProgressBarValue(prgb, value);
                }));
                return;
            }

            var pv = prgb.Tag as ProgressBarValues;
            pv.value += value;

            var percent = (int)((pv.value * 100) / pv.max);
            prgb.Value = percent;
        }

        private void SetProgressBarFinished(ProgressBar prgb)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    this.SetProgressBarFinished(prgb);
                }));
                return;
            }
            prgb.Value = prgb.Maximum;
        }

        private void RefreshDir(int level, string path)
        {
            if (!this.mRunning)
            {
                return;
            }

            var fileList = (string[])null;
            try { fileList = Directory.GetFiles(path); }
            catch (Exception) { }
            if (fileList != null)
            {
                foreach (var fileName in fileList)
                {
                    this.RefreshFile(fileName);
                }
            }

            var nextLvl = level + 1;
            try { fileList = Directory.GetDirectories(path); }
            catch (Exception) { }
            if (fileList != null)
            {
                foreach (var fileName in fileList)
                {
                    this.RefreshDir(nextLvl, fileName);
                }
            }

            if (1 == level && this.mRunning)
            {
                this.mIsFinished = true;
            }
        }

        private static string PrepareLongPath(string path)
        {
            var lPath = "\\\\?\\" + path;
            return lPath;
        }

        private static bool IsValidFileHandle(IntPtr handle)
        {
            var value = handle.ToInt64();
            return (value >= 0);
        }

        private static IntPtr OpenFileForReading(string path)
        {
            var lPath = PrepareLongPath(path);
            var handle = CreateFile(
                lPath,
                0x80000000,
                FileShare.Read,
                IntPtr.Zero,
                FileMode.Open,
                FileAttributes.Normal,
                IntPtr.Zero
            );
            return handle;
        }

        private class HashResults
        {
            public long size = 0;
            public byte[] crc32 = null;
            public byte[] sha256 = null;
        }

        private bool CopyFile(
            IntPtr srcFileHandle,
            long size,
            string dstPath,
            ref FILETIME creationTime,
            HashResults hr
        )
        {
            var lDstPath = dstPath; // PrepareLongPath(dstPath);
            var dstFileHandle = CreateFile(
                lDstPath,
                0x40000000,
                FileShare.Read,
                IntPtr.Zero,
                FileMode.Create,
                FileAttributes.Normal,
                IntPtr.Zero
            );
            if (!IsValidFileHandle(dstFileHandle))
            {
                return false;
            }

            var sha256 = (HashStream)null;
            var crc32 = (HashStream)null;

            var enableHash = (hr != null);
            if (enableHash)
            {
                crc32 = new HashStream(new Force.Crc32.Crc32Algorithm());
                sha256 = new HashStream(new SHA256Managed());
            }

            var offset = (long)0;
            var buf = this.mBuffer;
            var bufLen = (long)buf.Length;
            while (this.mRunning)
            {
                var remain = size - offset;
                if (remain <= 0)
                {
                    break;
                }

                var nWantToRead = remain;
                if (nWantToRead > bufLen)
                {
                    nWantToRead = bufLen;
                }

                var nRead = (uint)0;
                if (!ReadFile(srcFileHandle, buf, (uint)nWantToRead, out nRead, IntPtr.Zero))
                {
                    break;
                }
                if (nRead != nWantToRead)
                {
                    break;
                }

                var nWrite = (uint)0;
                if (!WriteFile(dstFileHandle, buf, nRead, out nWrite, IntPtr.Zero))
                {
                    break;
                }
                if (nWrite != nRead)
                {
                    break;
                }

                if (enableHash)
                {
                    crc32.Write(buf, 0, nRead);
                    sha256.Write(buf, 0, nRead);
                }

                offset += (long)nRead;
                this.mBytesTransfered += (long)nRead;
                this.IncreaseProgressBarValue(this.PrgbCurrentFile, nRead);
            }

            var fileTimeOK = SetFileTime(dstFileHandle, ref creationTime, IntPtr.Zero, IntPtr.Zero);
            CloseHandle(dstFileHandle);

            if (enableHash)
            {
                hr.crc32 = crc32.ComputeFinalHash();
                crc32.Dispose();

                hr.sha256 = sha256.ComputeFinalHash();
                sha256.Dispose();
            }

            var result = (offset == size);
            return result;
        }

        private void WriteFileHashWithAlg(StringBuilder sb, string name, byte[] hash)
        {
            if (null == hash)
            {
                return;
            }

            sb.Append(name);
            sb.Append(": ");
            foreach (var b in hash)
            {
                sb.Append(string.Format("{0:x2}", (uint)b));
            }
            sb.AppendLine();
        }

        private void WriteFileHash(string path, HashResults hr)
        {
            if (null == hr)
            {
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine(path);
            sb.Append("Size: ");
            sb.Append(hr.size.ToString());
            sb.AppendLine();

            WriteFileHashWithAlg(sb, "CRC32", hr.crc32);
            WriteFileHashWithAlg(sb, "SHA256", hr.sha256);

            sb.AppendLine();
            var s = sb.ToString();
            File.AppendAllText(this.mHashFile, s);
        }

        private void CheckHddTemperature()
        {
            var wmiQuery = "SELECT * FROM MSStorageDriver_ATAPISmartData";
            var queryObjects = ExecuteWmiQuery("root\\WMI", wmiQuery);
            if (null != queryObjects)
            {
                try
                {
                    foreach (ManagementObject queryObj in queryObjects)
                    {
                        try
                        {
                            var instanceName = (string)queryObj.GetPropertyValue("InstanceName");
                            var arrVendorSpecific = (byte[])queryObj.GetPropertyValue("VendorSpecific");
                            var temperatureIndex = (int)Array.IndexOf(arrVendorSpecific, TEMPERATURE_ATTRIBUTE);
                            var temperature = (int)arrVendorSpecific[temperatureIndex + 5];
                        }
                        catch (Exception) { }
                    }
                    //
                }
                catch (Exception) { }
                queryObjects.Dispose();
            }
            //
        }

        private void RefreshFile(string path)
        {
            this.CheckHddTemperature();
            while ((this.mIsPaused || this.mIsCoolDown) && this.mRunning)
            {
                Thread.Sleep(250);
            }
            if (!this.mRunning)
            {
                return;
            }

            if (path.Equals(this.mTmpFile, StringComparison.OrdinalIgnoreCase) ||
                path.Equals(this.mHashFile, StringComparison.OrdinalIgnoreCase)
            )
            {
                return;
            }

            if (path.IndexOf(":\\$RECYCLE.BIN", StringComparison.OrdinalIgnoreCase) > 0 ||
                path.IndexOf(":\\System Volume Information", StringComparison.OrdinalIgnoreCase) > 0 ||
                path.EndsWith("\\desktop.ini", StringComparison.OrdinalIgnoreCase)
            )
            {
                return;
            }

            this.SetCurrentFile(path);

            var handle = OpenFileForReading(path);
            var ok = IsValidFileHandle(handle);

            var size = (long)0;
            if (ok)
            {
                ok = GetFileSizeEx(handle, out size);
                if (!ok)
                {
                    size = 0;
                }
            }

            this.SetProgressBarMaxValue(this.PrgbCurrentFile, size);

            var hr = (HashResults)null;
            var enableHash = (size >= this.mMinHashFileSize);
            if (enableHash)
            {
                hr = new HashResults();
                hr.size = size;
            }

            var fileYearsOld = this.mMaxFileYearsOld;
            if (fileYearsOld >= 0)
            {
                fileYearsOld = this.mCurrentYear - fileYearsOld;
            }

            var fileTimeOK = false;
            var creationTime = new FILETIME();
            var lastAccessTime = new FILETIME();
            var lastWriteTime = new FILETIME();
            var lastWriteSystemTime = new SYSTEMTIME();
            if (ok)
            {
                while (true)
                {
                    fileTimeOK = GetFileTime(handle, ref creationTime, ref lastAccessTime, ref lastWriteTime);
                    if (!fileTimeOK)
                    {
                        break;
                    }
                    if (fileYearsOld < 0)
                    {
                        break;
                    }

                    fileTimeOK = FileTimeToSystemTime(ref lastWriteTime, out lastWriteSystemTime);
                    if (!fileTimeOK)
                    {
                        break;
                    }
                    if (lastWriteSystemTime.Year >= fileYearsOld)
                    {
                        ok = false;
                        break;
                    }
                    break;
                }
                //
            }

            if (ok)
            {
                ok = this.CopyFile(handle, size, this.mTmpFile, ref creationTime, hr);
            }
            CloseHandle(handle);

            if (ok)
            {
                var lPath = PrepareLongPath(path);
                ok = MoveFileEx(this.mTmpFile, lPath, 1 | 8);
            }

            if (ok && enableHash)
            {
                this.WriteFileHash(path, hr);
            }

            if (ok)
            {
                //Thread.Sleep(1000);
                this.SetProgressBarFinished(this.PrgbCurrentFile);
            }
            this.IncreaseProgressBarValue(this.PrgbTotal, size);
        }

        //
    }
}
