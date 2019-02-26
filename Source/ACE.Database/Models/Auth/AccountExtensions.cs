using ACE.Common.Cryptography;

namespace ACE.Database.Models.Auth
{
    public static class AccountExtensions
    {
        public static bool PasswordMatches(this Account account, string password)
        {
            return BCryptProvider.Verify(password, account.PasswordHash);
        }

        public static void SetPassword(this Account account, string value)
        {
            account.PasswordHash = GetPasswordHash(value);
        }

        private static string GetPasswordHash(string password)
        {
            return BCryptProvider.HashPassword(password);
        }
    }
}
