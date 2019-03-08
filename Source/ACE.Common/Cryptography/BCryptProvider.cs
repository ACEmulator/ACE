namespace ACE.Common.Cryptography
{
    public static class BCryptProvider
    {
        private const int WorkFactor = 10;

        public static string HashPassword(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input, WorkFactor, BCrypt.Net.SaltRevision.Revision2Y);
        }

        public static bool Verify(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }
    }
}
