using System;
using System.Security.Cryptography;
using System.Text;

namespace TianQin365.Common.Encrypt
{
    public class Hash
    {
        /// <summary>
        /// Hash加密字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns>加密后字符,加密的salt</returns>
        public Tuple<string, string> Create(string text)
        {
            var salt = GenerateSalt();
            var txt = EncodeText(text, salt);

            return Tuple.Create<string, string>(txt, salt);
        }

        /// <summary>
        /// 明文字符串与加密后的字符串校验
        /// </summary>
        /// <param name="textEncrypted">加密后的字符串</param>
        /// <param name="salt">加密字符串的salt</param>
        /// <param name="text">明文字符串</param>
        /// <returns></returns>
        public bool Check(string textEncrypted, string salt, string text)
        {
            var encodedText = EncodeText(text, salt);

            return textEncrypted.Equals(encodedText);
        }

        public string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public string EncodeText(string text, string salt)
        {
            byte[] bIn = Encoding.Unicode.GetBytes(text);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bRet = null;

            // MembershipPasswordFormat.Hashed
            HashAlgorithm hm = HashAlgorithm.Create("SHA1"); // MD5
            if (hm is KeyedHashAlgorithm)
            {
                KeyedHashAlgorithm kha = (KeyedHashAlgorithm)hm;
                if (kha.Key.Length == bSalt.Length)
                {
                    kha.Key = bSalt;
                }
                else if (kha.Key.Length < bSalt.Length)
                {
                    byte[] bKey = new byte[kha.Key.Length];
                    Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
                    kha.Key = bKey;
                }
                else
                {
                    byte[] bKey = new byte[kha.Key.Length];
                    for (int iter = 0; iter < bKey.Length;)
                    {
                        int len = Math.Min(bSalt.Length, bKey.Length - iter);
                        Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
                        iter += len;
                    }
                    kha.Key = bKey;
                }
                bRet = kha.ComputeHash(bIn);
            }
            else
            {
                byte[] bAll = new byte[bSalt.Length + bIn.Length];
                Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
                Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
                bRet = hm.ComputeHash(bAll);
            }

            return Convert.ToBase64String(bRet);
        }
    }
}
