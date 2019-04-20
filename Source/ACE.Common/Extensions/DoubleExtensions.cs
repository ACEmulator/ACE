namespace ACE.Common.Extensions
{
    public static class DoubleExtensions
    {
        public static string FormatChance(this double chance)
        {
            if (chance == 1)
            {
                return "100%";
            }
            if (chance == 0)
            {
                return "0%";
            }
            double r = (chance * 100);
            string p = r.ToString("F99").TrimEnd('0');
            if (!p.StartsWith("0."))
            {
                int extra = 2;
                if (p.IndexOf(".0") > -1 || p.EndsWith("."))
                {
                    extra = 0;
                }
                return p.Substring(0, p.IndexOf('.') + extra) + "%";
            }
            int i = p.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            if (i < 0)
            {
                return "0%";
            }
            return p.Substring(0, i + 1) + "%";
        }
    }
}
