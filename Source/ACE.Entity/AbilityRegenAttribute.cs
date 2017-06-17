using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
