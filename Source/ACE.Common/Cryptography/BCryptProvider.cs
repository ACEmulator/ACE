namespace ACE.Common.Cryptography
{
    public static class BCryptProvider
    {
        public static string HashPassword(string input, int workFactor = 10)
        {
            // Force BCrypt.Net-Next to use 2y instead of the default 2a
            // The older bcrypt package ACE used (BCrypt.Net-Core) defaultd to 2y
            // Reference: https://stackoverflow.com/questions/49878948/hashing-password-with-2y-identifier/75114685
            string salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor, 'y');

            return BCrypt.Net.BCrypt.HashPassword(input, salt);
        }

        public static bool Verify(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }

        public static int GetPasswordWorkFactor(string hash)
        {
            var hashInformation = BCrypt.Net.BCrypt.InterrogateHash(hash);

            if (int.TryParse(hashInformation.WorkFactor, out var workFactor))
                return workFactor;

            return 0;
        }
    }
}
