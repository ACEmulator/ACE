using System.ComponentModel;
using System.Linq;

namespace ACE.Entity.Enum.Properties
{
    public enum PropertyAttribute2nd : ushort
    {
        Undef       = 0,

        [AttributeFormula(AttributeCache.Endurance, 2)]
        MaxHealth   = 1,
        Health      = 2,

        [AttributeFormula(AttributeCache.Endurance)]
        MaxStamina  = 3,
        Stamina     = 4,

        [AttributeFormula(AttributeCache.Self)]
        MaxMana     = 5,
        Mana        = 6
    }

    public static class PropertyAttribute2ndExtensions
    {
        public static string GetDescription(this PropertyAttribute2nd prop)
        {
            var description = prop.GetAttributeOfType<DescriptionAttribute>();
            return description?.Description ?? prop.ToString();
        }

        public static AttributeFormulaAttribute GetFormula(this PropertyAttribute2nd attribute)
        {
            return attribute.GetAttributeOfType<AttributeFormulaAttribute>();
        }

        /// <summary>
        /// Will add a space infront of capital letter words in a string
        /// </summary>
        /// <param name="attribute2nd"></param>
        /// <returns>string with spaces infront of capital letters</returns>
        public static string ToSentence(this PropertyAttribute2nd attribute2nd)
        {
            return new string(attribute2nd.ToString().Replace("Max", "Maximum").ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray());
        }
    }
}
