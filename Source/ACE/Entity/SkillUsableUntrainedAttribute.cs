using System;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SkillUsableUntrainedAttribute : Attribute
    {
        public bool UsableUntrained { get; set; }

        public SkillUsableUntrainedAttribute(bool usableUntrained)
        {
            UsableUntrained = usableUntrained;
        }
    }
}
