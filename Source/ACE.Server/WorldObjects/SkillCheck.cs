using System;

namespace ACE.Server.WorldObjects
{
    public static class SkillCheck
    {
        public static double GetSkillChance(int skill, int difficulty, float factor = 0.03f)
        {
            var chance = 1.0 - (1.0 / (1.0 + Math.Exp(factor * (skill - difficulty))));

            return Math.Min(1.0, Math.Max(0.0, chance));
        }

        public static double GetSkillChance(uint skill, uint difficulty, float factor = 0.03f)
        {
            return GetSkillChance((int)skill, (int)difficulty, factor);
        }

        public static double GetMagicSkillChance(int skill, int difficulty)
        {
            return GetSkillChance(skill, difficulty, 0.07f);
        }

        public static string FormatChance(this double chance)
        {
            if (chance == 1) return "100%";
            if (chance == 0) return "0%";
            var r = (chance * 100);
            var p = r.ToString("F99").TrimEnd('0');
            if (!p.StartsWith("0."))
            {
                var extra = 2;
                if (p.IndexOf(".0") > -1 || p.EndsWith('.')) extra = 0;
                return p.Substring(0, p.IndexOf('.') + extra) + "%";
            }
            var i = p.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
            if (i < 0) return "0%";
            return p.Substring(0, i + 1) + "%";
        }
    }
}
