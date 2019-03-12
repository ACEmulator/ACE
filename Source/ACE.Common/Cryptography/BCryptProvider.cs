namespace ACE.Common.Cryptography
{
    public static class BCryptProvider
    {
        public static string HashPassword(string input, int workFactor = 10)
        {
            return BCrypt.Net.BCrypt.HashPassword(input, workFactor, BCrypt.Net.SaltRevision.Revision2Y);
        }

        public static bool Verify(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }

        public static int GetPasswordWorkFactor(string hash)
        {
            return BCrypt.Net.BCrypt.GetPasswordWorkFactor(hash);
        }
    }
}
