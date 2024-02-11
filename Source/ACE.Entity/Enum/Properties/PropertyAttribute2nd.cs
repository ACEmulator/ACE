using System.ComponentModel;
using System.Linq;

namespace ACE.Entity.Enum.Properties
{
    public enum PropertyAttribute2nd : ushort
    {
        Undef       = 0,
        MaxHealth   = 1,
        Health      = 2,
        MaxStamina  = 3,
        Stamina     = 4,
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

        /// <summary>
        /// Will add a space infront of capital letter words in a string
        /// </summary>
        /// <param name="attribute2nd"></param>
        /// <returns>string with spaces infront of capital letters</returns>
        public static string ToSentence(this PropertyAttribute2nd attribute2nd)
        {
            switch (attribute2nd)
            {
                case PropertyAttribute2nd.Undef:
                    return "Undef";
                case PropertyAttribute2nd.MaxHealth:
                    return "Maximum Health";
                case PropertyAttribute2nd.Health:
                    return "Health";
                case PropertyAttribute2nd.MaxStamina:
                    return "Maximum Stamina";
                case PropertyAttribute2nd.Stamina:
                    return "Stamina";
                case PropertyAttribute2nd.MaxMana:
                    return "Maximum Mana";
                case PropertyAttribute2nd.Mana:
                    return "Mana";
            }

            // TODO we really should log this as a warning to indicate that we're missing a case up above, and that the inefficient (GC unfriendly) line below will be used
            return new string(attribute2nd.ToString().Replace("Max", "Maximum").ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray());
        }
    }
}
