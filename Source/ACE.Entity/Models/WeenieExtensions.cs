using System;

using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Entity.Models
{
    public static class WeenieExtensions
    {
        // =====================================
        // Get
        // Bool, DID, Float, IID, Int, Int64, String, Position
        // =====================================

        public static bool? GetProperty(this Weenie weenie, PropertyBool property)
        {
            if (weenie.PropertiesBool == null)
                return null;

            if (weenie.PropertiesBool.TryGetValue(property, out var value))
                return value;

            return null;
        }

        public static uint? GetProperty(this Weenie weenie, PropertyDataId property)
        {
            if (weenie.PropertiesDID == null)
                return null;

            if (weenie.PropertiesDID.TryGetValue(property, out var value))
                return value;

            return null;
        }

        public static double? GetProperty(this Weenie weenie, PropertyFloat property)
        {
            if (weenie.PropertiesFloat == null)
                return null;

            if (weenie.PropertiesFloat.TryGetValue(property, out var value))
                return value;

            return null;
        }

        public static uint? GetProperty(this Weenie weenie, PropertyInstanceId property)
        {
            if (weenie.PropertiesIID == null)
                return null;

            if (weenie.PropertiesIID.TryGetValue(property, out var value))
                return value;

            return null;
        }

        public static int? GetProperty(this Weenie weenie, PropertyInt property)
        {
            if (weenie.PropertiesInt == null)
                return null;

            if (weenie.PropertiesInt.TryGetValue(property, out var value))
                return value;

            return null;
        }

        public static long? GetProperty(this Weenie weenie, PropertyInt64 property)
        {
            if (weenie.PropertiesInt64 == null)
                return null;

            if (weenie.PropertiesInt64.TryGetValue(property, out var value))
                return value;

            return null;
        }

        public static string GetProperty(this Weenie weenie, PropertyString property)
        {
            if (weenie.PropertiesString == null)
                return null;

            if (weenie.PropertiesString.TryGetValue(property, out var value))
                return value;

            return null;
        }

        public static PropertiesPosition GetProperty(this Weenie weenie, PositionType property)
        {
            if (weenie.PropertiesPosition == null)
                return null;

            if (weenie.PropertiesPosition.TryGetValue(property, out var value))
                return value;

            return null;
        }

        public static Position GetPosition(this Weenie weenie, PositionType property)
        {
            if (weenie.PropertiesPosition == null)
                return null;

            if (weenie.PropertiesPosition.TryGetValue(property, out var value))
                return new Position(value.ObjCellId, value.PositionX, value.PositionY, value.PositionZ, value.RotationX, value.RotationY, value.RotationZ, value.RotationW);

            return null;
        }


        // =====================================
        // Utility
        // =====================================

        public static string GetName(this Weenie weenie)
        {
            var name = weenie.GetProperty(PropertyString.Name);

            return name;
        }

        public static string GetPluralName(this Weenie weenie)
        {
            var pluralName = weenie.GetProperty(PropertyString.PluralName);

            if (pluralName == null)
                pluralName = weenie.GetProperty(PropertyString.Name).Pluralize();

            return pluralName;
        }

        public static bool IsStackable(this Weenie weenie)
        {
            switch (weenie.WeenieType)
            {
                case WeenieType.Stackable:

                case WeenieType.Ammunition:
                case WeenieType.Coin:
                case WeenieType.CraftTool:
                case WeenieType.Food:
                case WeenieType.Gem:
                case WeenieType.Missile:
                case WeenieType.SpellComponent:

                    return true;
            }
            return false;
        }

        public static bool IsStuck(this Weenie weenie)
        {
            return weenie.GetProperty(PropertyBool.Stuck) ?? false;
        }

        public static bool RequiresBackpackSlotOrIsContainer(this Weenie weenie)
        {
            var requiresBackPackSlot = weenie.GetProperty(PropertyBool.RequiresBackpackSlot) ?? false;

            return requiresBackPackSlot || weenie.WeenieType == WeenieType.Container;
        }

        public static bool IsVendorService(this Weenie weenie)
        {
            return weenie.GetProperty(PropertyBool.VendorService) ?? false;
        }
    }
}
