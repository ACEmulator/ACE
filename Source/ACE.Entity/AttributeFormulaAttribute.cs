using System;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AttributeFormulaAttribute : Attribute
    {
        public PropertyAttribute Attribute { get; set; }
        public PropertyAttribute2nd Attribute2nd { get; set; }

        public AttributeCache AttributeCache { get; set; }

        public byte Divisor { get; set; }

        public byte AbilityMultiplier { get; set; } = 1;

        public AttributeFormulaAttribute(PropertyAttribute attribute)
            : this(attribute, 1)
        {
        }

        public AttributeFormulaAttribute(PropertyAttribute attribute, byte divisor)
        {
            if (divisor == 0)
                throw new ArgumentException("0 not a valid value for " + nameof(divisor));

            Attribute = attribute;
            Divisor = divisor;
        }

        public AttributeFormulaAttribute(PropertyAttribute2nd attribute2nd)
            : this(attribute2nd, 1)
        {
        }

        public AttributeFormulaAttribute(PropertyAttribute2nd attribute2nd, byte divisor)
        {
            if (divisor == 0)
                throw new ArgumentException("0 not a valid value for " + nameof(divisor));

            Attribute2nd = attribute2nd;
            Divisor = divisor;
        }

        public AttributeFormulaAttribute(AttributeCache attributeCache)
            : this(attributeCache, 1)
        {
        }

        public AttributeFormulaAttribute(AttributeCache attributeCache, byte divisor)
        {
            if (divisor == 0)
                throw new ArgumentException("0 not a valid value for " + nameof(divisor));

            AttributeCache = attributeCache;
            Divisor = divisor;
        }

        public uint CalcBase(uint strength, uint endurance, uint coordination, uint quickness, uint focus, uint self)
        {
            uint sum = 0;

            if (((uint)AttributeCache & (uint)AttributeCache.Strength) != 0)
                sum += strength;

            if (((uint)AttributeCache & (uint)AttributeCache.Endurance) != 0)
                sum += endurance;

            if (((uint)AttributeCache & (uint)AttributeCache.Coordination) != 0)
                sum += coordination;

            if (((uint)AttributeCache & (uint)AttributeCache.Quickness) != 0)
                sum += quickness;

            if (((uint)AttributeCache & (uint)AttributeCache.Focus) != 0)
                sum += focus;

            if (((uint)AttributeCache & (uint)AttributeCache.Self) != 0)
                sum += self;

            return (uint)Math.Round((double)(sum * AbilityMultiplier) / Divisor);
        }
    }
}
