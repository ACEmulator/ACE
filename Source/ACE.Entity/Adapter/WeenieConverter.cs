using System;
using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;

namespace ACE.Entity.Adapter
{
    public static class WeenieConverter
    {
        public static Biota ConvertToBiota(this Weenie weenie, uint id)
        {
            var result = new Biota();

            result.Id = id;
            result.WeenieClassId = weenie.WeenieClassId;
            result.WeenieType = weenie.WeenieType;

            if (weenie.PropertiesBool != null)
                result.PropertiesBool = new Dictionary<PropertyBool, bool>(weenie.PropertiesBool);
            if (weenie.PropertiesDID != null)
                result.PropertiesDID = new Dictionary<PropertyDataId, uint>(weenie.PropertiesDID);
            if (weenie.PropertiesFloat != null)
                result.PropertiesFloat = new Dictionary<PropertyFloat, double>(weenie.PropertiesFloat);
            if (weenie.PropertiesIID != null)
                result.PropertiesIID = new Dictionary<PropertyInstanceId, uint>(weenie.PropertiesIID);
            if (weenie.PropertiesInt != null)
                result.PropertiesInt = new Dictionary<PropertyInt, int>(weenie.PropertiesInt);
            if (weenie.PropertiesInt64 != null)
                result.PropertiesInt64 = new Dictionary<PropertyInt64, long>(weenie.PropertiesInt64);
            if (weenie.PropertiesString != null)
                result.PropertiesString = new Dictionary<PropertyString, string>(weenie.PropertiesString);


            if (weenie.PropertiesPosition != null)
            {
                result.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>();

                foreach (var kvp in weenie.PropertiesPosition)
                    result.PropertiesPosition.Add(kvp.Key, kvp.Value.Clone());
            }


            if (weenie.PropertiesSpellBook != null)
                result.PropertiesSpellBook = new Dictionary<int, float>(weenie.PropertiesSpellBook);


            if (weenie.PropertiesAnimPart != null)
            {
                result.PropertiesAnimPart = new List<PropertiesAnimPart>();

                foreach (var record in weenie.PropertiesAnimPart)
                    result.PropertiesAnimPart.Add(record.Clone());
            }

            if (weenie.PropertiesPalette != null)
            {
                result.PropertiesPalette = new List<PropertiesPalette>();

                foreach (var record in weenie.PropertiesPalette)
                    result.PropertiesPalette.Add(record.Clone());
            }

            if (weenie.PropertiesTextureMap != null)
            {
                result.PropertiesTextureMap = new List<PropertiesTextureMap>();

                foreach (var record in weenie.PropertiesTextureMap)
                    result.PropertiesTextureMap.Add(record.Clone());
            }


            // Properties for all world objects that typically aren't modified over the original weenie

            if (weenie.PropertiesCreateList != null)
            {
                result.PropertiesCreateList = new List<PropertiesCreateList>();

                foreach (var record in weenie.PropertiesCreateList)
                    result.PropertiesCreateList.Add(record.Clone());
            }

            if (weenie.PropertiesEmote != null)
            {
                result.PropertiesEmote = new List<PropertiesEmote>();

                foreach (var record in weenie.PropertiesEmote)
                    result.PropertiesEmote.Add(record.Clone());
            }

            if (weenie.PropertiesEventFilter != null)
                result.PropertiesEventFilter = new List<int>(weenie.PropertiesEventFilter);

            if (weenie.PropertiesGenerator != null)
            {
                result.PropertiesGenerator = new List<PropertiesGenerator>();

                foreach (var record in weenie.PropertiesGenerator)
                    result.PropertiesGenerator.Add(record.Clone());
            }


            // Properties for creatures

            if (weenie.PropertiesAttribute != null)
            {
                result.PropertiesAttribute = new Dictionary<PropertyAttribute, PropertiesAttribute>();

                foreach (var kvp in weenie.PropertiesAttribute)
                    result.PropertiesAttribute.Add(kvp.Key, kvp.Value.Clone());
            }

            if (weenie.PropertiesAttribute2nd != null)
            {
                result.PropertiesAttribute2nd = new Dictionary<PropertyAttribute2nd, PropertiesAttribute2nd>();

                foreach (var kvp in weenie.PropertiesAttribute2nd)
                    result.PropertiesAttribute2nd.Add(kvp.Key, kvp.Value.Clone());
            }

            if (weenie.PropertiesBodyPart != null)
            {
                result.PropertiesBodyPart = new Dictionary<CombatBodyPart, PropertiesBodyPart>();

                foreach (var kvp in weenie.PropertiesBodyPart)
                    result.PropertiesBodyPart.Add(kvp.Key, kvp.Value.Clone());
            }

            if (weenie.PropertiesSkill != null)
            {
                result.PropertiesSkill = new Dictionary<Skill, PropertiesSkill>();

                foreach (var kvp in weenie.PropertiesSkill)
                    result.PropertiesSkill.Add(kvp.Key, kvp.Value.Clone());
            }


            // Properties for books

            if (weenie.PropertiesBook != null)
                result.PropertiesBook = weenie.PropertiesBook.Clone();

            if (weenie.PropertiesBookPageData != null)
                result.PropertiesBookPageData = new List<PropertiesBookPageData>(weenie.PropertiesBookPageData);


            return result;
        }
    }
}
