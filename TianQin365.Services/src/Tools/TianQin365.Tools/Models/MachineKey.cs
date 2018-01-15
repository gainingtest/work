using System.Security.Cryptography;
using System.Text;

namespace TianQin365.Tools.Models
{
    public class MachineKey
    {
        public static string Create(int len)
        {
            byte[] bytes = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(bytes);

            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                sb.Append(string.Format("{0:X2}", bytes[i]));

            return sb.ToString();
        }
    }
}
