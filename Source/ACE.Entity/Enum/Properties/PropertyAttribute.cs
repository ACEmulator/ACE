using System.ComponentModel;

namespace ACE.Entity.Enum.Properties
{
    // The order of quickness and coordination corresponds to the client
    public enum PropertyAttribute : ushort
    {
        Undef        = 0,
        Strength     = 1,
        Endurance    = 2,
        Quickness    = 3,
        Coordination = 4,
        Focus        = 5,
        Self         = 6,
        Health       = 7,
        Stamina      = 8,
        Mana         = 9
    }

    public static class PropertyAttributeExtensions
    {
        public static string GetDescription(this PropertyAttribute prop)
        {
            var description = EnumHelper.GetAttributeOfType<DescriptionAttribute>(prop);
            return description?.Description ?? prop.ToString();
        }
    }
}
