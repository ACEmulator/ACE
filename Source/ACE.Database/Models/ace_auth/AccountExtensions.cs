using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ACE.Database.Models.ace_auth
{
    public static class AccountExtensions
    {
        /// <summary>
        /// creates a new account object and pre-creates a new, random salt
        /// </summary>
        public static void CreateRandomSalt(this Account account)
        {
            byte[] salt = new byte[64]; // 64 bytes = 512 bits, ideal for use with SHA512

            using (var salter = new RNGCryptoServiceProvider())
                salter.GetNonZeroBytes(salt);

            account.PasswordSalt = Convert.ToBase64String(salt);
        }

        public static bool PasswordMatches(this Account account, string password)
        {
            var input = GetPasswordHash(account, password);
            return input == account.PasswordHash;
        }

        public static void SetPassword(this Account account, string value)
        {
            account.PasswordHash = GetPasswordHash(account, value);
        }

        private static string GetPasswordHash(Account account, string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(account.PasswordSalt);
            byte[] buffer = passwordBytes.Concat(saltBytes).ToArray();
            byte[] hash;

            using (SHA512Managed hasher = new SHA512Managed())
                hash = hasher.ComputeHash(buffer);

            return Convert.ToBase64String(hash);
        }
    }
}
