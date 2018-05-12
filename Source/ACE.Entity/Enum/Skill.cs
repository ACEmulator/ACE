using System.Linq;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// note: even though these are unnumbered, order is very important.  values of "none" or commented
    /// as retired or unused CANNOT be removed
    /// </summary>
    public enum Skill
    {
        None,

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Coordination, 3)]
        Axe,                 /* Retired */

        [AttributeFormula(AttributeCache.Coordination, 2)]
        Bow,                 /* Retired */

        [AttributeFormula(AttributeCache.Coordination, 2)]
        Crossbow,            /* Retired */

        [AttributeFormula(AttributeCache.Quickness | AttributeCache.Coordination, 3)]
        Dagger,              /* Retired */

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Coordination, 3)]
        Mace,                /* Retired */

        [AttributeFormula(AttributeCache.Quickness | AttributeCache.Coordination, 3)]
        [SkillCost(10, 10)]
        [SkillUsableUntrained(true)]
        MeleeDefense,

        [AttributeFormula(AttributeCache.Quickness | AttributeCache.Coordination, 5)]
        [SkillCost(6, 4)]
        [SkillUsableUntrained(true)]
        MissileDefense,

        Sling,               /* Retired */

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Coordination, 3)]
        Spear,               /* Retired */

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Coordination, 3)]
        Staff,               /* Retired */

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Coordination, 3)]
        Sword,               /* Retired */

        [AttributeFormula(AttributeCache.Coordination, 2)]
        ThrownWeapon,        /* Retired */

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Coordination, 3)]
        UnarmedCombat,       /* Retired */

        [AttributeFormula(AttributeCache.Focus, 3)]
        [SkillCost(0, 2, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        ArcaneLore,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Self, 7)]
        [SkillCost(0, 12, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        MagicDefense,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Self, 6)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(false)]
        ManaConversion,

        Spellcraft,          /* Unimplemented */

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Self, 6)]
        [SkillCost(2, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        ItemTinkering,

        [AttributeFormula(AttributeCache.Undef)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(false)]
        AssessPerson,

        [AttributeFormula(AttributeCache.Undef)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        Deception,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Coordination, 3)]
        [SkillCost(6, 4)]
        [SkillUsableUntrained(false)]
        Healing,

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Coordination, 2)]
        [SkillCost(0, 4, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Jump,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Coordination, 3)]
        [SkillCost(6, 4)]
        [SkillUsableUntrained(false)]
        Lockpick,

        [AttributeFormula(AttributeCache.Quickness)]
        [SkillCost(0, 4, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Run,

        Awareness,           /* Unimplemented */
        ArmsAndArmorRepair,  /* Unimplemented */

        [AttributeFormula(AttributeCache.Undef)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        AssessCreature,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Strength, 2)]
        [SkillCost(4, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        WeaponTinkering,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Endurance, 2)]
        [SkillCost(4, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        ArmorTinkering,

        [AttributeFormula(AttributeCache.Focus)]
        [SkillCost(4, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        MagicItemTinkering,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Self, 4)]
        [SkillCost(8, 8)]
        [SkillUsableUntrained(false)]
        CreatureEnchantment,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Self, 4)]
        [SkillCost(8, 8)]
        [SkillUsableUntrained(false)]
        ItemEnchantment,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Self, 4)]
        [SkillCost(12, 8)]
        [SkillUsableUntrained(false)]
        LifeMagic,

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Self, 4)]
        [SkillCost(16, 12)]
        [SkillUsableUntrained(false)]
        WarMagic,

        [AttributeFormula(AttributeCache.Undef)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(true)]
        Leadership,

        [AttributeFormula(AttributeCache.Undef)]
        [SkillCost(0, 2, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Loyalty,

        [AttributeFormula(AttributeCache.Coordination | AttributeCache.Focus, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(false)]
        Fletching,

        [AttributeFormula(AttributeCache.Coordination | AttributeCache.Focus, 3)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(false)]
        Alchemy,

        [AttributeFormula(AttributeCache.Coordination | AttributeCache.Focus, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(false)]
        Cooking,

        [AttributeFormula(AttributeCache.Undef)]
        [SkillCost(0, CanSpecialize = false, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Salvaging,

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Coordination, 3)]
        [SkillCost(8, 8)]
        [SkillUsableUntrained(true)]
        TwoHandedCombat,

        Gearcraft,           /* Retired */

        [AttributeFormula(AttributeCache.Focus | AttributeCache.Self, 4)]
        [SkillCost(16, 12)]
        [SkillUsableUntrained(false)]
        VoidMagic,

        [AttributeFormula(AttributeCache.Coordination | AttributeCache.Strength, 3)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(true)]
        HeavyWeapons,

        [AttributeFormula(AttributeCache.Coordination | AttributeCache.Strength, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(true)]
        LightWeapons,

        [AttributeFormula(AttributeCache.Coordination | AttributeCache.Quickness, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(true)]
        FinesseWeapons,

        [AttributeFormula(AttributeCache.Coordination, 2)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(true)]
        MissileWeapons,

        [AttributeFormula(AttributeCache.Coordination | AttributeCache.Strength, 2)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(true)]
        Shield,

        [AttributeFormula(AttributeCache.Coordination, 3, AbilityMultiplier = 2)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(false)]
        DualWield,

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Quickness, 3)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        Recklessness,

        [AttributeFormula(AttributeCache.Coordination | AttributeCache.Quickness, 3)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        SneakAttack,

        [AttributeFormula(AttributeCache.Strength | AttributeCache.Coordination, 3)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(false)]
        DirtyFighting,

        Challenge,          /* Unimplemented */

        [AttributeFormula(AttributeCache.Endurance | AttributeCache.Self, 3)]
        [SkillCost(8, 4)]
        [SkillUsableUntrained(false)]
        Summoning
    }

    public static class SkillExtensions
    {
        public static AttributeFormulaAttribute GetFormula(this Skill skill)
        {
            return skill.GetAttributeOfType<AttributeFormulaAttribute>();
        }

        public static SkillCostAttribute GetCost(this Skill skill)
        {
            return skill.GetAttributeOfType<SkillCostAttribute>();
        }

        public static SkillUsableUntrainedAttribute GetUsability(this Skill skill)
        {
            return skill.GetAttributeOfType<SkillUsableUntrainedAttribute>();
        }

        /// <summary>
        /// Will add a space infront of capital letter words in a string
        /// </summary>
        /// <param name="skill"></param>
        /// <returns>string with spaces infront of capital letters</returns>
        public static string ToSentence(this Skill skill)
        {
            return new string(skill.ToString().ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray());
        }
    }
}
