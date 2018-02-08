using ACE.Entity.Enum;
using System;

namespace ACE.Entity
{
    public class AbilityVitalAttribute : Attribute
    {
        private Vital _vital;

        public AbilityVitalAttribute(Vital vital)
        {
            _vital = vital;
        }

        public Vital Vital { get { return _vital; } }
    }
}
