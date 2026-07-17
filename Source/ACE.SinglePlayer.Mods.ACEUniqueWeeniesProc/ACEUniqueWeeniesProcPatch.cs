using System.Linq;

using ACE.Server.WorldObjects;

using HarmonyLib;

namespace ACEUniqueWeeniesProc;

[HarmonyPatch]
public static class ACEUniqueWeeniesProcPatch
{
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    [HarmonyPatch(typeof(WorldObject), nameof(WorldObject.TryProcEquippedItems),
        new[] { typeof(WorldObject), typeof(Creature), typeof(bool), typeof(WorldObject) })]
    public static bool UseUniqueEquipmentProcFilter(WorldObject __instance, WorldObject attacker,
        Creature target, bool selfTarget, WorldObject? weapon)
    {
        // Preserve stock ACE handling for a proc directly on the object, the
        // active weapon, and monster missile projectiles.
        if (__instance.HasProc && __instance.ProcSpellSelfTargeted == selfTarget)
            __instance.TryProcItem(attacker, target, selfTarget);

        if (weapon is { HasProc: true } && weapon.ProcSpellSelfTargeted == selfTarget)
            weapon.TryProcItem(attacker, target, selfTarget);

        if (attacker != __instance && attacker.HasProc && attacker.ProcSpellSelfTargeted == selfTarget)
            attacker.TryProcItem(attacker, target, selfTarget);

        // ACE normally limits this equipped-item pass to Aetheria. The
        // ACEUniqueWeenies content expects procs on jewelry, armor, and other
        // equipped objects. Cloak weave proc type 1 remains excluded exactly
        // as documented by the content repository.
        if (attacker is Creature wielder)
        {
            var equippedProcItems = wielder.EquippedObjects.Values.Where(item =>
                IsEligibleEquippedProc(item.HasProc, item.CloakWeaveProc,
                    item.ProcSpellSelfTargeted, selfTarget));

            foreach (var item in equippedProcItems)
                item.TryProcItem(attacker, target, selfTarget);
        }

        return false;
    }

    public static bool IsEligibleEquippedProc(bool hasProc, int? cloakWeaveProc,
        bool procSpellSelfTargeted, bool selfTarget) =>
        hasProc && cloakWeaveProc != 1 && procSpellSelfTargeted == selfTarget;
}
