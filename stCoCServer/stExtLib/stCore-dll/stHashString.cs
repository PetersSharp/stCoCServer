using System;
using System.Text;
using System.Security.Cryptography;

namespace stCore
{
    public static class stHashString
    {
        public static string ToMD5(this string src)
        {
            return stHashString._ToHash(src, "MD5");
        }
        public static string ToSHA1(this string src)
        {
            return stHashString._ToHash(src, "SHA1");
        }
        private static string _ToHash(string src, string type)
        {
            try
            {
                return BitConverter.ToString(
                    ((HashAlgorithm)CryptoConfig.CreateFromName(type)).ComputeHash(new UTF8Encoding().GetBytes(src))
                );
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}
