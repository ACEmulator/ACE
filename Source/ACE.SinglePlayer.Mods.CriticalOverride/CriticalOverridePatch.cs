using ACE.Server.Entity;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;

using HarmonyLib;

namespace CriticalOverride;

[HarmonyPatch]
public static class CriticalOverridePatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldObject), nameof(WorldObject.GetWeaponCriticalChance),
        new[] { typeof(WorldObject), typeof(Creature), typeof(CreatureSkill), typeof(Creature) })]
    public static bool OverridePhysicalCriticalChance(Creature target, ref float __result)
    {
        if (target is Player)
            return true;

        __result = Mod.Settings.CritChance;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldObject), nameof(WorldObject.GetWeaponMagicCritFrequency),
        new[] { typeof(WorldObject), typeof(Creature), typeof(CreatureSkill), typeof(Creature) })]
    public static bool OverrideMagicCriticalChance(Creature target, ref float __result)
    {
        if (target is Player)
            return true;

        __result = Mod.Settings.MagicCritChance;
        return false;
    }
}
