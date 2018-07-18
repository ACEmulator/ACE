namespace ACE.Entity.Enum
{
    public enum AttackHeight
    {
        High    = 1,
        Medium  = 2,
        Low     = 3
    }

    public static class AttackHeightExtensions
    {
        public static string GetString(this AttackHeight attackHeight)
        {
            switch (attackHeight)
            {
                case AttackHeight.High:   return "High";
                case AttackHeight.Medium: return "Med";
                case AttackHeight.Low:    return "Low";
            }
            return null;
        }
    }
}
