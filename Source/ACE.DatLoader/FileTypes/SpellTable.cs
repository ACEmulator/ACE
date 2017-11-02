﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    public class SpellTable
    {
        public uint FileId { get; set; }
        public ushort SpellBaseHash { get; set; } // not entirely sure what this is
        public Dictionary<uint, SpellBase> Spells { get; set; } = new Dictionary<uint, SpellBase>();

        public static SpellTable ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(0x0E00000E))
            {
                return (SpellTable)DatManager.PortalDat.FileCache[0x0E00000E];
            }
            else
            {
                // Create the datReader for the proper file
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(0x0E00000E);
                SpellTable spells = new SpellTable();

                spells.FileId = datReader.ReadUInt32();
                uint spellCount = datReader.ReadUInt16();
                spells.SpellBaseHash = datReader.ReadUInt16(); 

                for (uint i = 0; i < spellCount; i++)
                {
                    SpellBase newSpell = new SpellBase();
                    uint spellId = datReader.ReadUInt32();
                    newSpell.Name = datReader.ReadObfuscatedString();
                    datReader.AlignBoundary();
                    newSpell.Desc = datReader.ReadObfuscatedString();
                    datReader.AlignBoundary();
                    newSpell.School = (MagicSchool)datReader.ReadUInt32();
                    newSpell.Icon = datReader.ReadUInt32();
                    newSpell.Category = datReader.ReadUInt32();
                    newSpell.Bitfield = datReader.ReadUInt32();
                    newSpell.BaseMana = datReader.ReadUInt32();
                    newSpell.BaseRangeConstant = datReader.ReadSingle();
                    newSpell.BaseRangeMod = datReader.ReadSingle();
                    newSpell.Power = datReader.ReadUInt32();
                    newSpell.SpellEconomyMod = datReader.ReadSingle();
                    newSpell.FormulaVersion = datReader.ReadUInt32();
                    newSpell.ComponentLoss = datReader.ReadUInt32();
                    newSpell.MetaSpellType = (SpellType)datReader.ReadUInt32(); 
                    newSpell.MetaSpellId = datReader.ReadUInt32();

                    switch (newSpell.MetaSpellType)
                    {
                        case SpellType.Enchantment:
                        case SpellType.FellowEnchantment:
                            {
                                newSpell.Duration = datReader.ReadDouble();
                                newSpell.DegradeModifier = datReader.ReadSingle();
                                newSpell.DegradeLimit = datReader.ReadSingle();
                                break;
                            }
                        case SpellType.PortalSummon:
                            {
                                newSpell.PortalLifetime = datReader.ReadDouble();
                                break;
                            }
                    }

                    // Components : Load them first, then decrypt them. More efficient to hash all at once.
                    List<uint> rawComps = new List<uint>();
                    for (uint j = 0; j < 8; j++)
                    {
                        uint comp = datReader.ReadUInt32();
                        // We will only add the comp if it is valid
                        if (comp > 0)
                            rawComps.Add(comp);
                    }
                    // Get the decryped component values
                    newSpell.Formula = DecryptFormula(rawComps, newSpell.Name, newSpell.Desc);

                    newSpell.CasterEffect = datReader.ReadUInt32();
                    newSpell.TargetEffect = datReader.ReadUInt32();
                    newSpell.FizzleEffect = datReader.ReadUInt32();
                    newSpell.RecoveryInterval = datReader.ReadDouble();
                    newSpell.RecoveryAmount = datReader.ReadSingle();
                    newSpell.DisplayOrder = datReader.ReadUInt32();
                    newSpell.NonComponentTargetType = datReader.ReadUInt32();
                    newSpell.ManaMod = datReader.ReadUInt32();

                    spells.Spells.Add(spellId, newSpell);
                }

                DatManager.PortalDat.FileCache[0x0E00000E] = spells;
                return spells;
            }
        }

        private const uint HIGHEST_COMP_ID = 198; // "Essence of Kemeroi", for Void Spells -- not actually ever in game!

        /// <summary>
        /// Does the math based on the crypto keys (name and description) for the spell formula.
        /// </summary>
        private static List<uint> DecryptFormula(List<uint> rawComps, string name, string desc)
        {
            List<uint> comps = new List<uint>();
            // uint testDescHash = ComputeHash(" – 200");
            uint nameHash = ComputeHash(name);
            uint descHash = ComputeHash(desc);
            
            uint key = (nameHash % 0x12107680) + (descHash % 0xBEADCF45); 
            for (int i = 0; i < rawComps.Count; i++)
            {
                uint comp = (rawComps[i] - key);

                // This seems to correct issues with certain spells with extended characters.
                if (comp > HIGHEST_COMP_ID) // highest comp ID is 198 - "Essence of Kemeroi", for Void Spells
                    comp = comp & 0xFF;
                comps.Add(comp);
            }

            return comps;
        }

        /// <summary>
        /// Generates a hash based on the string. Used to decrypt spell formulas and calculate taper rotation for players.
        /// </summary>
        private static uint ComputeHash(string strToHash)
        {
            uint result = 0;

            if (strToHash.Length > 0)
            {
                byte[] str = Encoding.Default.GetBytes(strToHash);
                foreach (byte c in str)                
                {
                    result = c + (result << 4);
                    if ((result & 0xF0000000) != 0)
                        result = (result ^ ((result & 0xF0000000) >> 24)) & 0x0FFFFFFF;
                }
            }

            return result;
        }

        private const uint LOWEST_TAPER_ID = 63; // This is the lowest id in the SpellComponentTable of a taper (Red Taper)

        /// <summary>
        /// Returns the correct spell formula, which is hashed from a player's account name
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="accountName"></param>
        /// <returns>A list of spellcomp ids</returns>
        public static List<uint> GetSpellFormula(uint spellId, string accountName)
        {
            SpellTable s = ReadFromDat();
            SpellBase spell = s.Spells[spellId];
           
            switch (spell.FormulaVersion)
            {
                case 1:
                    return RandomizeVersion1(spell, accountName);
                case 2:
                    return RandomizeVersion2(spell, accountName);
                case 3:
                    return RandomizeVersion3(spell, accountName);
                default:
                    return spell.Formula;
            }
        }

        private static List<uint> RandomizeVersion1(SpellBase spell, string accountName)
        {
            List<uint> comps = spell.Formula;
            bool hasTaper1 = false;
            bool hasTaper2 = false;
            bool hasTaper3 = false;

            uint key = ComputeHash(accountName);
            uint seed = key % 0x13D573;

            uint scarab = comps[0];
            int herb_index = 1;
            if (comps.Count > 5)
            {
                herb_index = 2;
                hasTaper1 = true;
            }
            uint herb = comps[herb_index];

            int powder_index = herb_index + 1;
            if (comps.Count > 6)
            {
                powder_index++;
                hasTaper2 = true;
            }
            uint powder = comps[powder_index];

            int potion_index = powder_index + 1;
            uint potion = comps[potion_index];

            int talisman_index = potion_index + 1;
            if (comps.Count > 7)
            {
                talisman_index++;
                hasTaper3 = true;
            }
            uint talisman = comps[talisman_index];

            if (hasTaper1)
            {
                comps[1] = (powder + 2 * herb + potion + talisman + scarab) % 0xC + LOWEST_TAPER_ID;
            }

            if (hasTaper2)
            {
                comps[3] = (scarab + herb + talisman + 2 * (powder + potion)) * (seed / (scarab + (powder + potion))) % 0xC + LOWEST_TAPER_ID;
            }

            if (hasTaper3)
            {
                comps[6] = (powder + 2 * talisman + potion + herb + scarab) * (seed / (talisman + scarab)) % 0xC + LOWEST_TAPER_ID;
            }

            return comps;
        }

        private static List<uint> RandomizeVersion2(SpellBase spell, string accountName)
        {
            List<uint> comps = spell.Formula;

            uint key = ComputeHash(accountName);
            uint seed = key % 0x13D573;

            uint p1 = comps[0];
            uint c = comps[4];
            uint x = comps[5];
            uint a = comps[7];

            comps[3] = (a + 2 * comps[0] + 2 * c * x + comps[0] + comps[2] + comps[1]) % 0xC + LOWEST_TAPER_ID;
            comps[6] = (a + 2 * p1 * comps[2] + 2 * x + p1 * comps[2] + c) * (seed / (comps[1] * a + 2 * c)) % 0xC + LOWEST_TAPER_ID;

            return comps;
        }

        private static List<uint> RandomizeVersion3(SpellBase spell, string accountName)
        {
            List<uint> comps = spell.Formula;

            uint key = ComputeHash(accountName);
            uint seed1 = key % 0x13D573;
            uint seed2 = key % 0x4AEFD;
            uint seed3 = key % 0x96A7F;
            uint seed4 = key % 0x100A03;
            uint seed5 = key % 0xEB2EF;
            uint seed6 = key % 0x121E7D;

            uint compHash0 = (seed1 + comps[0]) % 0xC;
            uint compHash1 = (seed2 + comps[1]) % 0xC;
            uint compHash2 = (seed3 + comps[2]) % 0xC;
            uint compHash4 = (seed4 + comps[4]) % 0xC;
            uint compHash5 = (seed5 + comps[5]) % 0xC;

            // Some spells don't have the full number of comps. 2697 ("Aerfalle's Touch"), is one example.
            uint compHash7;
            if (comps.Count < 8)
                compHash7 = (seed6 + 0) % 0xC;
            else
                compHash7 = (seed6 + comps[7]) % 0xC;

            comps[3] = (compHash0 + compHash1 + compHash2 + compHash4 + compHash5 + compHash2 * compHash5 + compHash0 * compHash1 + compHash7 * (compHash4 + 1)) % 0xC + LOWEST_TAPER_ID;
            comps[6] = (compHash0 + compHash1 + compHash2 + compHash4 + key % 0x65039 % 0xC + compHash7 * (compHash4 * (compHash0 * compHash1 * compHash2 * compHash5 + 7) + 1) + compHash5 + 4 * compHash0 * compHash1 + compHash0 * compHash1 + 11 * compHash2 * compHash5) % 0xC + LOWEST_TAPER_ID;

            return comps;
        }
    }
}
