using System;
using System.Collections.Generic;
using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Factories.Tables;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using log4net;

namespace ACE.Server.Entity
{
    public class Tailoring
    {
        // http://acpedia.org/wiki/Tailoring
        // https://asheron.fandom.com/wiki/Tailoring

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // tailoring kits
        public const uint ArmorTailoringKit = 41956;
        public const uint WeaponTailoringKit = 51445;

        public const uint ArmorMainReductionTool = 42622;
        public const uint ArmorLowerReductionTool = 44879;
        public const uint ArmorMiddleReductionTool = 44880;

        public const uint ArmorLayeringToolTop = 42724;
        public const uint ArmorLayeringToolBottom = 42726;

        //public const uint MorphGemArmorLevel = 4200022;
        public const uint MorphGemValue = 4200023;
        //public const uint MorphGemArmorWork = 4200024;
        public const uint MorphGemArcane = 4200026;
        //public const uint MorphGemRandomEpic = 4200027;
        //public const uint MorphGemRandomSet = 4200028;
        public const uint MorphGemRemoveMissileDReq = 480484;
        public const uint MorphGemRemoveMeleeDReq = 480483;
        public const uint MorphGemRandomizeWeaponImbue = 480486;
        public const uint MorphGemRemovePlayerReq = 480485;
        public const uint MorphGemSlayerRandom = 480610;
        public const uint MorphGemRemoveLevelReq = 480609;

        public const int MorphGemMinValue = 20000;

        

        // Some WCIDs have Overlay Icons that need to be removed (e.g. Olthoi Alduressa Gauntlets or Boots)
        // There are other examples not here, like some stamped shields that might need to be added, as well.
        private static Dictionary<uint, int> ArmorOverlayIcons = new Dictionary<uint, int>{
            // These are from cache.bin 
            {22551, 100673784}, // Atlatl Tattoo
            {22552, 100673758}, // Axe Tattoo
            {22553, 100673759}, // Bow Tattoo
            {22554, 100673762}, // Crossbow Tattoo
            {22555, 100673763}, // Dagger Tattoo
            {22556, 100673774}, // Mace Tattoo
            {22557, 100673775}, // Magic Defense Tattoo
            {22558, 100673777}, // Mana Conversion Tattoo
            {22559, 100673778}, // Melee Defense Tattoo
            {22560, 100673779}, // Missile Defense Tattoo
            {22561, 100673781}, // Spear Tattoo
            {22562, 100673782}, // Staff Tattoo
            {22563, 100673783}, // Sword Tattoo
            {22564, 100673785}, // Unarmed Tattoo
            {31394, 100691319}, // Circle of Raven Might

            // These items were stampable and could have had a number of different icons
            {25811, 0}, // Shield of Power
            {25843, 0}, // Nefane Shield

            // From pcaps
            {37187, 100690144}, // Olthoi Alduressa Gauntlets
            {37207, 100690146}, // Olthoi Alduressa Boots
            {41198, 100690144}, // Gauntlets of Darkness
            {41201, 100690146}, // Sollerets of Darkness
        };

        // thanks for phenyl naphthylamine for a lot the initial work here!
        public static void UseObjectOnTarget(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"Tailoring.UseObjectOnTarget({player.Name}, {source.Name}, {target.Name})");

            // verify use requirements
            var useError = VerifyUseRequirements(player, source, target);
            if (useError != WeenieError.None)
            {
                player.SendUseDoneEvent(useError);
                return;
            }

            var animTime = 0.0f;

            var actionChain = new ActionChain();

            // handle switching to peace mode
            if (player.CombatMode != CombatMode.NonCombat)
            {
                var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                actionChain.AddDelaySeconds(stanceTime);

                animTime += stanceTime;
            }

            // perform clapping motion
            animTime += player.EnqueueMotion(actionChain, MotionCommand.ClapHands);

            actionChain.AddAction(player, () =>
            {
                // re-verify
                var useError = VerifyUseRequirements(player, source, target);
                if (useError != WeenieError.None)
                {
                    player.SendUseDoneEvent(useError);
                    return;
                }

                DoTailoring(player, source, target);
            });

            actionChain.EnqueueChain();

            player.NextUseTime = DateTime.UtcNow.AddSeconds(animTime);
        }

        public static WeenieError VerifyUseRequirements(Player player, WorldObject source, WorldObject target)
        {
            if (source == target)
                return WeenieError.YouDoNotPassCraftingRequirements;

            // ensure both source and target are in player's inventory
            if (player.FindObject(source.Guid.Full, Player.SearchLocations.MyInventory) == null)
                return WeenieError.YouDoNotPassCraftingRequirements;

            if (player.FindObject(target.Guid.Full, Player.SearchLocations.MyInventory) == null)
                return WeenieError.YouDoNotPassCraftingRequirements;

            // verify not retained item
            if (target.Retained)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat("You must use Sandstone Salvage to remove the retained property before tailoring.", ChatMessageType.Craft));
                return WeenieError.YouDoNotPassCraftingRequirements;
            }

            // verify not society armor
            if (source.IsSocietyArmor || target.IsSocietyArmor)
                return WeenieError.YouDoNotPassCraftingRequirements;

            return WeenieError.None;
        }

        public static void DoTailoring(Player player, WorldObject source, WorldObject target)
        {
            switch (source.WeenieClassId)
            {
                case ArmorTailoringKit:

                    TailorArmor(player, source, target);
                    return;

                case WeaponTailoringKit:

                    TailorWeapon(player, source, target);
                    return;

                case ArmorMainReductionTool:
                case ArmorLowerReductionTool:
                case ArmorMiddleReductionTool:

                    TailorReduceArmor(player, source, target);
                    return;

                case ArmorLayeringToolTop:
                case ArmorLayeringToolBottom:
                    TailorLayerArmor(player, source, target);
                    return;

                // intermediates
                case Heaume:             // helm
                case PlatemailGauntlets: // gauntlets
                case LeatherBoots:       // boots
                case LeatherVest:        // breastplate
                case YoroiGirth:         // girth
                case YoroiPauldrons:     // pauldrons
                case CeldonSleeves:      // vambraces
                case YoroiGreaves:       // tassets
                case YoroiLeggings:      // greaves
                case AmuliLeggings:      // lower-body multislot
                case WingedCoat:         // upper-body multislot
                case Tentacles:          // clothing or shield

                    ArmorApply(player, source, target);
                    return;

                case DarkHeart:

                    WeaponApply(player, source, target);
                    return;

                case MorphGemValue:
                case MorphGemArcane:
                case MorphGemRemoveMissileDReq:
                case MorphGemRemoveMeleeDReq:
                case MorphGemRandomizeWeaponImbue:
                case MorphGemRemovePlayerReq:
                case MorphGemSlayerRandom:
                case MorphGemRemoveLevelReq:
                    ApplyMorphGem(player, source, target);
                    return;
            }

            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
            return;
        }

        /// <summary>
        /// Consumes the source armor, and creates an intermediate tailoring kit
        /// to apply to the destination armor
        /// </summary>
        public static void TailorArmor(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"TailorArmor({player.Name}, {source.Name}, {target.Name})");

            var wcid = GetArmorWCID(target.ValidLocations ?? 0);
            if (wcid == null)
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            if (!HasAvailableSpace(player, source.WeenieClassId, target.WeenieClassId, wcid.Value))
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            var wo = WorldObjectFactory.CreateNewWorldObject(wcid.Value);

            SetArmorProperties(target, wo);

            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You tailor the appearance off an existing piece of armor.", ChatMessageType.Broadcast));

            Finalize(player, source, target, wo);
        }

        public static void SetCommonProperties(WorldObject source, WorldObject target)
        {
            // a lot of this was probably done with recipes and mutations in the original
            // here a lot is done directly in code..

            target.PaletteTemplate = source.PaletteTemplate;
            if (PropertyManager.GetBool("tailoring_intermediate_uieffects").Item)
                target.UiEffects = source.UiEffects;
            target.MaterialType = source.MaterialType;

            target.ObjScale = source.ObjScale;

            target.Shade = source.Shade;

            // This might not even be needed, but we'll do it anyways
            target.Shade2 = source.Shade2;
            target.Shade3 = source.Shade3;
            target.Shade4 = source.Shade4;

            target.LightsStatus = source.LightsStatus;
            target.Translucency = source.Translucency;

            target.SetupTableId = source.SetupTableId;
            target.PaletteBaseId = source.PaletteBaseId;
            target.ClothingBase = source.ClothingBase;

            target.PhysicsTableId = source.PhysicsTableId;
            target.SoundTableId = source.SoundTableId;

            target.Name = source.Name;
            target.LongDesc = LootGenerationFactory.GetLongDesc(target);

            target.IgnoreCloIcons = source.IgnoreCloIcons;
            target.IconId = source.IconId;
        }

        public static void SetArmorProperties(WorldObject source, WorldObject target)
        {
            SetCommonProperties(source, target);

            // ensure armor/clothing that covers head/hands/feet are cross-compatible
            // for something like shirt/breastplate, this will still be be prevented with ClothingPriority / CoverageMask check
            // (Outerwear vs. Underwear)
            target.TargetType = ItemType.Armor | ItemType.Clothing;

            target.ClothingPriority = source.ClothingPriority;
            target.Dyable = source.Dyable;

            // If this source item is one of the icons that contains an icon overlay as part of it, we will stash that icon in the
            // IconOverlaySecondary slot (it is unused) to be applied on the next step.
            if (ArmorOverlayIcons.ContainsKey(source.WeenieClassId) && source.IconOverlayId.HasValue)
                target.SetProperty(PropertyDataId.IconOverlaySecondary, (uint)source.IconOverlayId);

            // ObjDescOverride.Clear()
        }

        /// <summary>
        /// Applies the weapon properties to an in-between tailoring item, ready to be applied to a new weapon.
        /// </summary>
        public static void SetWeaponProperties(WorldObject source, WorldObject target)
        {
            SetCommonProperties(source, target);

            target.TargetType = source.ItemType;

            target.HookType = source.HookType;
            target.HookPlacement = source.HookPlacement;

            // These values are all set just for verification purposes. Likely originally handled by unique WCID and recipe system.
            if (source is MeleeWeapon)
            {
                target.DefaultCombatStyle = source.DefaultCombatStyle;  // unused currently, keeping this around in case its needed..
                target.W_AttackType = source.W_AttackType;
                target.W_WeaponType = source.W_WeaponType;
            }
            else if (source is MissileLauncher)
                target.DefaultCombatStyle = source.DefaultCombatStyle;

            target.W_DamageType = source.W_DamageType;
        }

        public static bool HasAvailableSpace(Player player, uint sourceWCID, uint targetWCID, uint resultWCID)
        {
            // ensure player has enough free inventory slots / container slots / available burden to mutate items
            var itemsToReceive = new ItemsToReceive(player);

            itemsToReceive.Remove(sourceWCID, 1);
            itemsToReceive.Remove(targetWCID, 1);
            itemsToReceive.Add(resultWCID, 1);

            if (itemsToReceive.PlayerExceedsLimits)
            {
                if (itemsToReceive.PlayerExceedsAvailableBurden)
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You are too encumbered to tailor that!"));
                else if (itemsToReceive.PlayerOutOfInventorySlots)
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You do not have enough pack space to tailor that!"));
                else if (itemsToReceive.PlayerOutOfContainerSlots)
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You do not have enough container slots to tailor that!"));

                return false;
            }

            return true;
        }

        public static void Finalize(Player player, WorldObject source, WorldObject target, WorldObject result)
        {
            player.TryConsumeFromInventoryWithNetworking(source, 1);
            player.TryConsumeFromInventoryWithNetworking(target, 1);

            // errors shouldn't be possible here, since the items were pre-validated, but just in case...
            if (!player.TryCreateInInventoryWithNetworking(result))
            {
                log.Error($"[TAILORING] Tailoring.Finalize({player.Name} (0x{player.Guid}), {source.Name} (0x{source.Guid}), {target.Name} (0x{target.Guid}), {result.Name}) - couldn't add {result.Name} ({result.Guid}) to player inventory after validation, this shouldn't happen!");
                result.Destroy();  // cleanup for guid manager
            }

            if (PropertyManager.GetBool("player_receive_immediate_save").Item)
                player.RushNextPlayerSave(5);

            player.SendUseDoneEvent();
        }

        /// <summary>
        /// Consumes the source weapon, and creates an intermediate tailoring kit
        /// to apply to the destination weapon
        /// </summary>
        public static void TailorWeapon(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"TailorWeapon({player.Name}, {source.Name}, {target.Name})");

            // ensure target is valid weapon
            if (!(target is MeleeWeapon) && !(target is MissileLauncher) && !(target is Caster))
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            if (target is MeleeWeapon && target.W_WeaponType == WeaponType.Undef)
            {
                // 'difficult to master' weapons were not tailorable
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            if (!HasAvailableSpace(player, source.WeenieClassId, target.WeenieClassId, DarkHeart))
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            // create intermediate weapon tailoring kit
            var wo = WorldObjectFactory.CreateNewWorldObject(DarkHeart);
            SetWeaponProperties(target, wo);

            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You tailor the appearance off the weapon.", ChatMessageType.Broadcast));

            Finalize(player, source, target, wo);
        }

        /// <summary>
        /// Reduces the coverage for a piece of armor
        /// </summary>
        public static void TailorReduceArmor(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"TailorReduceArmor({player.Name}, {source.Name}, {target.Name})");

            // Verify requirements - Can only reduce LootGen Armor
            if (target.ItemWorkmanship == null)
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            var validLocations = target.ValidLocations ?? EquipMask.None;
            var clothingPriority = CoverageMask.Unknown;

            switch (source.WeenieClassId)
            {
                case ArmorMainReductionTool:

                    if (validLocations.HasFlag(EquipMask.ChestArmor))
                    {
                        player.UpdateProperty(target, PropertyInt.ValidLocations, (int)EquipMask.ChestArmor);
                        clothingPriority = CoverageMask.OuterwearChest;
                    }
                    else if (validLocations.HasFlag(EquipMask.UpperArmArmor))
                    {
                        player.UpdateProperty(target, PropertyInt.ValidLocations, (int)EquipMask.UpperArmArmor);
                        clothingPriority = CoverageMask.OuterwearUpperArms;
                    }
                    else if (validLocations.HasFlag(EquipMask.AbdomenArmor))
                    {
                        player.UpdateProperty(target, PropertyInt.ValidLocations, (int)EquipMask.AbdomenArmor);
                        clothingPriority = CoverageMask.OuterwearAbdomen;
                    }
                    break;

                case ArmorLowerReductionTool:
                    // Can't reduce Chest Armor to anything but chest!
                    if (validLocations.HasFlag(EquipMask.ChestArmor))
                        break;

                    if (validLocations.HasFlag(EquipMask.UpperArmArmor))
                    {
                        player.UpdateProperty(target, PropertyInt.ValidLocations, (int)EquipMask.LowerArmArmor);
                        clothingPriority = CoverageMask.OuterwearLowerArms;
                    }
                    else if (validLocations.HasFlag(EquipMask.UpperLegArmor))
                    {
                        player.UpdateProperty(target, PropertyInt.ValidLocations, (int)EquipMask.LowerLegArmor);
                        clothingPriority = CoverageMask.OuterwearLowerLegs;
                    }
                    else if (validLocations.HasFlag(EquipMask.LowerLegArmor | EquipMask.FootWear))
                    {
                        player.UpdateProperty(target, PropertyInt.ValidLocations, (int)EquipMask.FootWear);
                        clothingPriority = CoverageMask.Feet;
                    }
                    break;

                case ArmorMiddleReductionTool:
                    if (validLocations.HasFlag(EquipMask.UpperLegArmor))
                    {
                        player.UpdateProperty(target, PropertyInt.ValidLocations, (int)EquipMask.UpperLegArmor);
                        clothingPriority = CoverageMask.OuterwearUpperLegs;
                    }
                    break;
            }

            if (clothingPriority == CoverageMask.Unknown)
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You modify your armor.", ChatMessageType.Broadcast));

            player.UpdateProperty(target, PropertyInt.ClothingPriority, (int)clothingPriority);
            player.TryConsumeFromInventoryWithNetworking(source, 1);

            target.SaveBiotaToDatabase();

            player.SendUseDoneEvent();
        }


        public static void ApplyMorphGem(Player player, WorldObject source, WorldObject target)
        {                        
            // Remove Melee D requirement - weenie ID = 480483
            // Alter imbue gems - ? Random change to change current imbue to alternative(AR/ CS / CB) -would need to adjust icon underlay and imbue - 480486
            // Remove Player wield requirement(similar to amethyst) - 480485


            try
            {
                //Only allow loot gen items to be morphed, except for player req and level req ones
                if ((target.ItemWorkmanship == null || target.IsAttunedOrContainsAttuned || target.ResistMagic == 9999) && source.WeenieClassId != MorphGemRemoveLevelReq && source.WeenieClassId != MorphGemRemovePlayerReq)
                {
                    player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    return;
                }

                string playerMsg = string.Empty;

                switch (source.WeenieClassId)
                {
                    #region MorphGemArmorLevel
                    //case MorphGemArmorLevel:

                    //    //Get the current AL of the item
                    //    var currentItemAL = target.GetProperty(PropertyInt.ArmorLevel);

                    //    //Disallow using AL morph gem on items w/ no AL
                    //    //if (!currentItemAL.HasValue || target.NumTimesTinkered != 0)
                    //    if (!currentItemAL.HasValue)
                    //    {
                    //        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    //        return;
                    //    }

                    //    //Get tinker log to get the num steel tinks
                    //    var tinkerLog = target.GetProperty(PropertyString.TinkerLog);
                    //    ushort numSteelTinks = 0;
                    //    if (!string.IsNullOrEmpty(tinkerLog))
                    //    {
                    //        string[] tinkerLogItems = tinkerLog.Split(',');
                    //        if (tinkerLogItems != null && tinkerLogItems.Length > 0)
                    //        {
                    //            foreach (var tink in tinkerLogItems)
                    //            {
                    //                if (tink.Equals("64"))
                    //                {
                    //                    numSteelTinks++;
                    //                }
                    //            }
                    //        }
                    //    }

                    //    //Roll for a value to change the AL by
                    //    var alRandom = new Random();
                    //    var alGain = alRandom.Next(0, 14);
                    //    var alLoss = alRandom.Next(0, 7);
                    //    var alChange = alGain - alLoss;
                    //    alChange = alChange > 10 ? 10 : alChange < -5 ? -5 : alChange;

                    //    var newAl = currentItemAL.Value + alChange;

                    //    //Don't let new Armor Level exceed maximums
                    //    var validLocations = target.ValidLocations ?? EquipMask.None;

                    //    uint maxAl = 0;

                    //    if (validLocations.HasFlag(EquipMask.HeadWear) || validLocations.HasFlag(EquipMask.HandWear) || validLocations.HasFlag(EquipMask.FootWear))
                    //    {
                    //        maxAl = MaxExtremityArmorLevel;
                    //    }
                    //    else if (validLocations.HasFlag(EquipMask.AbdomenArmor) ||
                    //             validLocations.HasFlag(EquipMask.ChestArmor) ||
                    //             validLocations.HasFlag(EquipMask.LowerArmArmor) ||
                    //             validLocations.HasFlag(EquipMask.UpperArmArmor) ||
                    //             validLocations.HasFlag(EquipMask.LowerLegArmor) ||
                    //             validLocations.HasFlag(EquipMask.UpperLegArmor)
                    //    )
                    //    {
                    //        maxAl = MaxBodyArmorLevel;
                    //    }
                    //    else if (validLocations.HasFlag(EquipMask.Shield))
                    //    {
                    //        maxAl = MaxShieldArmorLevel;
                    //    }

                    //    if (maxAl == 0)
                    //    {
                    //        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    //        return;
                    //    }

                    //    maxAl = maxAl + (numSteelTinks * 20u);

                    //    if (newAl > maxAl)
                    //    {
                    //        newAl = (int)maxAl;
                    //        alChange = newAl - currentItemAL.Value;
                    //    }

                    //    //Set the new AL value
                    //    player.UpdateProperty(target, PropertyInt.ArmorLevel, newAl);

                    //    //Send player message confirming the applied morph gem
                    //    string playerMsg = string.Empty;
                    //    if (alChange > 0)
                    //    {
                    //        playerMsg = $"As your skilled hands run softly along the contours of your armor, you quiver with anticipation.  With a swift and decisive thrust you apply the Morph Gem in a movement that is somehow both forceful and gentle at the same time.  You let out a short girly gasp that turns into a smile as you realize that your armor has been enhanced and has gained {alChange} armor level.";
                    //    }
                    //    else if (alChange == 0)
                    //    {
                    //        playerMsg = $"As your hands run softly along the contours of your armor, you quiver with anticipation.  With a timid yet determined thrust you attempt to apply the Morph Gem.  But luck being the cunt she is, the Morph Gem shatters on impact and your armor remains unchanged.";
                    //    }
                    //    else
                    //    {
                    //        playerMsg = $"As your shaking hands run softly along the contours of your armor, you quiver with anticipation.  With a timid yet determined thrust you attempt to apply the Morph Gem, but alas your hand is led astray of its mark as you are distracted by your 'room mate' calling out that your salad is ready and she bought you some new underwear with a smaller crotch for added support.  You cry softly in despair as you realize you've damaged your precious armor, which has lost {-1 * alChange} armor level as a result.";
                    //    }

                    //    player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));

                    //    break;
                    #endregion MorphGemArmorLevel

                    #region MorphGemValue
                    case MorphGemValue:

                        //Value gem - lowers value by 5 - 15%, can't lower below 20k value, 10% chance to increase value by same

                        //Get the current Value of the item
                        var currentItemValue = target.GetProperty(PropertyInt.Value);

                        if (!currentItemValue.HasValue)
                        {
                            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                            return;
                        }

                        if (currentItemValue.Value <= 20000)
                        {
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat("Morph gems do not allow an item's Value to be reduced below 20k", ChatMessageType.Broadcast));
                            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                            return;
                        }

                        //Roll for an amount to change the item Value by
                        var valRandom = new Random();
                        bool valueGain = valRandom.Next(0, 99) < 10;                        
                        var percentChange = valRandom.Next(5, 16) / 100f;
                        var valueChange = (int)Math.Round(currentItemValue.Value * percentChange * (valueGain ? 1 : -1));                        
                        var newValue = currentItemValue.Value + valueChange;

                        //Don't let new Armor Value exceed minimum of 20k
                        if (newValue < 20000)
                        {
                            valueChange = 20000 - currentItemValue.Value;
                            newValue = 20000;
                        }

                        //Set the new item value
                        player.UpdateProperty(target, PropertyInt.Value, newValue);

                        if (valueChange > 0)
                        {
                            playerMsg = $"Bad luck cunt. The Morph Gem fucked you. Your item's value has increased by {valueChange}";
                        }
                        else if (valueChange == 0)
                        {
                            playerMsg = $"The Morph Gem shatters against your item and leaves it unchanged. Could be worse.";
                        }
                        else
                        {
                            playerMsg = $"You apply the Morph Gem skillfully and have reduced the value of your item by {-1 * valueChange}";
                        }

                        //Send player message confirming the applied morph gem
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));

                        break;

                    #endregion MorphGemValue

                    #region MorphGemArmorWork
                    //case MorphGemArmorWork:

                    //    //Get the current Work of the item
                    //    var currentItemWork = target.GetProperty(PropertyInt.ItemWorkmanship);

                    //    if (!currentItemWork.HasValue)
                    //    {
                    //        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    //        return;
                    //    }

                    //    //Roll for a value to change the Workmanship by
                    //    var workRandom = new Random();
                    //    var workGain = workRandom.Next(0, 9);
                    //    var workLoss = workRandom.Next(0, 9);
                    //    var workChange = workGain - workLoss;
                    //    workChange = workChange > 1 ? 1 : workChange < -2 ? -2 : workChange;

                    //    var newWork = currentItemWork.Value + workChange;

                    //    //Don't let new Workmanship exceed maximums
                    //    if (newWork > MaxItemWork)
                    //    {
                    //        workChange = (int)MaxItemWork - currentItemWork.Value;
                    //        newWork = (int)MaxItemWork;
                    //    }
                    //    else if (newWork < MinItemWork)
                    //    {
                    //        newWork = (int)MinItemWork;
                    //        workChange = newWork - currentItemWork.Value;
                    //    }

                    //    //Set the new Workmanship value
                    //    player.UpdateProperty(target, PropertyInt.ItemWorkmanship, newWork);

                    //    if (workChange > 0)
                    //    {
                    //        playerMsg = $"Bad luck cunt.  The Morph Gem fucked you.  Your armor workmanship has increased by {workChange}";
                    //    }
                    //    else if (workChange == 0)
                    //    {
                    //        playerMsg = $"The Morph Gem shatters against your armor and leaves it unchanged.  Could be worse.";
                    //    }
                    //    else
                    //    {
                    //        playerMsg = $"You apply the Morph Gem skillfully and have reduced the workmanship of your armor by {-1 * workChange}";
                    //    }

                    //    //Send player message confirming the applied morph gem
                    //    player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));

                    //    break;
                    #endregion MorphGemArmorWork

                    #region MorphGemArcane
                    case MorphGemArcane:

                        // Lower arcane lore requirement on items - 1 - 20 - 10 % chance to increase lore by same range

                        //Get the current Arcane of the item
                        var currentItemArcane = target.GetProperty(PropertyInt.ItemDifficulty);

                        if (!currentItemArcane.HasValue)
                        {
                            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                            return;
                        }

                        //Roll for an amount to change the item Arcane by
                        var arcaneRandom = new Random();
                        var arcaneGain = arcaneRandom.Next(0, 99) < 10;
                        var arcaneChange = arcaneRandom.Next(1,20) * (arcaneGain ? 1 : -1);

                        var newArcane = currentItemArcane.Value + arcaneChange;

                        //Don't let new arcane exceed minimum of 1
                        if (newArcane < 1)
                        {
                            newArcane = 1;
                            arcaneChange = currentItemArcane.Value < 1 ? 0 : 1 - currentItemArcane.Value;
                        }

                        //Set the new arcane
                        player.UpdateProperty(target, PropertyInt.ItemDifficulty, newArcane);

                        if (arcaneChange > 0)
                        {
                            playerMsg = $"Bad luck cunt.  The Morph Gem fucked you.  Your item arcane requirement has increased by {arcaneChange}";
                        }
                        else if (arcaneChange == 0)
                        {
                            playerMsg = $"The Morph Gem shatters against your item and leaves it unchanged.  Could be worse.";
                        }
                        else
                        {
                            playerMsg = $"You apply the Morph Gem skillfully and have reduced the arcane requirement of your item by {-1 * arcaneChange}";
                        }

                        //Send player message confirming the applied morph gem
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));

                        break;
                    #endregion MorphGemArcane

                    #region MorphGemRandomEpic
                    //case MorphGemRandomEpic:

                    //    //First check if the item has any epics, and see how many
                    //    var itemEpicList = target.EpicCantrips;
                    //    if (itemEpicList == null || itemEpicList.Count < 1)
                    //    {
                    //        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    //        player.Session.Network.EnqueueSend(new GameMessageSystemChat("The target item has no epic cantrips to randomize", ChatMessageType.Broadcast));
                    //        return;
                    //    }

                    //    //Come up with a new random list of epics to apply
                    //    List<int> newEpicList = new List<int>();
                    //    foreach (var currEpic in itemEpicList)
                    //    {
                    //        while (true)
                    //        {
                    //            //For now this morph gem can only be applied to armor.
                    //            //In the future if we expand to include non-armor (like jewelry),
                    //            //will need to have logic to use different Roll methods (like JewelryCantrips.Roll())
                    //            SpellId newCantrip = ArmorCantrips.Roll();
                    //            List<SpellId> progression = SpellLevelProgression.GetSpellLevels(newCantrip);

                    //            if (progression != null && progression.Count >= 3)
                    //            {
                    //                int newEpicSpellId = (int)progression[2];
                    //                if (newEpicSpellId != currEpic.Key && !newEpicList.Contains(newEpicSpellId))
                    //                {
                    //                    newEpicList.Add(newEpicSpellId);
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }

                    //    //Give a small chance to remove an epic
                    //    if (newEpicList.Count > 1)
                    //    {
                    //        var epicRandom = new Random();
                    //        var roll = epicRandom.Next(0, int.MaxValue);
                    //        if (roll % 15 == 0 && newEpicList.Count > 0)
                    //        {
                    //            newEpicList.RemoveAt(0);
                    //        }
                    //    }

                    //    //Give a small chance to add an epic
                    //    if (newEpicList.Count < 4)
                    //    {
                    //        var epicRandom = new Random();
                    //        var roll = epicRandom.Next(0, int.MaxValue);
                    //        if (roll % 10 == 0 && newEpicList.Count > 0)
                    //        {
                    //            while (true)
                    //            {
                    //                SpellId newCantrip = ArmorCantrips.Roll();
                    //                List<SpellId> progression = SpellLevelProgression.GetSpellLevels(newCantrip);
                    //                if (progression != null && progression.Count >= 3)
                    //                {
                    //                    int newEpicSpellId = (int)progression[2];
                    //                    if (!newEpicList.Contains(newEpicSpellId))
                    //                    {
                    //                        newEpicList.Add((int)progression[2]);
                    //                        break;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }

                    //    //Give a small chance to add Epic Impen
                    //    var impenRandom = new Random();
                    //    bool impenSuccess = false;
                    //    var impenRoll = impenRandom.Next(0, int.MaxValue);
                    //    if (impenRoll % 7 == 0 && !newEpicList.Contains(4667))
                    //    {
                    //        if (newEpicList.Count < 4)
                    //        {
                    //            newEpicList.Add(4667);
                    //        }
                    //        else
                    //        {
                    //            newEpicList[0] = 4667;
                    //        }

                    //        impenSuccess = true;
                    //    }

                    //    //Remove all existing epics
                    //    string removedSpellList = "";
                    //    int removedEpicNum = 0;
                    //    foreach (var spell in itemEpicList)
                    //    {
                    //        target.Biota.TryRemoveKnownSpell(spell.Key, target.BiotaDatabaseLock);
                    //        //player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Removed spellId { spell.Key }", ChatMessageType.Broadcast));
                    //        removedEpicNum++;
                    //        if (removedEpicNum == 1)
                    //        {
                    //            removedSpellList = $"{new Spell(spell.Key, true).Name}";
                    //        }
                    //        else if (removedEpicNum == itemEpicList.Count)
                    //        {
                    //            removedSpellList += $" and {new Spell(spell.Key, true).Name}";
                    //        }
                    //        else
                    //        {
                    //            removedSpellList += $", {new Spell(spell.Key, true).Name}";
                    //        }
                    //        //removedSpellList += removedEpicNum < itemEpicList.Count ? $"{ new Spell(spell.Key, true).Name }, " : $"and { new Spell(spell.Key, true).Name }";
                    //    }

                    //    //Add new epics
                    //    string addedSpellList = "";
                    //    int addedEpicNum = 0;
                    //    foreach (var spellId in newEpicList)
                    //    {
                    //        target.Biota.GetOrAddKnownSpell(spellId, target.BiotaDatabaseLock, out _);
                    //        //player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Added spellId { spellId }", ChatMessageType.Broadcast));
                    //        addedEpicNum++;
                    //        if (addedEpicNum == 1)
                    //        {
                    //            addedSpellList = $"{new Spell(spellId, true).Name}";
                    //        }
                    //        else if (addedEpicNum == newEpicList.Count)
                    //        {
                    //            addedSpellList += $" and {new Spell(spellId, true).Name}";
                    //        }
                    //        else
                    //        {
                    //            addedSpellList += $", {new Spell(spellId, true).Name}";
                    //        }
                    //        //addedSpellList += addedEpicNum < itemEpicList.Count ? $"{ new Spell(spellId, true).Name }" : $"and { new Spell(spellId, true).Name }";
                    //    }

                    //    string impenMessage = impenSuccess ? "\n\nYour armor also somehow looks tougher, like it might have once been worn by some kind of tough guy and his tough guy essence sort of rubbed off on it and now it's more tough than it was before." : "";

                    //    string randomizeResultMsg = $"Staring into the morph gem intently, your head swims at the chaos within it.  As you slump to the ground you scream in silence at the realization that eternity is boundless and upon you; upon us all.  You smash the morph gem hard against your armor and it explodes into everything and nothing.  Washed away are the epic enchantments that once took hold.\n\nThe spells {removedSpellList} are no longer.\n\nIn their place, the spells {addedSpellList} have been cast upon your armor.{impenMessage}";
                    //    player.Session.Network.EnqueueSend(new GameMessageSystemChat(randomizeResultMsg, ChatMessageType.Broadcast));

                    //    break;
                    #endregion MorphGemRandomEpic

                    #region MorphGemRandomSet
                    //case MorphGemRandomSet:

                    //    if (target.ClothingPriority == null || (target.ClothingPriority & (CoverageMask)CoverageMaskHelper.Outerwear) == 0)
                    //    {
                    //        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    //        player.Session.Network.EnqueueSend(new GameMessageSystemChat("The target item does not meet the requirements for adding an Equipment Set", ChatMessageType.Broadcast));
                    //        return;
                    //    }

                    //    var originalSetId = target.EquipmentSetId;
                    //    bool setRollResult = false;

                    //    if (target.EquipmentSetId.HasValue)
                    //    {
                    //        //If item has an existing set, roll a 10% chance to remove the Set
                    //        int removeSetRoll = ThreadSafeRandom.Next(0, 9);
                    //        if (removeSetRoll > 0)
                    //        {
                    //            setRollResult = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //If item has no set, roll a 15% chance to add a Set
                    //        int addSetRoll = ThreadSafeRandom.Next(0, 99);
                    //        if (addSetRoll < 15)
                    //        {
                    //            setRollResult = true;
                    //        }
                    //    }

                    //    if (setRollResult)
                    //    {
                    //        target.EquipmentSetId = (EquipmentSet)ThreadSafeRandom.Next((int)EquipmentSet.Soldiers, (int)EquipmentSet.Lightningproof);
                    //        if (originalSetId.HasValue && target.EquipmentSetId.Value == originalSetId.Value)
                    //        {
                    //            int counter = 0;
                    //            while (target.EquipmentSetId.Value == originalSetId.Value && counter < 10)
                    //            {
                    //                target.EquipmentSetId = (EquipmentSet)ThreadSafeRandom.Next((int)EquipmentSet.Soldiers, (int)EquipmentSet.Lightningproof);
                    //                counter++;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        target.EquipmentSetId = null;
                    //    }

                    //    string resultMsg = string.Empty;

                    //    if (originalSetId.HasValue && setRollResult)
                    //    {
                    //        //Randomized existing set
                    //        resultMsg = "You and I, we aren't so different.  We are nothing alike yet we are... nothing.  None of it matters.  You, especially, don't matter.  You will die.  I will die.  Will we then be the same?  Will we be at all?  Will you shut the fuck up?  Your armor has a different Set on it now, congrats.";
                    //    }
                    //    else if (!originalSetId.HasValue && setRollResult)
                    //    {
                    //        //Added new set
                    //        resultMsg = "If you get caught with drugs and they ask you who you got them from, it was the naked Indian.  Also, your item that didn't have a Set, well, now it has a Set.  Take a look and see.  Hopefully it's what you wanted.";
                    //    }
                    //    else if (originalSetId.HasValue && !setRollResult)
                    //    {
                    //        //Remove existing set
                    //        resultMsg = "Bad luck cunt, your armor that had a Set on it, well now it doesn't have a Set on it.  Also, you're ugly and smell bad, and those are your best qualities.";
                    //    }
                    //    else
                    //    {
                    //        //No existing set, failed to add a set
                    //        resultMsg = "I once had a dream that I was peeing on a tree in the woods.  When I woke up, I had pissed all over myself.  I hope that makes you feel slightly better about the fact that your item which didn't previously have a Set on it still doesn't have a Set on it.  Better luck next time cunt.";
                    //    }

                    //    player.Session.Network.EnqueueSend(new GameMessageSystemChat(resultMsg, ChatMessageType.Broadcast));

                    //    break;
                    #endregion MorphGemRandomSet

                    #region MorphGemRemoveMissileDReq
                    case MorphGemRemoveMissileDReq:
                        // Remove Missile D requirement - weenie ID = 480484

                        //Validate that the item has a Missile D activation requirement
                        if (target.ItemSkillLimit != Skill.MissileDefense || target.ItemSkillLevelLimit == null)
                        {
                            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                            return;
                        }

                        //Remove the activation requirement
                        target.ItemSkillLimit = null;
                        target.ItemSkillLevelLimit = null;

                        playerMsg = $"You apply the Morph Gem skillfully and have removed the Missile Defense activation requirement of your item.";

                        //Send player message confirming the applied morph gem
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));
                        break;

                    #endregion MorphGemRemoveMissileDReq
     
                    #region MorphGemRemoveMeleeDReq
                    case MorphGemRemoveMeleeDReq:
                        //Validate that the item has a Melee D activation requirement
                        if (target.ItemSkillLimit != Skill.MeleeDefense || target.ItemSkillLevelLimit == null)
                        {
                            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                            return;
                        }

                        //Remove the activation requirement
                        target.ItemSkillLimit = null;
                        target.ItemSkillLevelLimit = null;

                        playerMsg = $"You apply the Morph Gem skillfully and have removed the Melee Defense activation requirement of your item.";

                        //Send player message confirming the applied morph gem
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));
                        break;

                    #endregion MorphGemRemoveMeleeDReq

                    #region MorphGemRandomizeWeaponImbue
                    case MorphGemRandomizeWeaponImbue:

                        //Verify the item is imbued with AR, CS or CB
                        var isValid = false;                        

                        if(target.HasImbuedEffect(ImbuedEffectType.CripplingBlow) ||
                            target.HasImbuedEffect(ImbuedEffectType.ArmorRending) ||
                            target.HasImbuedEffect(ImbuedEffectType.CriticalStrike))
                        {
                            isValid = true;
                        }

                        if (!isValid)
                        {
                            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                            return;
                        }

                        var origImbueEffect = target.ImbuedEffect;

                        var wepImbueRandom = new Random();
                        var roll = wepImbueRandom.Next(0, 1);
                        if(target.HasImbuedEffect(ImbuedEffectType.CripplingBlow))
                        {
                            target.ImbuedEffect = roll == 0 ? ImbuedEffectType.ArmorRending : ImbuedEffectType.CriticalStrike;
                        }
                        else if (target.HasImbuedEffect(ImbuedEffectType.ArmorRending))
                        {
                            target.ImbuedEffect = roll == 0 ? ImbuedEffectType.CripplingBlow : ImbuedEffectType.CriticalStrike;
                        }
                        else if (target.HasImbuedEffect(ImbuedEffectType.CriticalStrike))
                        {
                            target.ImbuedEffect = roll == 0 ? ImbuedEffectType.ArmorRending : ImbuedEffectType.CripplingBlow;
                        }

                        target.IconUnderlayId = RecipeManager.IconUnderlay[target.ImbuedEffect];

                        playerMsg = $"You apply the Morph Gem skillfully and have changed your weapon's imbue from {origImbueEffect} to {target.ImbuedEffect}";

                        //Send player message confirming the applied morph gem
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));

                        break;
                    #endregion MorphGemRandomizeWeaponImbue

                    #region MorphGemRemovePlayerReq
                    case MorphGemRemovePlayerReq:

                        if (!target.GetProperty(PropertyInstanceId.AllowedWielder).HasValue)
                        {
                            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                            return;
                        }

                        var origWielder = target.GetProperty(PropertyString.CraftsmanName);

                        target.RemoveProperty(PropertyInstanceId.AllowedWielder);
                        target.RemoveProperty(PropertyString.CraftsmanName);

                        playerMsg = $"You apply the Morph Gem skillfully and have altered your item so it is no longer wield restricted to {origWielder}";

                        //Send player message confirming the applied morph gem
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));

                        break;
                    #endregion MorphGemRemovePlayerReq

                    #region MorphGemSlayerRandom
                    case MorphGemSlayerRandom:

                        var tinkerLottoLog = target.GetProperty(PropertyString.TinkerLottoLog);
                        if (!String.IsNullOrEmpty(tinkerLottoLog) && tinkerLottoLog.Contains("Slayer") && target.SlayerCreatureType != null)
                        {
                            var selectSlayerType = ThreadSafeRandom.Next(1, 25);
                            switch (selectSlayerType)
                            {
                                case 1:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Banderling;
                                    break;

                                case 2:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Drudge;
                                    break;

                                case 3:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Gromnie;
                                    break;

                                case 4:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Lugian;
                                    break;

                                case 5:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Grievver;
                                    break;

                                case 6:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mattekar;
                                    break;

                                case 7:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mite;
                                    break;

                                case 8:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mosswart;
                                    break;

                                case 9:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Mumiyah;
                                    break;

                                case 10:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Olthoi;
                                    break;

                                case 11:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.PhyntosWasp;
                                    break;

                                case 12:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shadow;
                                    break;

                                case 13:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Shreth;
                                    break;

                                case 14:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Skeleton;
                                    break;

                                case 15:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tumerok;
                                    break;

                                case 16:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Tusker;
                                    break;

                                case 17:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Virindi;
                                    break;

                                case 18:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Wisp;
                                    break;

                                case 19:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Zefir;
                                    break;

                                case 20:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Golem;
                                    break;

                                case 21:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Gurog;
                                    break;

                                case 22:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Burun;
                                    break;

                                case 23:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Remoran;
                                    break;

                                case 24:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Reedshark;
                                    break;

                                case 25:
                                    target.SlayerCreatureType = ACE.Entity.Enum.CreatureType.Eater;
                                    break;
                            }

                            playerMsg = $"The Morph Gem alters your weapon's slayer type to {target.SlayerCreatureType}";
                            

                            //Send player message confirming the applied morph gem
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));
                        }
                        else
                        {
                            //Must be a slayer that was applied by tinker lotto
                            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                            return;
                        }
                        break;

                    #endregion MorphGemSlayerRandom


                    #region MorphGemRemoveLevelReq

                    case MorphGemRemoveLevelReq:

                        if (!target.GetProperty(PropertyInt.WieldDifficulty).HasValue)
                        {
                            player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                            return;
                        }

                        var origLevelReq = target.GetProperty(PropertyInt.WieldDifficulty);

                        target.RemoveProperty(PropertyInt.WieldDifficulty);

                        playerMsg = $"You apply the Morph Gem skillfully and have altered your item so it no longer requires level {origLevelReq} to wield";

                        //Send player message confirming the applied morph gem
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(playerMsg, ChatMessageType.Broadcast));

                        break;

                    #endregion MorphGemRemoveLevelReq
                    default:
                        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                        return;
                }

                player.TryConsumeFromInventoryWithNetworking(source, 1);

                target.SaveBiotaToDatabase();

                player.SendUseDoneEvent();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Exception in Tailoring.ApplyMorphGem. Ex: {0}", ex);
            }
        }

        /// <summary>
        /// Adjusts the layering priority for a piece of armor
        /// </summary>
        public static void TailorLayerArmor(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"TailorLayerArmor({player.Name}, {source.Name}, {target.Name})");

            var topLayer = source.WeenieClassId == ArmorLayeringToolTop;
            player.UpdateProperty(target, PropertyBool.TopLayerPriority, topLayer);

            player.TryConsumeFromInventoryWithNetworking(source, 1);

            target.SaveBiotaToDatabase();

            player.SendUseDoneEvent();
        }

        /// <summary>
        /// Applies an intermediate tailoring kit to a destination piece of armor
        /// </summary>
        public static void ArmorApply(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"ArmorApply({player.Name}, {source.Name}, {target.Name})");

            // verify armor type
            if (source.ClothingPriority != target.ClothingPriority)
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You tailor the appearance onto a different piece of armor.", ChatMessageType.Broadcast));

            // update properties
            UpdateArmorProps(player, source, target);

            // Send UpdateObject, mostly for the client to register the new name.
            player.Session.Network.EnqueueSend(new GameMessageUpdateObject(target));

            player.TryConsumeFromInventoryWithNetworking(source, 1);

            target.SaveBiotaToDatabase();

            player.SendUseDoneEvent();
        }

        /// <summary>
        /// Applies an intermediate tailoring kit to a destination weapon
        /// </summary>
        public static void WeaponApply(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"WeaponApply({player.Name}, {source.Name}, {target.Name})");

            // verify weapon type
            switch (source.TargetType)
            {
                case ItemType.MeleeWeapon:

                    if (source.W_WeaponType != target.W_WeaponType ||
                        source.W_DamageType != target.W_DamageType)
                    {
                        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                        return;
                    }
                    break;

                case ItemType.MissileWeapon:

                    if (source.DefaultCombatStyle != target.DefaultCombatStyle ||
                        source.W_DamageType != DamageType.Undef && source.W_DamageType != target.W_DamageType)
                    {
                        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                        return;
                    }
                    break;

                case ItemType.Caster:

                    if (source.W_DamageType != DamageType.Undef && source.W_DamageType != target.W_DamageType)
                    {
                        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                        return;
                    }
                    break;

                default:
                    player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    return;
            }

            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You tailor the appearance onto a different weapon.", ChatMessageType.Broadcast));

            // Update all of the relevant properties
            UpdateWeaponProps(player, source, target);

            // Send UpdateObject, mostly for the client to register the new name.
            player.Session.Network.EnqueueSend(new GameMessageUpdateObject(target));

            player.TryConsumeFromInventoryWithNetworking(source, 1);

            target.SaveBiotaToDatabase();

            player.SendUseDoneEvent();
        }

        public static void UpdateCommonProps(Player player, WorldObject source, WorldObject target)
        {
            player.UpdateProperty(target, PropertyInt.PaletteTemplate, source.PaletteTemplate);
            //player.UpdateProperty(target, PropertyInt.UiEffects, (int?)source.UiEffects);
            if (source.MaterialType.HasValue)
                player.UpdateProperty(target, PropertyInt.MaterialType, (int?)source.MaterialType);

            player.UpdateProperty(target, PropertyFloat.DefaultScale, source.ObjScale);

            player.UpdateProperty(target, PropertyFloat.Shade, source.Shade);
            player.UpdateProperty(target, PropertyFloat.Shade2, source.Shade2);
            player.UpdateProperty(target, PropertyFloat.Shade3, source.Shade3);
            player.UpdateProperty(target, PropertyFloat.Shade4, source.Shade4);

            player.UpdateProperty(target, PropertyBool.LightsStatus, source.LightsStatus);
            player.UpdateProperty(target, PropertyFloat.Translucency, source.Translucency);

            player.UpdateProperty(target, PropertyDataId.Setup, source.SetupTableId);
            player.UpdateProperty(target, PropertyDataId.ClothingBase, source.ClothingBase);
            player.UpdateProperty(target, PropertyDataId.PaletteBase, source.PaletteBaseId);

            player.UpdateProperty(target, PropertyString.Name, source.Name);
            player.UpdateProperty(target, PropertyString.LongDesc, source.LongDesc);

            player.UpdateProperty(target, PropertyBool.IgnoreCloIcons, source.IgnoreCloIcons);
            player.UpdateProperty(target, PropertyDataId.Icon, source.IconId);
        }

        public static void UpdateArmorProps(Player player, WorldObject source, WorldObject target)
        {
            UpdateCommonProps(player, source, target);

            player.UpdateProperty(target, PropertyBool.Dyable, source.Dyable);

            // If the item we are replacing is one of our preset icons with an overlay, we need to remove it.
            if (ArmorOverlayIcons.ContainsKey(target.WeenieClassId))
                player.UpdateProperty(target, PropertyDataId.IconOverlay, null);

            // If the source item has an icon stashed in the Secondary Overlay, it's because we put it there!
            // Apply this overlay if the target does not already have one.
            if (source.GetProperty(PropertyDataId.IconOverlaySecondary).HasValue && !target.IconOverlayId.HasValue)
                player.UpdateProperty(target, PropertyDataId.IconOverlay, source.GetProperty(PropertyDataId.IconOverlaySecondary));

            // ObjDescOverride.Clear()
        }

        public static void UpdateWeaponProps(Player player, WorldObject source, WorldObject target)
        {
            UpdateCommonProps(player, source, target);

            player.UpdateProperty(target, PropertyInt.HookType, source.HookType);
            player.UpdateProperty(target, PropertyInt.HookPlacement, source.HookPlacement);
        }

        public static uint? GetArmorWCID(EquipMask validLocations)
        {
            switch (validLocations)
            {
                case EquipMask.HeadWear:
                    return Heaume;
                case EquipMask.HandWear:
                    return PlatemailGauntlets;
                case EquipMask.FootWear:
                case EquipMask.FootWear | EquipMask.LowerLegWear:
                    return LeatherBoots;
                case EquipMask.ChestArmor:
                    return LeatherVest;
                case EquipMask.AbdomenArmor:
                    return YoroiGirth;
                case EquipMask.UpperArmArmor:
                    return YoroiPauldrons;
                case EquipMask.LowerArmArmor:
                    return CeldonSleeves;
                case EquipMask.UpperLegArmor:
                    return YoroiGreaves;
                case EquipMask.LowerLegArmor:
                    return YoroiLeggings;
            }

            if (validLocations.HasFlag(EquipMask.ChestArmor) || validLocations.HasFlag(EquipMask.UpperArmArmor))
                return WingedCoat;
            if (validLocations.HasFlag(EquipMask.AbdomenArmor) || validLocations.HasFlag(EquipMask.UpperLegArmor))
                return AmuliLeggings;

            if (validLocations.HasFlag(EquipMask.Armor) || validLocations == EquipMask.Cloak || validLocations == EquipMask.Shield ||
                validLocations.HasFlag(EquipMask.ChestWear) || validLocations.HasFlag(EquipMask.AbdomenWear))
                return Tentacles;

            return null;
        }

        // intermediates
        public const uint LeatherVest = 42403;
        public const uint WingedCoat = 42405;
        public const uint PlatemailGauntlets = 42407;
        public const uint YoroiGirth = 42409;
        public const uint YoroiGreaves = 42411;
        public const uint Heaume = 42414;
        public const uint YoroiLeggings = 42416;
        public const uint AmuliLeggings = 42417;
        public const uint YoroiPauldrons = 42418;
        public const uint CeldonSleeves = 42421;
        public const uint LeatherBoots = 42422;
        public const uint Tentacles = 44863;
        public const uint DarkHeart = 51451;

        /// <summary>
        /// Returns TRUE if the input wcid is a tailoring kit
        /// </summary>
        public static bool IsTailoringKit(uint wcid)
        {
            // ...
            switch (wcid)
            {
                case ArmorTailoringKit:
                case WeaponTailoringKit:
                case ArmorMainReductionTool:
                case ArmorLowerReductionTool:
                case ArmorMiddleReductionTool:
                case ArmorLayeringToolTop:
                case ArmorLayeringToolBottom:
                case Heaume:
                case PlatemailGauntlets:
                case LeatherBoots:
                case LeatherVest:
                case YoroiGirth:
                case YoroiPauldrons:
                case CeldonSleeves:
                case YoroiGreaves:
                case YoroiLeggings:
                case AmuliLeggings:
                case WingedCoat:
                case Tentacles:
                case DarkHeart:
                case MorphGemValue:                
                case MorphGemArcane:
                case MorphGemRemoveMissileDReq:
                case MorphGemRemoveMeleeDReq:
                case MorphGemRandomizeWeaponImbue:
                case MorphGemRemovePlayerReq:
                case MorphGemSlayerRandom:
                case MorphGemRemoveLevelReq:

                    return true;

                default:

                    return false;
            }
        }
    }
}
