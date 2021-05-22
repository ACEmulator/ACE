using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;

namespace ACE.Entity.Adapter
{
    public static class WeenieConverter
    {
        public static Biota ConvertToBiota(this Weenie weenie, uint id, bool instantiateEmptyCollections = false, bool referenceWeenieCollectionsForCommonProperties = false)
        {
            var result = new Biota();

            result.Id = id;
            result.WeenieClassId = weenie.WeenieClassId;
            result.WeenieType = weenie.WeenieType;

            if (weenie.PropertiesBool != null && (instantiateEmptyCollections || weenie.PropertiesBool.Count > 0))
                result.PropertiesBool = new Dictionary<PropertyBool, bool>(weenie.PropertiesBool);
            if (weenie.PropertiesDID != null && (instantiateEmptyCollections || weenie.PropertiesDID.Count > 0))
                result.PropertiesDID = new Dictionary<PropertyDataId, uint>(weenie.PropertiesDID);
            if (weenie.PropertiesFloat != null && (instantiateEmptyCollections || weenie.PropertiesFloat.Count > 0))
                result.PropertiesFloat = new Dictionary<PropertyFloat, double>(weenie.PropertiesFloat);
            if (weenie.PropertiesIID != null && (instantiateEmptyCollections || weenie.PropertiesIID.Count > 0))
                result.PropertiesIID = new Dictionary<PropertyInstanceId, uint>(weenie.PropertiesIID);
            if (weenie.PropertiesInt != null && (instantiateEmptyCollections || weenie.PropertiesInt.Count > 0))
                result.PropertiesInt = new Dictionary<PropertyInt, int>(weenie.PropertiesInt);
            if (weenie.PropertiesInt64 != null && (instantiateEmptyCollections || weenie.PropertiesInt64.Count > 0))
                result.PropertiesInt64 = new Dictionary<PropertyInt64, long>(weenie.PropertiesInt64);
            if (weenie.PropertiesString != null && (instantiateEmptyCollections || weenie.PropertiesString.Count > 0))
                result.PropertiesString = new Dictionary<PropertyString, string>(weenie.PropertiesString);


            if (weenie.PropertiesPosition != null && (instantiateEmptyCollections || weenie.PropertiesPosition.Count > 0))
            {
                result.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>(weenie.PropertiesPosition.Count);

                foreach (var kvp in weenie.PropertiesPosition)
                    result.PropertiesPosition.Add(kvp.Key, kvp.Value.Clone());
            }


            if (weenie.PropertiesSpellBook != null && (instantiateEmptyCollections || weenie.PropertiesSpellBook.Count > 0))
                result.PropertiesSpellBook = new Dictionary<int, float>(weenie.PropertiesSpellBook);


            if (weenie.PropertiesAnimPart != null && (instantiateEmptyCollections || weenie.PropertiesAnimPart.Count > 0))
            {
                result.PropertiesAnimPart = new List<PropertiesAnimPart>(weenie.PropertiesAnimPart.Count);

                foreach (var record in weenie.PropertiesAnimPart)
                    result.PropertiesAnimPart.Add(record.Clone());
            }

            if (weenie.PropertiesPalette != null && (instantiateEmptyCollections || weenie.PropertiesPalette.Count > 0))
            {
                result.PropertiesPalette = new Collection<PropertiesPalette>();

                foreach (var record in weenie.PropertiesPalette)
                    result.PropertiesPalette.Add(record.Clone());
            }

            if (weenie.PropertiesTextureMap != null && (instantiateEmptyCollections || weenie.PropertiesTextureMap.Count > 0))
            {
                result.PropertiesTextureMap = new List<PropertiesTextureMap>(weenie.PropertiesTextureMap.Count);

                foreach (var record in weenie.PropertiesTextureMap)
                    result.PropertiesTextureMap.Add(record.Clone());
            }


            // Properties for all world objects that typically aren't modified over the original weenie

            if (referenceWeenieCollectionsForCommonProperties)
            {
                result.PropertiesCreateList = weenie.PropertiesCreateList;
                result.PropertiesEmote = weenie.PropertiesEmote;
                result.PropertiesEventFilter = weenie.PropertiesEventFilter;
                result.PropertiesGenerator = weenie.PropertiesGenerator;
            }
            else
            {
                if (weenie.PropertiesCreateList != null && (instantiateEmptyCollections || weenie.PropertiesCreateList.Count > 0))
                {
                    result.PropertiesCreateList = new Collection<PropertiesCreateList>();

                    foreach (var record in weenie.PropertiesCreateList)
                        result.PropertiesCreateList.Add(record.Clone());
                }

                if (weenie.PropertiesEmote != null && (instantiateEmptyCollections || weenie.PropertiesEmote.Count > 0))
                {
                    result.PropertiesEmote = new Collection<PropertiesEmote>();

                    foreach (var record in weenie.PropertiesEmote)
                        result.PropertiesEmote.Add(record.Clone());
                }

                if (weenie.PropertiesEventFilter != null && (instantiateEmptyCollections || weenie.PropertiesEventFilter.Count > 0))
                    result.PropertiesEventFilter = new HashSet<int>(weenie.PropertiesEventFilter);

                if (weenie.PropertiesGenerator != null && (instantiateEmptyCollections || weenie.PropertiesGenerator.Count > 0))
                {
                    result.PropertiesGenerator = new List<PropertiesGenerator>(weenie.PropertiesGenerator.Count);

                    foreach (var record in weenie.PropertiesGenerator)
                        result.PropertiesGenerator.Add(record.Clone());
                }
            }


            // Properties for creatures

            if (weenie.PropertiesAttribute != null && (instantiateEmptyCollections || weenie.PropertiesAttribute.Count > 0))
            {
                result.PropertiesAttribute = new Dictionary<PropertyAttribute, PropertiesAttribute>(weenie.PropertiesAttribute.Count);

                foreach (var kvp in weenie.PropertiesAttribute)
                    result.PropertiesAttribute.Add(kvp.Key, kvp.Value.Clone());
            }

            if (weenie.PropertiesAttribute2nd != null && (instantiateEmptyCollections || weenie.PropertiesAttribute2nd.Count > 0))
            {
                result.PropertiesAttribute2nd = new Dictionary<PropertyAttribute2nd, PropertiesAttribute2nd>(weenie.PropertiesAttribute2nd.Count);

                foreach (var kvp in weenie.PropertiesAttribute2nd)
                    result.PropertiesAttribute2nd.Add(kvp.Key, kvp.Value.Clone());
            }

            if (referenceWeenieCollectionsForCommonProperties)
            {
                result.PropertiesBodyPart = weenie.PropertiesBodyPart;
            }
            else
            {
                if (weenie.PropertiesBodyPart != null && (instantiateEmptyCollections || weenie.PropertiesBodyPart.Count > 0))
                {
                    result.PropertiesBodyPart = new Dictionary<CombatBodyPart, PropertiesBodyPart>(weenie.PropertiesBodyPart.Count);

                    foreach (var kvp in weenie.PropertiesBodyPart)
                        result.PropertiesBodyPart.Add(kvp.Key, kvp.Value.Clone());
                }
            }

            if (weenie.PropertiesSkill != null && (instantiateEmptyCollections || weenie.PropertiesSkill.Count > 0))
            {
                result.PropertiesSkill = new Dictionary<Skill, PropertiesSkill>(weenie.PropertiesSkill.Count);

                foreach (var kvp in weenie.PropertiesSkill)
                    result.PropertiesSkill.Add(kvp.Key, kvp.Value.Clone());
            }


            // Properties for books

            if (weenie.PropertiesBook != null)
                result.PropertiesBook = weenie.PropertiesBook.Clone();

            if (weenie.PropertiesBookPageData != null && (instantiateEmptyCollections || weenie.PropertiesBookPageData.Count > 0))
                result.PropertiesBookPageData = new List<PropertiesBookPageData>(weenie.PropertiesBookPageData);


            return result;
        }
    }
}
