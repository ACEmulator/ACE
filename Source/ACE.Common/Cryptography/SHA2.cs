using System;
using System.Security.Cryptography;
using System.Text;

namespace ACE.Common.Cryptography
{
    public enum SHA2Type
    {
        SHA256,
        SHA512
    }

    public static class SHA2
    {
        public static string Hash(SHA2Type type, string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);

            try
            {
                byte[] digest;
                switch (type)
                {
                    case SHA2Type.SHA256:
                        digest = SHA256.Create().ComputeHash(buffer);
                        break;
                    case SHA2Type.SHA512:
                        digest = SHA512.Create().ComputeHash(buffer);
                        break;
                    default:
                        return "";
                }

                return BitConverter.ToString(digest).Replace("-", "").ToLower();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return "";
            }
        }
    }
}
