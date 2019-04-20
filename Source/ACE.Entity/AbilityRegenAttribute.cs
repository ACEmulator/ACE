using System;

namespace ACE.Entity
{
    public class AbilityRegenAttribute : Attribute
    {
        public double Rate { get; set; }

        // FIXME(ddevec): Make based off stats?
        public AbilityRegenAttribute(double rate)
        {
            Rate = rate;
        }
    }
}
