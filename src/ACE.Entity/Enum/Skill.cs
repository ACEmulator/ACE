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
        Axe,                 /* Retired */
        Bow,                 /* Retired */
        Crossbow,            /* Retired */
        Dagger,              /* Retired */
        Mace,                /* Retired */

        [AbilityFormula(Ability.Quickness | Ability.Coordination, 3)]
        [SkillCost(10, 10)]
        [SkillUsableUntrained(true)]
        MeleeDefense,

        [AbilityFormula(Ability.Quickness | Ability.Coordination, 5)]
        [SkillCost(6, 4)]
        [SkillUsableUntrained(true)]
        MissileDefense,

        Sling,               /* Retired */
        Spear,               /* Retired */
        Staff,               /* Retired */
        Sword,               /* Retired */
        ThrownWeapon,        /* Retired */
        UnarmedCombat,       /* Retired */

        [AbilityFormula(Ability.Focus, 3)]
        [SkillCost(0, 2, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        ArcaneLore,

        [AbilityFormula(Ability.Focus | Ability.Self, 7)]
        [SkillCost(0, 12, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        MagicDefense,

        [AbilityFormula(Ability.Focus | Ability.Self, 6)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(false)]
        ManaConversion,

        Spellcraft,          /* Unimplemented */

        [AbilityFormula(Ability.Focus | Ability.Self, 6)]
        [SkillCost(2, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        ItemTinkering,

        [AbilityFormula(Ability.None)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(false)]
        AssessPerson,

        [AbilityFormula(Ability.None)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        Deception,

        [AbilityFormula(Ability.Focus | Ability.Coordination, 3)]
        [SkillCost(6, 4)]
        [SkillUsableUntrained(false)]
        Healing,

        [AbilityFormula(Ability.Strength | Ability.Coordination, 2)]
        [SkillCost(0, 4, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Jump,

        [AbilityFormula(Ability.Focus | Ability.Coordination, 3)]
        [SkillCost(6, 4)]
        [SkillUsableUntrained(false)]
        Lockpick,

        [AbilityFormula(Ability.Quickness)]
        [SkillCost(0, 4, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Run,

        Awareness,           /* Unimplemented */
        ArmsAndArmorRepair,  /* Unimplemented */

        [AbilityFormula(Ability.None)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        AssessCreature,

        [AbilityFormula(Ability.Focus | Ability.Strength, 2)]
        [SkillCost(4, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        WeaponTinkering,

        [AbilityFormula(Ability.Focus | Ability.Endurance, 2)]
        [SkillCost(4, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        ArmorTinkering,

        [AbilityFormula(Ability.Focus)]
        [SkillCost(4, CanSpecialize = false)]
        [SkillUsableUntrained(false)]
        MagicItemTinkering,

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(8, 8)]
        [SkillUsableUntrained(false)]
        CreatureEnchantment,

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(8, 8)]
        [SkillUsableUntrained(false)]
        ItemEnchantment,

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(12, 8)]
        [SkillUsableUntrained(false)]
        LifeMagic,

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(16, 12)]
        [SkillUsableUntrained(false)]
        WarMagic,

        [AbilityFormula(Ability.None)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(true)]
        Leadership,

        [AbilityFormula(Ability.None)]
        [SkillCost(0, 2, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Loyalty,

        [AbilityFormula(Ability.Coordination | Ability.Focus, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(false)]
        Fletching,

        [AbilityFormula(Ability.Coordination | Ability.Focus, 3)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(false)]
        Alchemy,

        [AbilityFormula(Ability.Coordination | Ability.Focus, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(false)]
        Cooking,

        [AbilityFormula(Ability.None)]
        [SkillCost(0, CanSpecialize = false, TrainsFree = true)]
        [SkillUsableUntrained(true)]
        Salvaging,

        [AbilityFormula(Ability.Strength | Ability.Coordination, 3)]
        [SkillCost(8, 8)]
        [SkillUsableUntrained(true)]
        TwoHandedCombat,

        Gearcraft,           /* Retired */

        [AbilityFormula(Ability.Focus | Ability.Self, 4)]
        [SkillCost(16, 12)]
        [SkillUsableUntrained(false)]
        VoidMagic,

        [AbilityFormula(Ability.Coordination | Ability.Strength, 3)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(true)]
        HeavyWeapons,

        [AbilityFormula(Ability.Coordination | Ability.Strength, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(true)]
        LightWeapons,

        [AbilityFormula(Ability.Coordination | Ability.Quickness, 3)]
        [SkillCost(4, 4)]
        [SkillUsableUntrained(true)]
        FinesseWeapons,

        [AbilityFormula(Ability.Coordination, 2)]
        [SkillCost(6, 6)]
        [SkillUsableUntrained(true)]
        MissileWeapons,

        [AbilityFormula(Ability.Coordination | Ability.Strength, 2)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(true)]
        Shield,

        [AbilityFormula(Ability.Coordination, 3, AbilityMultiplier = 2)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(false)]
        DualWield,

        [AbilityFormula(Ability.Strength | Ability.Quickness, 3)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        Recklessness,

        [AbilityFormula(Ability.Coordination | Ability.Quickness, 3)]
        [SkillCost(4, 2)]
        [SkillUsableUntrained(false)]
        SneakAttack,

        [AbilityFormula(Ability.Strength | Ability.Coordination, 3)]
        [SkillCost(2, 2)]
        [SkillUsableUntrained(false)]
        DirtyFighting,

        Challenge,          /* Unimplemented */

        [AbilityFormula(Ability.Endurance | Ability.Self, 3)]
        [SkillCost(8, 4)]
        [SkillUsableUntrained(false)]
        Summoning
    }

    public static class SkillExtensions
    {
        public static AbilityFormulaAttribute GetFormula(this Skill skill)
        {
            return Enum.EnumHelper.GetAttributeOfType<AbilityFormulaAttribute>(skill);
        }

        public static SkillCostAttribute GetCost(this Skill skill)
        {
            return Enum.EnumHelper.GetAttributeOfType<SkillCostAttribute>(skill);
        }

        public static SkillUsableUntrainedAttribute GetUsability(this Skill skill)
        {
            return Enum.EnumHelper.GetAttributeOfType<SkillUsableUntrainedAttribute>(skill);
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

    public enum SkillStatus : uint
    {
        Inactive,
        Untrained,
        Trained,
        Specialized
    }
}
