using System;

using log4net;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class CorePlating
    {
        // http://acpedia.org/wiki/Announcements_-_2010/06_-_Shifting_Gears#Gear_Knights
        // http://acpedia.org/wiki/Core_Plating_Integrator
        // http://acpedia.org/wiki/Core_Plating_Deintegrator

        public const uint CorePlatingIntegrator   = 42979;
        public const uint CorePlatingDeintegrator = 43022;

        public static bool IsCorePlatingDevice(WorldObject wo)
        {
            return wo.WeenieClassId == CorePlatingIntegrator || wo.WeenieClassId == CorePlatingDeintegrator;
        }

        public static bool IsIntegrator(uint wcid)
        {
            return wcid == CorePlatingIntegrator;
        }

        public static bool IsDeintegrator(uint wcid)
        {
            return wcid == CorePlatingDeintegrator;
        }

        /// <summary>
        /// The player uses a Core Plating device on a piece of armor or clothing
        /// </summary>
        public static void UseObjectOnTarget(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"CorePlating.UseObjectOnTarget({player.Name}, {source.Name}, {target.Name})");

            if (player.IsBusy)
            {
                player.SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            var allowCraftInCombat = PropertyManager.GetBool("allow_combat_mode_crafting").Item;

            if (!allowCraftInCombat && player.CombatMode != CombatMode.NonCombat)
            {
                player.SendUseDoneEvent(WeenieError.YouMustBeInPeaceModeToTrade);
                return;
            }

            // verify use requirements
            var useError = VerifyUseRequirements(player, source, target);
            if (useError != WeenieError.None)
            {
                player.SendUseDoneEvent(useError);
                return;
            }

            var motionCommand = MotionCommand.ClapHands;

            var actionChain = new ActionChain();
            var nextUseTime = 0.0f;

            player.IsBusy = true;

            if (allowCraftInCombat && player.CombatMode != CombatMode.NonCombat)
            {
                // Drop out of combat mode.  This depends on the server property "allow_combat_mode_craft" being True.
                // If not, this action would have aborted due to not being in NonCombat mode.
                var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                actionChain.AddDelaySeconds(stanceTime);

                nextUseTime += stanceTime;
            }

            var motion = new Motion(player, motionCommand);
            var currentStance = player.CurrentMotionState.Stance; // expected to be MotionStance.NonCombat
            var clapTime = Physics.Animation.MotionTable.GetAnimationLength(player.MotionTableId, currentStance, motionCommand);

            actionChain.AddAction(player, () => player.SendMotionAsCommands(motionCommand, currentStance));
            actionChain.AddDelaySeconds(clapTime);

            nextUseTime += clapTime;

            actionChain.AddAction(player, () =>
            {
                // re-verify
                var useError = VerifyUseRequirements(player, source, target);
                if (useError != WeenieError.None)
                {
                    player.SendUseDoneEvent(useError);
                    return;
                }

                if (IsIntegrator(source.WeenieClassId))
                    Integrate(player, source, target);
                else if (IsDeintegrator(source.WeenieClassId))
                    Deintegrate(player, source, target);
                else
                    player.SendUseDoneEvent(WeenieError.CraftGeneralErrorNoUiMsg);
            });

            //player.EnqueueMotion(actionChain, MotionCommand.Ready);

            actionChain.AddAction(player, () => player.IsBusy = false);

            actionChain.EnqueueChain();

            player.NextUseTime = DateTime.UtcNow.AddSeconds(nextUseTime);
        }

        public static WeenieError VerifyUseRequirements(Player player, WorldObject source, WorldObject target)
        {
            if (source == target)
            {
                player.SendTransientError($"You can't use the {source.Name} on itself.");
                return WeenieError.YouDoNotPassCraftingRequirements;
            }

            // ensure both source and target are in player's inventory
            if (player.FindObject(source.Guid.Full, Player.SearchLocations.MyInventory) == null)
                return WeenieError.YouDoNotPassCraftingRequirements;

            if (player.FindObject(target.Guid.Full, Player.SearchLocations.MyInventory) == null)
                return WeenieError.YouDoNotPassCraftingRequirements;

            if (source.WeenieClassId != CorePlatingIntegrator && source.WeenieClassId != CorePlatingDeintegrator)
                return WeenieError.YouDoNotPassCraftingRequirements;

            if ((target.ValidLocations & (EquipMask.Clothing | EquipMask.Armor)) == 0)
            {
                player.SendTransientError($"You can't use the {source.Name} on {target.Name} because it is not a piece of armor or clothing.");
                return WeenieError.YouDoNotPassCraftingRequirements;
            }

            if (IsIntegrator(source.WeenieClassId))
            {
                if (target.GetProperty(PropertyInt.HeritageSpecificArmor) == (int)HeritageGroup.Gearknight)
                {
                    //player.SendTransientError($"This armor has already been integrated into gear plating.");
                    player.SendMessage($"This armor has already been integrated into gear plating.");
                    return WeenieError.YouDoNotPassCraftingRequirements;
                }

                if (target.GetProperty(PropertyInt.HeritageSpecificArmor) > 0)
                {
                    //player.SendTransientError($"This armor cannot be integrated into gear plating as it is created specifically for another race.");
                    player.SendMessage($"This armor cannot be integrated into gear plating as it is created specifically for another race.");
                    return WeenieError.YouDoNotPassCraftingRequirements;
                }
            }
            else if (IsDeintegrator(source.WeenieClassId))
            {
                if (target.GetProperty(PropertyInt.HeritageSpecificArmor) != (int)HeritageGroup.Gearknight)
                {
                    //player.SendTransientError($"This armor has not been integrated into gear plating.");
                    player.SendMessage($"This armor has not been integrated into gear plating.");
                    return WeenieError.YouDoNotPassCraftingRequirements;
                }
            }

            return WeenieError.None;
        }

        public const uint CorePlatingGearOverlay = 0x06006D70;

        public static void Integrate(Player player, WorldObject source, WorldObject target)
        {
            player.SendMessage("Your integrator forges the piece into gear plating for a Gear Knight.");

            if (target.IconOverlayId != 0)
                target.IconOverlaySecondary = target.IconOverlayId;

            var slotName = "";
            switch (target.ValidLocations)
            {
                case EquipMask.HeadWear:
                    slotName = " Helm ";
                    break;
                case EquipMask.ChestWear:
                case EquipMask.ChestArmor:
                case EquipMask.ChestWear | EquipMask.AbdomenWear | EquipMask.UpperArmWear:
                case EquipMask.ChestArmor | EquipMask.AbdomenArmor | EquipMask.UpperArmArmor:
                    slotName = " Chest ";
                    break;
                case EquipMask.ChestWear | EquipMask.AbdomenWear | EquipMask.UpperArmWear | EquipMask.LowerArmWear:
                case EquipMask.ChestArmor | EquipMask.AbdomenArmor | EquipMask.UpperArmArmor | EquipMask.LowerArmArmor:
                    slotName = " Hauberk ";
                    break;
                // no pcaps for "shirts" found
                //case EquipMask.ChestWear | EquipMask.AbdomenWear | EquipMask.UpperArmWear:
                //case EquipMask.ChestArmor | EquipMask.AbdomenArmor | EquipMask.UpperArmArmor:
                //    slotName = " Shirt ";
                //    break;
                case EquipMask.AbdomenWear:
                case EquipMask.AbdomenArmor:
                    slotName = " Girth ";
                    break;
                case EquipMask.UpperArmWear:
                case EquipMask.UpperArmArmor:
                    slotName = " Pauldron ";
                    break;
                case EquipMask.LowerArmWear:
                case EquipMask.LowerArmArmor:
                    slotName = " Bracer ";
                    break;
                case EquipMask.UpperArmWear | EquipMask.LowerArmWear:
                case EquipMask.UpperArmArmor  | EquipMask.LowerArmArmor:
                    slotName = " Sleeve ";
                    break;
                case EquipMask.HandWear:
                    slotName = " Gauntlet ";
                    break;
                case EquipMask.UpperLegWear:
                case EquipMask.UpperLegArmor:
                    slotName = " Tasset ";
                    break;
                case EquipMask.LowerLegWear:
                case EquipMask.LowerLegArmor:
                    slotName = " Greaves ";
                    break;
                case EquipMask.UpperLegWear | EquipMask.LowerLegWear:
                case EquipMask.UpperLegArmor | EquipMask.LowerLegArmor:
                case EquipMask.AbdomenWear | EquipMask.UpperLegWear | EquipMask.LowerLegWear:
                case EquipMask.AbdomenArmor | EquipMask.UpperLegArmor | EquipMask.LowerLegArmor:
                    slotName = " Leg ";
                    break;                
                case EquipMask.FootWear:
                case EquipMask.LowerLegWear | EquipMask.FootWear:
                    slotName = " Solleret ";
                    break;
                // pcap showed boots were called solleret as well
                //case EquipMask.LowerLegWear | EquipMask.FootWear:
                //    slotName = " Boot ";
                //    break;
                case EquipMask.Armor:
                case EquipMask.HeadWear | EquipMask.Armor:
                    slotName = " Body ";
                    break;
            }

            player.UpdateProperty(target, PropertyInt.HeritageSpecificArmor, (int)HeritageGroup.Gearknight);
            player.UpdateProperty(target, PropertyDataId.IconOverlay, CorePlatingGearOverlay);
            player.UpdateProperty(target, PropertyString.GearPlatingName, $"Core{slotName}Plating");
            player.UpdateProperty(target, PropertyString.Use, "This Aetherium core plating installs into the frame of a Gear Knight to strengthen it.");

            target.SaveBiotaToDatabase();

            player.SendUseDoneEvent();
        }

        public static void Deintegrate(Player player, WorldObject source, WorldObject target)
        {
            player.SendMessage("Your deintegrator restores the original form of this piece of gear.");

            player.UpdateProperty(target, PropertyInt.HeritageSpecificArmor, null);
            if (target.IconOverlayId == CorePlatingGearOverlay)
                player.UpdateProperty(target, PropertyDataId.IconOverlay, target.IconOverlaySecondary ?? null);
            player.UpdateProperty(target, PropertyString.GearPlatingName, null);

            var targetWeenie = Database.DatabaseManager.World.GetCachedWeenie(target.WeenieClassId);

            if (targetWeenie != null)
            {
                var useString = targetWeenie.GetProperty(PropertyString.Use);
                if (useString != null)
                    player.UpdateProperty(target, PropertyString.Use, useString);
                else
                    player.UpdateProperty(target, PropertyString.Use, null);
            }
            else
                player.UpdateProperty(target, PropertyString.Use, null);

            if (target.IconOverlaySecondary != null)
                target.IconOverlaySecondary = null;

            target.SaveBiotaToDatabase();

            player.SendUseDoneEvent();
        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
