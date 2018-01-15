using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TianQin365.Common.Encrypt
{
    public class TripleDES
    {
        private byte[] _iv = new byte[] { 0xa7, 0xc7, 0x1b, 0xd8, 0x6f, 0x4b, 0xe6, 0x81 };
        private TripleDESCryptoServiceProvider _desCSP = new TripleDESCryptoServiceProvider();

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string Encrypt(string str, string key)
        {
            var d = null as string;
            using (MemoryStream ms = new MemoryStream())
            {
                var keys = Encoding.UTF8.GetBytes(key);
                CryptoStream cryStream = new CryptoStream(ms, _desCSP.CreateEncryptor(keys, this._iv), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cryStream);
                sw.Write(str);
                sw.Close();

                d = Convert.ToBase64String(ms.ToArray());
                cryStream.Close();
            }
            return d;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public string Decrypt(string str, string key)
        {
            var d = null as string;
            var datas = Convert.FromBase64String(str);
            using (MemoryStream ms = new MemoryStream(datas))
            {
                var keys = Encoding.UTF8.GetBytes(key);
                CryptoStream cryStream = new CryptoStream(ms, _desCSP.CreateDecryptor(keys, this._iv), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cryStream);

                d = sr.ReadToEnd();

                sr.Close();
                cryStream.Close();
            }
            return d;
        }
    }
}
