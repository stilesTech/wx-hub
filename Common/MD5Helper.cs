using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class MD5Helper
    {
        public static string ComputeHash(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            str = string.Empty;

            for (int i = 0; i < s.Length; i++)
            {
                str = str + s[i].ToString("x2");
            }

            return str;
        }

        public static string ComputeHash16(string str)
        {
            return ComputeHash(str).Substring(8, 16);
        }
    }
}
