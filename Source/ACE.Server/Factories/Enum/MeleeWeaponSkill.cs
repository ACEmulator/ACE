using ACE.Entity.Enum;

namespace ACE.Server.Factories.Enum
{
    public enum MeleeWeaponSkill
    {
        Undef,
        HeavyWeapons,
        LightWeapons,
        FinesseWeapons,
        TwoHandedCombat,
    };

    public static class MeleeWeaponSkillExtensions
    {
        public static MeleeWeaponSkill ToMeleeWeaponSkill(this Skill skill)
        {
            switch (skill)
            {
                case Skill.HeavyWeapons:
                    return MeleeWeaponSkill.HeavyWeapons;
                case Skill.LightWeapons:
                    return MeleeWeaponSkill.LightWeapons;
                case Skill.FinesseWeapons:
                    return MeleeWeaponSkill.FinesseWeapons;
                case Skill.TwoHandedCombat:
                    return MeleeWeaponSkill.TwoHandedCombat;
            }
            return MeleeWeaponSkill.Undef;
        }

        public static string GetScriptName(this MeleeWeaponSkill weaponSkill)
        {
            switch (weaponSkill)
            {
                case MeleeWeaponSkill.HeavyWeapons:
                    return "heavy";
                case MeleeWeaponSkill.LightWeapons:
                    return "light";
                case MeleeWeaponSkill.FinesseWeapons:
                    return "finesse";
                case MeleeWeaponSkill.TwoHandedCombat:
                    return "two_handed";
            }
            return null;
        }

        public static string GetScriptName_Combined(this MeleeWeaponSkill weaponSkill)
        {
            switch (weaponSkill)
            {
                case MeleeWeaponSkill.HeavyWeapons:
                    return "heavy";
                case MeleeWeaponSkill.LightWeapons:
                case MeleeWeaponSkill.FinesseWeapons:
                    return "light_finesse";
                case MeleeWeaponSkill.TwoHandedCombat:
                    return "two_handed";
            }
            return null;
        }
    }
}
