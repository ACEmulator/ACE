using System;
using System.Collections.Generic;

namespace ACE.Entity.Models
{
    public static class WeenieExtensions
    {
        public static Biota CreateCopyAsBiota(this Weenie weenie, uint id)
        {
            var biota = new Biota();

            biota.Id = id;
            biota.WeenieClassId = weenie.WeenieClassId;
            biota.WeenieType = weenie.WeenieType;

            if (weenie.PropertiesBool != null)
                biota.PropertiesBool = new Dictionary<ushort, bool>(weenie.PropertiesBool);
            if (weenie.PropertiesDID != null)
                biota.PropertiesDID = new Dictionary<ushort, uint>(weenie.PropertiesDID);
            if (weenie.PropertiesFloat != null)
                biota.PropertiesFloat = new Dictionary<ushort, double>(weenie.PropertiesFloat);
            if (weenie.PropertiesIID != null)
                biota.PropertiesIID = new Dictionary<ushort, uint>(weenie.PropertiesIID);
            if (weenie.PropertiesInt != null)
                biota.PropertiesInt = new Dictionary<ushort, int>(weenie.PropertiesInt);
            if (weenie.PropertiesInt64 != null)
                biota.PropertiesInt64 = new Dictionary<ushort, long>(weenie.PropertiesInt64);
            if (weenie.PropertiesString != null)
                biota.PropertiesString = new Dictionary<ushort, string>(weenie.PropertiesString);


            if (weenie.PropertiesPosition != null)
            {
                biota.PropertiesPosition = new Dictionary<ushort, PropertiesPosition>();

                foreach (var kvp in weenie.PropertiesPosition)
                    biota.PropertiesPosition.Add(kvp.Key, kvp.Value.Clone());
            }


            if (weenie.PropertiesSpellBook != null)
                biota.PropertiesSpellBook = new Dictionary<int, float>(weenie.PropertiesSpellBook);


            if (weenie.PropertiesAnimPart != null)
            {
                biota.PropertiesAnimPart = new List<PropertiesAnimPart>();

                foreach (var record in weenie.PropertiesAnimPart)
                    biota.PropertiesAnimPart.Add(record.Clone());
            }

            if (weenie.PropertiesPalette != null)
            {
                biota.PropertiesPalette = new List<PropertiesPalette>();

                foreach (var record in weenie.PropertiesPalette)
                    biota.PropertiesPalette.Add(record.Clone());
            }

            if (weenie.PropertiesTextureMap != null)
            {
                biota.PropertiesTextureMap = new List<PropertiesTextureMap>();

                foreach (var record in weenie.PropertiesTextureMap)
                    biota.PropertiesTextureMap.Add(record.Clone());
            }


            // Properties for all world objects that typically aren't modified over the original weenie

            if (weenie.PropertiesCreateList != null)
            {
                biota.PropertiesCreateList = new List<PropertiesCreateList>();

                foreach (var record in weenie.PropertiesCreateList)
                    biota.PropertiesCreateList.Add(record.Clone());
            }

            if (weenie.PropertiesEmote != null)
            {
                biota.PropertiesEmote = new List<PropertiesEmote>();

                foreach (var record in weenie.PropertiesEmote)
                    biota.PropertiesEmote.Add(record.Clone());
            }

            if (weenie.PropertiesEventFilter != null)
                biota.PropertiesEventFilter = new List<int>(weenie.PropertiesEventFilter);

            if (weenie.PropertiesGenerator != null)
            {
                biota.PropertiesGenerator = new List<PropertiesGenerator>();

                foreach (var record in weenie.PropertiesGenerator)
                    biota.PropertiesGenerator.Add(record.Clone());
            }


            // Properties for creatures

            if (weenie.PropertiesAttribute != null)
            {
                biota.PropertiesAttribute = new Dictionary<ushort, PropertiesAttribute>();

                foreach (var kvp in weenie.PropertiesAttribute)
                    biota.PropertiesAttribute.Add(kvp.Key, kvp.Value.Clone());
            }

            if (weenie.PropertiesAttribute2nd != null)
            {
                biota.PropertiesAttribute2nd = new Dictionary<ushort, PropertiesAttribute2nd>();

                foreach (var kvp in weenie.PropertiesAttribute2nd)
                    biota.PropertiesAttribute2nd.Add(kvp.Key, kvp.Value.Clone());
            }

            if (weenie.PropertiesBodyPart != null)
            {
                biota.PropertiesBodyPart = new Dictionary<ushort, PropertiesBodyPart>();

                foreach (var kvp in weenie.PropertiesBodyPart)
                    biota.PropertiesBodyPart.Add(kvp.Key, kvp.Value.Clone());
            }

            if (weenie.PropertiesSkill != null)
            {
                biota.PropertiesSkill = new Dictionary<ushort, PropertiesSkill>();

                foreach (var kvp in weenie.PropertiesSkill)
                    biota.PropertiesSkill.Add(kvp.Key, kvp.Value.Clone());
            }


            // Properties for books

            if (weenie.PropertiesBook != null)
                biota.PropertiesBook = weenie.PropertiesBook.Clone();

            if (weenie.PropertiesBookPageData != null)
                biota.PropertiesBookPageData = new List<PropertiesBookPageData>(weenie.PropertiesBookPageData);


            return biota;
        }
    }
}
