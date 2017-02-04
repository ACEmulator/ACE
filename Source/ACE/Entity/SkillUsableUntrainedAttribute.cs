using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SkillUsableUntrainedAttribute : Attribute
    {
        public SkillUsableUntrainedAttribute(bool usableUntrained)
        {
            UsableUntrained = usableUntrained;
        }

        public bool UsableUntrained { get; set; }
    }
}
