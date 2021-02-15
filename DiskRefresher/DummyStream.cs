using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiskRefresher
{
    class DummyStream : Stream
    {
        private long mPos = 0;

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return this.mPos; }
        }

        public override long Position
        {
            get { return this.mPos; }
            set { }
        }

        public override void Flush()
        {
            //
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return 0;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.mPos += count;
        }

        //
    }
}
