using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

using HarmonyLib;

namespace SocietyTailoring;

[HarmonyPatch]
public static class SocietyTailoringPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Tailoring), nameof(Tailoring.VerifyUseRequirements),
        new[] { typeof(Player), typeof(WorldObject), typeof(WorldObject) })]
    public static bool AllowSocietyArmor(Player player, WorldObject source, WorldObject target, ref WeenieError __result)
    {
        if (source == target ||
            player.FindObject(source.Guid.Full, Player.SearchLocations.MyInventory) is null ||
            player.FindObject(target.Guid.Full, Player.SearchLocations.MyInventory) is null)
        {
            __result = WeenieError.YouDoNotPassCraftingRequirements;
            return false;
        }

        if (target.Retained)
        {
            player.Session.Network.EnqueueSend(new GameMessageSystemChat(
                "You must use Sandstone Salvage to remove the retained property before tailoring.",
                ChatMessageType.Craft));
            __result = WeenieError.YouDoNotPassCraftingRequirements;
            return false;
        }

        // This intentionally omits stock ACE's IsSocietyArmor rejection. All
        // other checks above mirror Tailoring.VerifyUseRequirements in the
        // ACE version pinned by this launcher.
        __result = WeenieError.None;
        return false;
    }
}
