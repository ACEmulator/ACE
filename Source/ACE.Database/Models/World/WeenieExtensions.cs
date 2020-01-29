using System;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.Models.World
{
    public static class WeenieExtensions
    {
        // =====================================
        // Get
        // Bool, DID, Float, IID, Int, Int64, String, Position
        // =====================================

        public static bool? GetProperty(this Weenie weenie, PropertyBool property)
        {
            return weenie.WeeniePropertiesBool.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static uint? GetProperty(this Weenie weenie, PropertyDataId property)
        {
            return weenie.WeeniePropertiesDID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static double? GetProperty(this Weenie weenie, PropertyFloat property)
        {
            return weenie.WeeniePropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property)?.Value;
        }

        public static uint? GetProperty(this Weenie weenie, PropertyInstanceId property)
        {
            return weenie.WeeniePropertiesIID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static int? GetProperty(this Weenie weenie, PropertyInt property)
        {
            return weenie.WeeniePropertiesInt.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static long? GetProperty(this Weenie weenie, PropertyInt64 property)
        {
            return weenie.WeeniePropertiesInt64.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static string GetProperty(this Weenie weenie, PropertyString property)
        {
            return weenie.WeeniePropertiesString.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static WeeniePropertiesPosition GetProperty(this Weenie weenie, PositionType positionType)
        {
            return weenie.WeeniePropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);
        }

        public static Position GetPosition(this Weenie weenie, PositionType positionType)
        {
            var result = weenie.WeeniePropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);

            if (result == null)
                return null;

            return new Position(result.ObjCellId, result.OriginX, result.OriginY, result.OriginZ, result.AnglesX, result.AnglesY, result.AnglesZ, result.AnglesW);
        }


        // =====================================
        // Set
        // Bool, DID, Float, IID, Int, Int64, String, Position
        // =====================================

        // todo

        // =====================================
        // Remove
        // Bool, DID, Float, IID, Int, Int64, String, Position
        // =====================================

        // todo


        // =====================================
        // Get
        // All Else
        // =====================================

        public static WeeniePropertiesSpellBook GetSpell(this Weenie weenie, int spell)
        {
            return weenie.WeeniePropertiesSpellBook.FirstOrDefault(x => x.Spell == spell);
        }


        public static uint? GetAnimationId(this Weenie weenie, byte index)
        {
            return weenie.WeeniePropertiesAnimPart.FirstOrDefault(x => x.Index == index)?.AnimationId;
        }

        public static WeeniePropertiesPalette GetPalette(this Weenie weenie, uint subPaletteId)
        {
            return weenie.WeeniePropertiesPalette.FirstOrDefault(x => x.SubPaletteId == subPaletteId);
        }

        public static WeeniePropertiesTextureMap GetTextureMap(this Weenie weenie, byte index)
        {
            return weenie.WeeniePropertiesTextureMap.FirstOrDefault(x => x.Index == index);
        }


        public static WeeniePropertiesCreateList GetCreateList(this Weenie weenie, sbyte destinationType)
        {
            return weenie.WeeniePropertiesCreateList.FirstOrDefault(x => x.DestinationType == destinationType);
        }

        public static WeeniePropertiesEmote GetEmote(this Weenie weenie, uint category)
        {
            return weenie.WeeniePropertiesEmote.FirstOrDefault(x => x.Category == category);
        }

        public static WeeniePropertiesEventFilter GetEventFilter(this Weenie weenie, int eventId)
        {
            return weenie.WeeniePropertiesEventFilter.FirstOrDefault(x => x.Event == eventId);
        }

        // WeeniePropertiesGenerator


        public static WeeniePropertiesAttribute GetProperty(this Weenie weenie, PropertyAttribute property)
        {
            return weenie.WeeniePropertiesAttribute.FirstOrDefault(x => x.Type == (uint)property);
        }

        public static WeeniePropertiesAttribute2nd GetProperty(this Weenie weenie, PropertyAttribute2nd property)
        {
            return weenie.WeeniePropertiesAttribute2nd.FirstOrDefault(x => x.Type == (uint)property);
        }

        public static WeeniePropertiesBodyPart GetBodyPart(this Weenie weenie, ushort key)
        {
            return weenie.WeeniePropertiesBodyPart.FirstOrDefault(x => x.Key == key);
        }

        public static WeeniePropertiesSkill GetProperty(this Weenie weenie, Skill skill)
        {
            return weenie.WeeniePropertiesSkill.FirstOrDefault(x => x.Type == (uint)skill);
        }


        public static WeeniePropertiesBookPageData GetBookPageData(this Weenie weenie, uint pageId)
        {
            return weenie.WeeniePropertiesBookPageData.FirstOrDefault(x => x.PageId == pageId);
        }
    }
}
