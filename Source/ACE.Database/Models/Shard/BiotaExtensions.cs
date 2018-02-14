using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.Models.Shard
{
    public static class BiotaExtensions
    {
        public static uint? GetAnimationId(this Biota biota, byte index)
        {
            return biota.BiotaPropertiesAnimPart.FirstOrDefault(x => x.Index == index)?.AnimationId;
        }

        public static BiotaPropertiesAttribute GetAttribute(this Biota biota, Ability ability)
        {
            return biota.BiotaPropertiesAttribute.FirstOrDefault(x => x.Type == (uint)ability);
        }

        public static BiotaPropertiesAttribute2nd GetAttribute2nd(this Biota biota, Ability ability)
        {
            return biota.BiotaPropertiesAttribute2nd.FirstOrDefault(x => x.Type == (uint)ability);
        }

        public static BiotaPropertiesBodyPart GetBodyPart(this Biota biota, ushort key)
        {
            return biota.BiotaPropertiesBodyPart.FirstOrDefault(x => x.Key == key);
        }

        public static BiotaPropertiesBookPageData GetBookPageData(this Biota biota, uint pageId)
        {
            return biota.BiotaPropertiesBookPageData.FirstOrDefault(x => x.PageId == pageId);
        }

        public static bool? GetProperty(this Biota biota, PropertyBool property)
        {
            return biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static BiotaPropertiesContract GetContract(this Biota biota, uint contractId)
        {
            return biota.BiotaPropertiesContract.FirstOrDefault(x => x.ContractId == contractId);
        }

        public static BiotaPropertiesCreateList GetCreateList(this Biota biota, sbyte destinationType)
        {
            return biota.BiotaPropertiesCreateList.FirstOrDefault(x => x.DestinationType == destinationType);
        }

        public static uint? GetProperty(this Biota biota, PropertyDataId property)
        {
            return biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static BiotaPropertiesEmote GetEmote(this Biota biota, uint category)
        {
            return biota.BiotaPropertiesEmote.FirstOrDefault(x => x.Category == category);
        }

        // BiotaPropertiesEmoteAction

        public static BiotaPropertiesEventFilter GetEventFilter(this Biota biota, int eventId)
        {
            return biota.BiotaPropertiesEventFilter.FirstOrDefault(x => x.Event == eventId);
        }

        public static double? GetProperty(this Biota biota, PropertyDouble property)
        {
            return biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property)?.Value;
        }

        // BiotaPropertiesFriendList

        // BiotaPropertiesGenerator

        public static int? GetProperty(this Biota biota, PropertyInstanceId property)
        {
            return biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static int? GetProperty(this Biota biota, PropertyInt property)
        {
            return biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static long? GetProperty(this Biota biota, PropertyInt64 property)
        {
            return biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static BiotaPropertiesPalette GetPalette(this Biota biota, uint subPaletteId)
        {
            return biota.BiotaPropertiesPalette.FirstOrDefault(x => x.SubPaletteId == subPaletteId);
        }

        public static Position GetPosition(this Biota biota, PositionType positionType)
        {
            var result = biota.BiotaPropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);

            if (result == null)
                return null;

            return new Position(result.Landblock ?? 0, result.OriginX, result.OriginY, result.OriginZ, result.AnglesX, result.AnglesY, result.AnglesZ, result.AnglesW);
        }

        public static BiotaPropertiesSkill GetProperty(this Biota biota, Skill skill)
        {
            return biota.BiotaPropertiesSkill.FirstOrDefault(x => x.Type == (uint)skill);
        }

        public static BiotaPropertiesSpellBook GetSpell(this Biota biota, int spell)
        {
            return biota.BiotaPropertiesSpellBook.FirstOrDefault(x => x.Spell == spell);
        }

        public static string GetProperty(this Biota biota, PropertyString property)
        {
            return biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static BiotaPropertiesTextureMap GetTextureMap(this Biota biota, byte index)
        {
            return biota.BiotaPropertiesTextureMap.FirstOrDefault(x => x.Index == index);
        }



        public static void SetProperty(this Biota biota, PropertyBool property, bool value)
        {
            var result = biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                result.Value = value;
            else
                biota.BiotaPropertiesBool.Add(new BiotaPropertiesBool {Type = (ushort)property, Value = value});

        }

        public static void SetProperty(this Biota biota, PropertyDataId property, uint value)
        {
            var result = biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                result.Value = value;
            else
                biota.BiotaPropertiesDID.Add(new BiotaPropertiesDID { Type = (ushort)property, Value = value });
        }

        public static void SetProperty(this Biota biota, PropertyDouble property, double value)
        {
            var result = biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property);
            if (result != null)
                result.Value = value;
            else
                biota.BiotaPropertiesFloat.Add(new BiotaPropertiesFloat { Type = (ushort)property, Value = value });
        }

        public static void SetProperty(this Biota biota, PropertyInstanceId property, int value)
        {
            var result = biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                result.Value = value;
            else
                biota.BiotaPropertiesIID.Add(new BiotaPropertiesIID { Type = (ushort)property, Value = value });
        }

        public static void SetProperty(this Biota biota, PropertyInt property, int value)
        {
            var result = biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                result.Value = value;
            else
                biota.BiotaPropertiesInt.Add(new BiotaPropertiesInt { Type = (ushort)property, Value = value });
        }

        public static void SetProperty(this Biota biota, PropertyInt64 property, long value)
        {
            var result = biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                result.Value = value;
            else
                biota.BiotaPropertiesInt64.Add(new BiotaPropertiesInt64 { Type = (ushort)property, Value = value });
        }

        public static void SetProperty(this Biota biota, PropertyString property, string value)
        {
            var result = biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                result.Value = value;
            else
                biota.BiotaPropertiesString.Add(new BiotaPropertiesString { Type = (ushort)property, Value = value });
        }



        public static void RemoveProperty(this Biota biota, PropertyBool property)
        {
            var result = biota.BiotaPropertiesBool.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                biota.BiotaPropertiesBool.Remove(result);
        }

        public static void RemoveProperty(this Biota biota, PropertyDataId property)
        {
            var result = biota.BiotaPropertiesDID.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                biota.BiotaPropertiesDID.Remove(result);
        }

        public static void RemoveProperty(this Biota biota, PropertyDouble property)
        {
            var result = biota.BiotaPropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property);
            if (result != null)
                biota.BiotaPropertiesFloat.Remove(result);
        }

        public static void RemoveProperty(this Biota biota, PropertyInstanceId property)
        {
            var result = biota.BiotaPropertiesIID.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                biota.BiotaPropertiesIID.Remove(result);
        }

        public static void RemoveProperty(this Biota biota, PropertyInt property)
        {
            var result = biota.BiotaPropertiesInt.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                biota.BiotaPropertiesInt.Remove(result);
        }

        public static void RemoveProperty(this Biota biota, PropertyInt64 property)
        {
            var result = biota.BiotaPropertiesInt64.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                biota.BiotaPropertiesInt64.Remove(result);
        }

        public static void RemoveProperty(this Biota biota, PropertyString property)
        {
            var result = biota.BiotaPropertiesString.FirstOrDefault(x => x.Type == (uint)property);
            if (result != null)
                biota.BiotaPropertiesString.Remove(result);
        }
    }
}
