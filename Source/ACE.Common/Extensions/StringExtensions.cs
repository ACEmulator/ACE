namespace ACE.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool StartsWithVowel(this string s)
        {
            if (s == null || s.Length == 0)
                return false;

            char firstLetter = s.ToLower()[0];
            bool isVowel = "aeiou".IndexOf(firstLetter) >= 0;

            return isVowel;
        }
    }
}
