using System;

using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class AbilityVitalAttribute : Attribute
    {
        public AbilityVitalAttribute(Vital vital)
        {
            Vital = vital;
        }

        public Vital Vital { get; }
    }
}
