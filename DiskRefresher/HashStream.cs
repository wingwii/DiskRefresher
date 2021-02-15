using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DiskRefresher
{
    class HashStream : IDisposable
    {
        private HashAlgorithm mAlg = null;
        private DummyStream mOutStrm = null;
        private CryptoStream mCryptoStrm = null;

        public HashStream(HashAlgorithm alg)
        {
            this.mAlg = alg;
            this.mOutStrm = new DummyStream();
            this.mCryptoStrm = new CryptoStream(this.mOutStrm, alg, CryptoStreamMode.Write);
        }

        public void Dispose()
        {
            this.mCryptoStrm.Dispose();
            this.mOutStrm.Dispose();
            this.mAlg.Dispose();
        }

        public void Write(byte[] buf, int offset, uint size)
        {
            this.mCryptoStrm.Write(buf, offset, (int)size);
        }

        public byte[] ComputeFinalHash()
        {
            this.mCryptoStrm.FlushFinalBlock();
            this.mCryptoStrm.Close();
            this.mOutStrm.Close();
            var result = this.mAlg.Hash;
            return result;
        }

        //
    }
}
