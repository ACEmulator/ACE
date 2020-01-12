namespace ACE.Server.Entity
{
    public class BaseDamage
    {
        public int MaxDamage;

        public float Variance;

        public float MinDamage => MaxDamage * (1.0f - Variance);

        public BaseDamage(int maxDamage, float variance)
        {
            MaxDamage = maxDamage;
            Variance = variance;
        }
    }
}
