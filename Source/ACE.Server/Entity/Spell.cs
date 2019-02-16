using System;
using System.Collections.Generic;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Database;
using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    /// <summary>
    /// The Spell class for game code
    /// A wrapper around SpellBase and Database.Spell
    /// </summary>
    public partial class Spell
    {
        /// <summary>
        /// The spell information from the client DAT
        /// </summary>
        public SpellBase _spellBase;

        /// <summary>
        /// The spell information from the server DB
        /// </summary>
        public Database.Models.World.Spell _spell;

        /// <summary>
        /// Returns TRUE if spell is missing from either the client DAT or the server spell db
        /// </summary>
        public bool NotFound { get => _spellBase == null || _spell == null; }

        /// <summary>
        /// The components required to cast the spell
        /// </summary>
        public SpellFormula Formula;

        /// <summary>
        /// Constructs a Spell from a spell ID
        /// </summary>
        /// <param name="loadDB">If FALSE, only loads the DAT info (faster)</param>
        public Spell(uint spellID, bool loadDB = true)
        {
            Init(spellID, loadDB);
        }

        /// <summary>
        /// Constructs a Spell from a spell ID
        /// </summary>
        public Spell(int spellID, bool loadDB = true)
        {
            Init((uint)spellID, loadDB);
        }

        /// <summary>
        /// Constructs a Spell from a Spell enum
        /// </summary>
        public Spell(SpellId spell, bool loadDB = true)
        {
            Init((uint)spell, loadDB);
        }

        /// <summary>
        /// Default initializer
        /// </summary>
        public void Init(uint spellID, bool loadDB = true)
        {
            DatManager.PortalDat.SpellTable.Spells.TryGetValue(spellID, out _spellBase);

            if (loadDB)
                _spell = DatabaseManager.World.GetCachedSpell(spellID);

            if (_spellBase != null)
                Formula = new SpellFormula(this, _formula);
        }

        /// <summary>
        /// Uses the server spell level formula,
        /// which checks the power level of the spell,
        /// and compares to the minimum power for each spell level.
        /// </summary>
        public uint Level
        {
            // this uses the server / power-based formula
            // for the client / scarab-based formula, use Formula.Level
            get
            {
                for (uint spellLevel = SpellFormula.MaxSpellLevel; spellLevel > 0; spellLevel--)
                {
                    var minPower = SpellFormula.MinPower[spellLevel];
                    if (Power >= minPower)
                        return spellLevel;
                }
                return 0;
            }
        }

        /// <summary>
        /// Returns TRUE if the spell levels are the same between the client and server formulas
        /// </summary>
        public bool LevelMatch { get => Formula.Level == Level; }

        /// <summary>
        /// Returns TRUE if this is a beneficial spell
        /// </summary>
        public bool IsBeneficial => Flags.HasFlag(SpellFlags.Beneficial);

        /// <summary>
        /// Returns TRUE if this is a hamrful spell
        /// </summary>
        public bool IsHarmful { get => !IsBeneficial; }

        public bool IsProjectile => NumProjectiles > 0;

        public List<uint> TryBurnComponents()
        {
            var consumed = new List<uint>();

            // the base rate for each component is defined per-spell
            var baseRate = ComponentLoss;

            //DebugComponents();

            foreach (var component in Formula.CurrentFormula)
            {
                if (!SpellFormula.SpellComponentsTable.SpellComponents.TryGetValue(component, out var spellComponent))
                {
                    Console.WriteLine($"Spell.TryBurnComponents(): Couldn't find SpellComponent {component}");
                    continue;
                }

                // component burn rate = spell base rate * component destruction modifier
                var burnRate = baseRate * spellComponent.CDM;

                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
                if (rng < burnRate)
                    consumed.Add(component);
            }
            return consumed;
        }

        public void DebugComponents()
        {
            var baseComponents = Formula.Components;
            var currComponents = Formula.CurrentFormula;

            Console.WriteLine($"{Name}:");
            Console.WriteLine($"Base formula: {string.Join(", ", GetComponentNames(baseComponents))}");
            Console.WriteLine($"Current formula: {string.Join(", ", GetComponentNames(currComponents))}");
        }

        public static string GetConsumeString(List<uint> components)
        {
            var compNames = GetComponentNames(components);
            return $"The spell consumed the following components: {string.Join(", ", compNames)}";
        }

        public static List<string> GetComponentNames(List<uint> components)
        {
            var compNames = new List<string>();

            foreach (var component in components)
            {
                if (!SpellFormula.SpellComponentsTable.SpellComponents.TryGetValue(component, out var spellComponent))
                {
                    Console.WriteLine($"Spell.GetComponentNames(): Couldn't find SpellComponent {component}");
                    continue;
                }

                compNames.Add(spellComponent.Name);
            }
            return compNames;
        }

        public static uint SpellComponentDIDs = 0x27000002;

        public static uint GetComponentWCID(uint compID)
        {
            var dualDIDs = DatManager.PortalDat.ReadFromDat<DualDidMapper>(SpellComponentDIDs);

            if (!dualDIDs.ClientEnumToID.TryGetValue(compID, out var wcid))
            {
                Console.WriteLine($"GetComponentWCID({compID}): couldn't find component ID");
                return 0;
            }
            return wcid;
        }

        public Skill GetMagicSkill()
        {
            switch (School)
            {
                case MagicSchool.CreatureEnchantment: return Skill.CreatureEnchantment;
                case MagicSchool.ItemEnchantment:     return Skill.ItemEnchantment;
                case MagicSchool.LifeMagic:           return Skill.LifeMagic;
                case MagicSchool.WarMagic:            return Skill.WarMagic;
                case MagicSchool.VoidMagic:           return Skill.VoidMagic;
            }
            return Skill.None;
        }

        public bool IsPortalSpell
        {
            get
            {
                return MetaSpellType == SpellType.PortalLink
                    || MetaSpellType == SpellType.PortalRecall
                    || MetaSpellType == SpellType.PortalSending
                    || MetaSpellType == SpellType.PortalSummon;
            }
        }

        /// <summary>
        /// Handles forward compatibility for old item spells which should be auras
        /// </summary>
        public bool HasItemCategory
        {
            get
            {
                return Category == SpellCategory.AttackModRaising
                    || Category == SpellCategory.DamageRaising
                    || Category == SpellCategory.DefenseModRaising
                    || Category == SpellCategory.WeaponTimeRaising
                    || Category == SpellCategory.AppraisalResistanceLowering
                    || Category == SpellCategory.SpellDamageRaising;
            }
        }
    }
}
