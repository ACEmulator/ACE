using ACE.Entity.Enum.Properties;

namespace ACE.Entity.Enum
{
    public enum AugmentationType
    {
        None = 0,
        /// <summary>
        /// Reinforcement of the Lugians - +5 strength
        /// </summary>
        Strength = 1,
        /// <summary>
        /// Bleeargh's Fortitude - +5 endurance
        /// </summary>
        Endurance = 2,
        /// <summary>
        /// Oswald's Enhancement - +5 coordination
        /// </summary>
        Coordination = 3,
        /// <summary>
        /// Siraluun's Blessing - +5 quickness
        /// </summary>
        Quickness = 4,
        /// <summary>
        /// Enduring Calm - +5 focus
        /// </summary>
        Focus = 5,
        /// <summary>
        /// Steadfast Will - +5 self
        /// </summary>
        Self = 6,
        /// <summary>
        /// Ciandra's Essence - specialize salvaging
        /// </summary>
        Salvage = 7,
        /// <summary>
        /// Yoshi's Essence - specialize item tinkering
        /// </summary>
        ItemTinkering = 8,
        /// <summary>
        /// Jibril's Essence - specialize armor tinkering
        /// </summary>
        ArmorTinkering = 9,
        /// <summary>
        /// Celdiseth's Essence - specialize magic tinkering
        /// </summary>
        MagicItemTinkering = 10,
        /// <summary>
        /// Koga's Essence - specialize weapon tinkering
        /// </summary>
        WeaponTinkering = 11,
        /// <summary>
        /// Shadow of The Seventh Mule - extra pack slot
        /// </summary>
        PackSlot = 12, 
        /// <summary>
        /// Might of The Seventh Mule - increased burden capacity
        /// </summary>
        BurdenLimit = 13,
        /// <summary>
        /// Clutch of the Miser - reduced death item loss
        /// </summary>
        DeathItemLoss = 14,
        /// <summary>
        /// Enduring Enchantment - retain spells on death
        /// </summary>
        DeathSpellLoss = 15,
        /// <summary>
        /// Critical Protection - chance to nullify critical damage
        /// </summary>
        CritProtect = 16,
        /// <summary>
        /// Quick Learner - increased XP via hunting / killing creatures
        /// </summary>
        BonusXP = 17,
        /// <summary>
        /// Ciandra's Fortune - increase salvaging material output
        /// </summary>
        BonusSalvage = 18,
        /// <summary>
        /// Charmed Smith - increased chance of success for imbuing items
        /// </summary>
        ImbueChance = 19,
        /// <summary>
        /// Innate Renewal - faster vial regeneration rate
        /// </summary>
        RegenBonus = 20,
        /// <summary>
        /// Archmage's Endurance - increased spell duration
        /// </summary>
        SpellDuration = 21,
        /// <summary>
        /// Enhancement of the Blade Turner - reduced slash damage
        /// </summary>
        ResistSlash = 22,
        /// <summary>
        /// Enhancement of the Arrow Turner - reduced pierce damage
        /// </summary>
        ResistPierce = 23,
        /// <summary>
        /// Enhancement of the Mace Turner - reduced bludgeon damage
        /// </summary>
        ResistBludgeon = 24,
        /// <summary>
        /// Caustic Enhancement - reduced acid damage
        /// </summary>
        ResistAcid = 25,
        /// <summary>
        /// Fiery Enhancement - reduced fire damage
        /// </summary>
        ResistFire = 26,
        /// <summary>
        /// Icy Enhancement - reduced cold damage
        /// </summary>
        ResistCold = 27,
        /// <summary>
        /// Storm's Enhancement - reduced lightning damage
        /// </summary>
        ResistElectric = 28,
        /// <summary>
        /// Infused Creature Magic - foci for creature magic
        /// </summary>
        FociCreature = 29,
        /// <summary>
        /// Infused Item Magic - foci for item magic
        /// </summary>
        FociItem = 30,
        /// <summary>
        /// Infused Life Magic - foci for life magic
        /// </summary>
        FociLife = 31,
        /// <summary>
        /// Infused War Magic - foci for war magic
        /// </summary>
        FociWar = 32,
        /// <summary>
        /// Eye of the Remorseless - increased critical hit chance
        /// </summary>
        CritChance = 33,
        /// <summary>
        /// Hand of the Remorseless - increased critical hit damage
        /// </summary>
        CritDamage = 34,
        /// <summary>
        /// Master of The Steel Circle - +10 to all melee skills
        /// </summary>
        Melee = 35,
        /// <summary>
        /// Master of the Focused Eye - +10 to all missile skills
        /// </summary>
        Missile = 36,
        /// <summary>
        /// Master of The Five Fold Path - +10 to all magic skills
        /// </summary>
        Magic = 37,
        /// <summary>
        /// Frenzy of the Slayer - increased damage rating
        /// </summary>
        Damage = 38,
        /// <summary>
        /// Iron Skin of the Invincible - increased damage resistance rating
        /// </summary>
        DamageResist = 39,
        /// <summary>
        /// Jack of All Trades - +5 to all skills
        /// </summary>
        AllStats = 40,
        // missing 41?
        /// <summary>
        /// Infused Void Magic - foci for void magic
        /// </summary>
        FociVoid = 42,
    }

    public static class AugTypeHelper
    {
        /// <summary>
        /// Returns TRUE if this AugmentationType belongs to the 'innate attribute' family,
        /// which has a shared cap of 10
        /// </summary>
        public static bool IsAttribute(AugmentationType type)
        {
            return type >= AugmentationType.Strength && type <= AugmentationType.Self;
        }

        /// <summary>
        /// Returns TRUE if this AugmentationType belongs to the 'resistance' family,
        /// which has a shared cap of 2
        /// </summary>
        public static bool IsResist(AugmentationType type)
        {
            return type >= AugmentationType.ResistSlash && type <= AugmentationType.ResistElectric;
        }

        /// <summary>
        /// Returns TRUE if this AugmentationType specializes a skill
        /// </summary>
        public static bool IsSkill(AugmentationType type)
        {
            switch (type)
            {
                case AugmentationType.Salvage:
                case AugmentationType.ItemTinkering:
                case AugmentationType.ArmorTinkering:
                case AugmentationType.MagicItemTinkering:
                case AugmentationType.WeaponTinkering:
                    return true;
                default:
                    return false;
            }
        }

        public static PropertyAttribute GetAttribute(AugmentationType type)
        {
            switch (type)
            {
                case AugmentationType.Strength:
                    return PropertyAttribute.Strength;
                case AugmentationType.Endurance:
                    return PropertyAttribute.Endurance;
                case AugmentationType.Coordination:
                    return PropertyAttribute.Coordination;
                case AugmentationType.Quickness:
                    return PropertyAttribute.Quickness;
                case AugmentationType.Focus:
                    return PropertyAttribute.Focus;
                case AugmentationType.Self:
                    return PropertyAttribute.Self;
                default:
                    return PropertyAttribute.Undef;
            }
        }

        public static Skill GetSkill(AugmentationType type)
        {
            switch (type)
            {
                case AugmentationType.Salvage:
                    return Skill.Salvaging;
                case AugmentationType.ItemTinkering:
                    return Skill.ItemTinkering;
                case AugmentationType.ArmorTinkering:
                    return Skill.ArmorTinkering;
                case AugmentationType.MagicItemTinkering:
                    return Skill.MagicItemTinkering;
                case AugmentationType.WeaponTinkering:
                    return Skill.WeaponTinkering;
                default:
                    return Skill.None;
            }
        }

        public static PlayScript GetEffect(AugmentationType type)
        {
            if (IsAttribute(type))
                return PlayScript.AugmentationUseAttribute;
            else if (IsResist(type))
                return PlayScript.AugmentationUseResistances;
            else if (IsSkill(type))
                return PlayScript.AugmentationUseSkill;
            else
                return PlayScript.AugmentationUseOther;
        }
    }
}
