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

            // ValidLocations has already been verified prior to this, so we can be safe casting it -- it won't null
            string gearPlatingName = GetGearPlatingName((EquipMask)target.ValidLocations);

            player.UpdateProperty(target, PropertyInt.HeritageSpecificArmor, (int)HeritageGroup.Gearknight);
            player.UpdateProperty(target, PropertyDataId.IconOverlay, CorePlatingGearOverlay);
            player.UpdateProperty(target, PropertyString.GearPlatingName, gearPlatingName);
            player.UpdateProperty(target, PropertyString.Use, "This Aetherium core plating installs into the frame of a Gear Knight to strengthen it.");

            target.SaveBiotaToDatabase();

            player.SendUseDoneEvent();
        }

        public static string GetGearPlatingName(EquipMask locations)
        {
            var slotName = ""; 
            string platingType;

            // Underoos are called "Underplating". Just make sure we ignore shoes.
            if ((locations & (EquipMask.ChestWear | EquipMask.AbdomenWear | EquipMask.UpperArmWear | EquipMask.LowerArmWear | EquipMask.UpperLegWear | EquipMask.LowerLegWear)) != 0
                && (locations & EquipMask.FootWear) == 0)
            {
                platingType = "Underplating";

                if ((locations & EquipMask.ChestWear) != 0)
                {
                    slotName = " Upper Body ";
                }
                else
                {
                    slotName = " Lower Body ";
                }
            }
            else
            {
                platingType = "Plating";

                switch (locations)
                {
                    case EquipMask.HeadWear:
                        slotName = " Helm ";
                        break;
                    case EquipMask.ChestArmor | EquipMask.AbdomenArmor:
                        slotName = " Cuirass ";
                        break;
                    case EquipMask.ChestArmor:
                    case EquipMask.ChestArmor | EquipMask.AbdomenArmor | EquipMask.UpperArmArmor:
                        slotName = " Chest ";
                        break;
                    case EquipMask.ChestArmor | EquipMask.UpperArmArmor: // No logs found for this exact combo, but "Shirt Mesh" was used for Chainmail Shirts, which falls under "Coat" now
                    case EquipMask.HeadWear | EquipMask.ChestArmor | EquipMask.UpperArmArmor: // No logs found for this exact combo, but "Shirt Mesh" was used for Chainmail Shirts, which falls under "Coat" now
                        slotName = " Shirt ";
                        platingType = "Mesh";
                        break;
                    case EquipMask.ChestArmor | EquipMask.AbdomenArmor | EquipMask.UpperArmArmor | EquipMask.LowerArmArmor:
                        slotName = " Hauberk ";
                        break;
                    case EquipMask.ChestArmor | EquipMask.LowerArmArmor | EquipMask.UpperArmArmor | EquipMask.HeadWear: // No logs found for this combo
                    case EquipMask.ChestArmor | EquipMask.UpperArmArmor | EquipMask.LowerArmArmor:
                        slotName = " Coat ";
                        break;
                    case EquipMask.AbdomenArmor:
                    case EquipMask.AbdomenArmor | EquipMask.UpperLegArmor: // No logs found for this combo, only currently applies to "Leather Shorts" WCID 25650, which appears to be a bug in the data
                        slotName = " Girth ";
                        break;
                    case EquipMask.UpperArmArmor:
                        slotName = " Pauldron ";
                        break;
                    case EquipMask.LowerArmArmor:
                        slotName = " Bracer ";
                        break;
                    case EquipMask.UpperArmArmor | EquipMask.LowerArmArmor:
                        slotName = " Sleeve ";
                        break;
                    case EquipMask.HandWear:
                        slotName = " Gauntlet ";
                        break;
                    case EquipMask.UpperLegArmor:
                        slotName = " Tasset ";
                        break;
                    case EquipMask.LowerLegArmor:
                        slotName = " Greaves ";
                        break;
                    case EquipMask.UpperLegArmor | EquipMask.LowerLegArmor:
                        slotName = " Leg ";
                        break;
                    case EquipMask.AbdomenArmor | EquipMask.UpperLegArmor | EquipMask.LowerLegArmor:
                        slotName = " Pants ";
                        platingType = "Mesh";
                        break;
                    case EquipMask.FootWear:
                    case EquipMask.LowerLegWear | EquipMask.FootWear:
                        slotName = " Solleret ";
                        break;
                    case EquipMask.Armor:
                    case EquipMask.Armor | EquipMask.HandWear: // No logs found for this combo, only Ursuin Guise
                    case EquipMask.Armor | EquipMask.HeadWear | EquipMask.HandWear: // No logs found for this combo, Guises/Costumes
                    case EquipMask.ChestArmor | EquipMask.UpperArmArmor | EquipMask.LowerArmArmor | EquipMask.UpperLegArmor | EquipMask.LowerLegArmor: // Swamp Lord's War Pain, WCID 27889
                    case EquipMask.HeadWear | EquipMask.Armor:
                        slotName = " Body ";
                        break;
                }
            }

            return $"Core{slotName}{platingType}";
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
