using System.ComponentModel;

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
        public static string GetDescription(this PropertyAttribute prop)
        {
            var description = prop.GetAttributeOfType<DescriptionAttribute>();
            return description?.Description ?? prop.ToString();
        }
    }
}
