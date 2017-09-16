using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
