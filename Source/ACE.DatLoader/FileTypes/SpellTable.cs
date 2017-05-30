using System;
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
                        case SpellType.Enchantment_SpellType:
                        case SpellType.FellowEnchantment_SpellType:
                            {
                                newSpell.Duration = datReader.ReadDouble();
                                newSpell.DegradeModifier = datReader.ReadSingle();
                                newSpell.DegradeLimit = datReader.ReadSingle();
                                break;
                            }
                        case SpellType.PortalSummon_SpellType:
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

        /// <summary>
        /// Does the math based on the crypto keys (name and description) for the spell formula.
        /// </summary>
        /// <param name="rawComps"></param>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private static List<uint> DecryptFormula(List<uint> rawComps, string name, string desc)
        {
            List<uint> comps = new List<uint>();
            uint nameHash = ComputeHash(name);
            uint descHash = ComputeHash(desc);

            uint key = (nameHash % 0x12107680) + (descHash % 0xBEADCF45); 
            for (int i = 0; i < rawComps.Count; i++)
            {
                uint comp = rawComps[i] - key;
                comps.Add(comp);
            }

            return comps;
        }

        /// <summary>
        /// Generates a hash based on the string. Used to decrypt spell formulas and calculate taper rotation for players.
        /// </summary>
        /// <param name="strToHash"></param>
        /// <returns></returns>
        private static uint ComputeHash(string strToHash)
        {
            uint result = 0;

            if (strToHash.Length > 0)
            {
                foreach (char c in strToHash)
                {
                    result = c + (result << 4);
                    if ((result & 0xF0000000) != 0)
                        result = (result ^ ((result & 0xF0000000) >> 24)) & 0xFFFFFFF;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the correct spell formula, which is hashed from a player's account name
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="accountName"></param>
        /// <returns>A list of spellcomp ids</returns>
        public static List<uint> GetSpellFormula(uint spellId, string accountName)
        {
            // TODO - Make this function work. Also, figure out the math!
            return new List<uint>();
        }
    }
}
