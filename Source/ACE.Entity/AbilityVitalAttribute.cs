using ACE.Entity.Enum;
using System;

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
