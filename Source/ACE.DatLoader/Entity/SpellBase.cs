using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class SpellBase : IUnpackable
    {
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public MagicSchool School { get; private set; }
        public uint Icon { get; private set; }
        public SpellCategory Category { get; private set; } // All related levels of the same spell. Same category spells will not stack. (Strength Self I & Strength Self II)
        public uint Bitfield { get; private set; }
        public uint BaseMana { get; private set; } // Mana Cost
        public float BaseRangeConstant { get; private set; }
        public float BaseRangeMod { get; private set; }
        public uint Power { get; private set; } // Used to determine which spell in the catgory is the strongest.
        public float SpellEconomyMod { get; private set; } // A legacy of a bygone era
        public uint FormulaVersion { get; private set; }
        public float ComponentLoss { get; private set; } // Burn rate
        public SpellType MetaSpellType { get; private set; }
        public uint MetaSpellId { get; private set; } // Just the spell id again
        
        // Only on EnchantmentSpell/FellowshipEnchantmentSpells
        public double Duration { get; private set; }
        public float DegradeModifier { get; private set; } // Unknown what this does
        public float DegradeLimit { get; private set; }  // Unknown what this does

        public double PortalLifetime { get; private set; } // Only for PortalSummon_SpellType

        public List<uint> Formula { get; private set; } // UInt Values correspond to the SpellComponentsTable

        public uint CasterEffect { get; private set; }  // effect that playes on the caster of the casted spell (e.g. for buffs, protects, etc)
        public uint TargetEffect { get; private set; } // effect that playes on the target of the casted spell (e.g. for debuffs, vulns, etc)
        public uint FizzleEffect { get; private set; } // is always zero. All spells have the same fizzle effect.
        public double RecoveryInterval { get; private set; } // is always zero
        public float RecoveryAmount { get; private set; } // is always zero
        public uint DisplayOrder { get; private set; } // for soring in the spell list in the client UI
        public uint NonComponentTargetType { get; private set; } // Unknown what this does
        public uint ManaMod { get; private set; } // Additional mana cost per target (e.g. "Incantation of Acid Bane" Mana Cost = 80 + 14 per target)

        public SpellBase()
        {
        }

        public SpellBase(uint power, double duration, float degradeModifier, float degradeLimit)
        {
            Power = power;

            Duration = duration;
            DegradeModifier = degradeModifier;
            DegradeLimit = degradeLimit;
        }

        public void Unpack(BinaryReader reader)
        {
            Name = reader.ReadObfuscatedString();
            reader.AlignBoundary();
            Desc = reader.ReadObfuscatedString();
            reader.AlignBoundary();
            School = (MagicSchool)reader.ReadUInt32();
            Icon = reader.ReadUInt32();
            Category = (SpellCategory)reader.ReadUInt32();
            Bitfield = reader.ReadUInt32();
            BaseMana = reader.ReadUInt32();
            BaseRangeConstant = reader.ReadSingle();
            BaseRangeMod = reader.ReadSingle();
            Power = reader.ReadUInt32();
            SpellEconomyMod = reader.ReadSingle();
            FormulaVersion = reader.ReadUInt32();
            ComponentLoss = reader.ReadSingle();
            MetaSpellType = (SpellType)reader.ReadUInt32();
            MetaSpellId = reader.ReadUInt32();

            switch (MetaSpellType)
            {
                case SpellType.Enchantment:
                case SpellType.FellowEnchantment:
                    Duration = reader.ReadDouble();
                    DegradeModifier = reader.ReadSingle();
                    DegradeLimit = reader.ReadSingle();
                    break;
                case SpellType.PortalSummon:
                    PortalLifetime = reader.ReadDouble();
                    break;
            }

            // Components : Load them first, then decrypt them. More efficient to hash all at once.
            List<uint> rawComps = new List<uint>();

            for (uint j = 0; j < 8; j++)
            {
                uint comp = reader.ReadUInt32();

                // We will only add the comp if it is valid
                if (comp > 0)
                    rawComps.Add(comp);
            }

            // Get the decryped component values
            Formula = DecryptFormula(rawComps, Name, Desc);

            CasterEffect = reader.ReadUInt32();
            TargetEffect = reader.ReadUInt32();
            FizzleEffect = reader.ReadUInt32();
            RecoveryInterval = reader.ReadDouble();
            RecoveryAmount = reader.ReadSingle();
            DisplayOrder = reader.ReadUInt32();
            NonComponentTargetType = reader.ReadUInt32();
            ManaMod = reader.ReadUInt32();
        }

        private const uint HIGHEST_COMP_ID = 198; // "Essence of Kemeroi", for Void Spells -- not actually ever in game!

        /// <summary>
        /// Does the math based on the crypto keys (name and description) for the spell formula.
        /// </summary>
        private static List<uint> DecryptFormula(List<uint> rawComps, string name, string desc)
        {
            List<uint> comps = new List<uint>();

            // uint testDescHash = ComputeHash(" – 200");
            uint nameHash = SpellTable.ComputeHash(name);
            uint descHash = SpellTable.ComputeHash(desc);

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

        private string spellWords;

        /// <summary>
        /// Not technically part of this function, but saves numerous looks later.
        /// </summary>
        public string GetSpellWords(SpellComponentsTable comps)
        {
            if (spellWords != null)
                return spellWords;

            spellWords = SpellComponentsTable.GetSpellWords(comps, Formula);

            return spellWords;
        }
    }
}
