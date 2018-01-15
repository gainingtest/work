using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TianQin365.Common.Tests
{
    [TestClass]
    public class TripleDESTests
    {
        [TestMethod]
        public void ConvertIVtoBase64()
        {
            var iv = new byte[8];

            new RNGCryptoServiceProvider().GetBytes(iv);

            var base64 = Convert.ToBase64String(iv);
        }
    }
}
