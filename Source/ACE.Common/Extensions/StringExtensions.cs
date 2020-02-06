using System;

namespace ACE.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool StartsWithVowel(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            char firstLetter = s.ToLower()[0];
            bool isVowel = "aeiou".IndexOf(firstLetter) >= 0;

            return isVowel;
        }

        /// <summary>
        /// For objects that don't have a PropertyString.PluralName
        /// </summary>
        public static string Pluralize(this string name)
        {
            if (name.EndsWith("ch") || name.EndsWith("s") || name.EndsWith("sh") || name.EndsWith("x") || name.EndsWith("z"))
                return name + "es";
            else
                return name + "s";
        }

        /// <summary>
        /// Removes a string from the beginning of a string
        /// </summary>
        public static string TrimStart(this string result, string trimStart)
        {
            if (result.StartsWith(trimStart, StringComparison.OrdinalIgnoreCase))
                result = result.Substring(trimStart.Length);

            return result;
        }

        /// <summary>
        /// Removes a string from the end of a string
        /// </summary>
        public static string TrimEnd(this string result, string trimEnd)
        {
            if (result.EndsWith(trimEnd, StringComparison.OrdinalIgnoreCase))
                result = result.Substring(0, result.Length - trimEnd.Length);

            return result;
        }
    }
}
