using System;
using System.Collections.Generic;

using log4net;

using ACE.Common;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    /// <summary>
    /// The Spell class for game code
    /// A wrapper around SpellBase and Database.Spell
    /// </summary>
    public partial class Spell: IEquatable<Spell>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            if (loadDB && (_spell == null || _spellBase == null))
                log.Debug($"Spell.Init(spellID = {spellID}, loadDB = {loadDB}) failed! {(_spell == null ? "_spell was null" : "")} {(_spellBase == null ? "_spellBase was null" : "")}");
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
        /// Returns TRUE if this is a harmful spell
        /// </summary>
        public bool IsHarmful { get => !IsBeneficial; }

        /// <summary>
        /// Returns TRUE if this spell is resistable
        /// </summary>
        public bool IsResistable => Flags.HasFlag(SpellFlags.Resistable);

        public bool IsProjectile => NumProjectiles > 0;

        public bool IsSelfTargeted => Flags.HasFlag(SpellFlags.SelfTargeted);

        public bool IsTracking => !Flags.HasFlag(SpellFlags.NonTrackingProjectile);

        public bool IsFellowshipSpell => Flags.HasFlag(SpellFlags.FellowshipSpell);

        public List<uint> TryBurnComponents(Player player)
        {
            var consumed = new List<uint>();

            // the base rate for each component is defined per-spell
            var baseRate = ComponentLoss;

            // get magic skill mod
            var magicSkill = GetMagicSkill();
            var playerSkill = player.GetCreatureSkill(magicSkill);
            var skillMod = Math.Min(1.0f, (float)Power / playerSkill.Current);
            //Console.WriteLine($"TryBurnComponents.SkillMod: {skillMod}");

            //DebugComponents();

            foreach (var component in Formula.CurrentFormula)
            {
                if (!SpellFormula.SpellComponentsTable.SpellComponents.TryGetValue(component, out var spellComponent))
                {
                    Console.WriteLine($"Spell.TryBurnComponents(): Couldn't find SpellComponent {component}");
                    continue;
                }

                // component burn rate = spell base rate * component destruction modifier * skillMod?
                var burnRate = baseRate * spellComponent.CDM * skillMod;

                // TODO: curve?
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

        /// <summary>
        /// Returns TRUE if spell category matches impen / bane / brittlemail / lure
        /// </summary>
        public bool IsImpenBaneType
        {
            get
            {
                switch (Category)
                {
                    case SpellCategory n when n >= SpellCategory.ArmorValueRaising && n <= SpellCategory.AcidicResistanceLowering:
                    case SpellCategory.ArmorValueRaisingRare:
                    case SpellCategory.AcidResistanceRaisingRare:
                    case SpellCategory.BludgeonResistanceRaisingRare:
                    case SpellCategory.ColdResistanceRaisingRare:
                    case SpellCategory.ElectricResistanceRaisingRare:
                    case SpellCategory.FireResistanceRaisingRare:
                    case SpellCategory.PierceResistanceRaisingRare:
                    case SpellCategory.SlashResistanceRaisingRare:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Returns TRUE if spell category matches spells that should redirect to items player is holding
        /// </summary>
        public bool IsItemRedirectableType
        {
            get
            {
                switch (Category)
                {
                    case SpellCategory.DamageRaisingRare:
                    case SpellCategory.AttackModRaisingRare:
                    case SpellCategory.DefenseModRaisingRare:
                    case SpellCategory.WeaponTimeRaisingRare:
                    case SpellCategory.AppraisalResistanceLoweringRare:
                    case SpellCategory.MaxDamageRaising:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsNegativeRedirectable => IsHarmful && (IsImpenBaneType || IsOtherNegativeRedirectable);

        public bool IsOtherNegativeRedirectable
        {
            get
            {
                switch (Category)
                {
                    case SpellCategory.DamageLowering:            // encompasses both blood and spirit loather, inconsistent with spirit drinker in dat
                    case SpellCategory.DefenseModLowering:
                    case SpellCategory.AttackModLowering:
                    case SpellCategory.WeaponTimeLowering:        // verified
                    case SpellCategory.ManaConversionModLowering: // hermetic void, replaced hide value, unchanged category in dat
                        return true;
                }
                return false;
            }
        }

        public bool IsPortalSpell
        {
            get
            {
                switch (MetaSpellType)
                {
                    case SpellType.PortalLink:
                    case SpellType.PortalRecall:
                    case SpellType.PortalSending:
                    case SpellType.PortalSummon:
                    case SpellType.FellowPortalSending:
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Handles forward compatibility for old item spells which should be auras
        /// </summary>
        public bool HasItemCategory
        {
            get
            {
                switch (Category)
                {
                    case SpellCategory.AttackModRaising:
                    case SpellCategory.DamageRaising:
                    case SpellCategory.DefenseModRaising:
                    case SpellCategory.WeaponTimeRaising:        // verified
                    case SpellCategory.ManaConversionModRaising:
                    case SpellCategory.SpellDamageRaising:
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Returns TRUE for any spells which could potentially affect the run rate,
        /// such as spells which alter run / quickness / strength
        /// </summary>
        public bool UpdatesRunRate
        {
            get
            {
                if (_spell == null)
                    return false;

                // this is commented out as below in UpdatesMaxVitals
                // i forget the exact reasoning, are all the proper hooks in places for each vitae %,
                // and not just add/remove?
                /*if (_spell.Id == 666)   // vitae
                    return true;*/

                if (StatModType.HasFlag(EnchantmentTypeFlags.Attribute) && (StatModKey == (uint)PropertyAttribute.Strength || StatModKey == (uint)PropertyAttribute.Quickness))
                    return true;

                if (StatModType.HasFlag(EnchantmentTypeFlags.Skill) && StatModKey == (uint)Skill.Run)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Returns a list of MaxVitals affected by this spell
        /// </summary>
        public List<PropertyAttribute2nd> UpdatesMaxVitals
        {
            get
            {
                var maxVitals = new List<PropertyAttribute2nd>();

                if (_spell == null)
                    return maxVitals;

                /*if (_spell.Id == 666)   // Vitae
                {
                    maxVitals.Add(PropertyAttribute2nd.MaxHealth);
                    maxVitals.Add(PropertyAttribute2nd.MaxStamina);
                    maxVitals.Add(PropertyAttribute2nd.MaxMana);

                    return maxVitals;
                }*/

                if (StatModType.HasFlag(EnchantmentTypeFlags.SecondAtt) && StatModKey != 0)
                    maxVitals.Add((PropertyAttribute2nd)StatModKey);

                else if (StatModType.HasFlag(EnchantmentTypeFlags.Attribute))
                {
                    switch ((PropertyAttribute)StatModKey)
                    {
                        case PropertyAttribute.Endurance:
                            maxVitals.Add(PropertyAttribute2nd.MaxHealth);
                            maxVitals.Add(PropertyAttribute2nd.MaxStamina);
                            break;

                        case PropertyAttribute.Self:
                            maxVitals.Add(PropertyAttribute2nd.MaxMana);
                            break;
                    }
                }
                return maxVitals;
            }
        }

        /// <summary>
        /// Returns TRUE if this spell is a DamageOverTime or HealingOverTime spell
        /// </summary>
        public bool IsDamageOverTime
        {
            get
            {
                if (Flags.HasFlag(SpellFlags.DamageOverTime))
                    return true;

                switch (Category)
                {
                    case SpellCategory.HealOverTimeRaising:
                    case SpellCategory.DamageOverTimeRaising:
                    case SpellCategory.AetheriaProcHealthOverTimeRaising:
                    case SpellCategory.AetheriaProcDamageOverTimeRaising:
                    case SpellCategory.NetherDamageOverTimeRaising:
                    case SpellCategory.NetherDamageOverTimeRaising2:
                    case SpellCategory.NetherDamageOverTimeRaising3:

                        return true;
                }

                switch ((PropertyInt)StatModKey)
                {
                    case PropertyInt.HealOverTime:
                    case PropertyInt.DamageOverTime:

                        return true;
                }

                return false;
            }
        }

        public bool HasExtraTick => IsDamageOverTime;

        public bool Equals(Spell spell)
        {
            return spell != null && Id == spell.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
