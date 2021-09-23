using System;
using System.Collections.Generic;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class Tailoring
    {
        // http://acpedia.org/wiki/Tailoring
        // https://asheron.fandom.com/wiki/Tailoring

        // tailoring kits
        public const uint ArmorTailoringKit = 41956;
        public const uint WeaponTailoringKit = 51445;

        public const uint ArmorMainReductionTool = 42622;
        public const uint ArmorLowerReductionTool = 44879;
        public const uint ArmorMiddleReductionTool = 44880;

        public const uint ArmorLayeringToolTop = 42724;
        public const uint ArmorLayeringToolBottom = 42726;

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
            target.LightsStatus = source.LightsStatus;
            target.Translucency = source.Translucency;

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

        public static void Finalize(Player player, WorldObject source, WorldObject target, WorldObject result)
        {
            player.TryConsumeFromInventoryWithNetworking(source, 1);
            player.TryConsumeFromInventoryWithNetworking(target, 1);

            player.TryCreateInInventoryWithNetworking(result);

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

            // create intermediate weapon tailoring kit
            var wo = WorldObjectFactory.CreateNewWorldObject(51451);
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
            player.UpdateProperty(target, PropertyBool.LightsStatus, source.LightsStatus);
            player.UpdateProperty(target, PropertyFloat.Translucency, source.Translucency);
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

                    return true;

                default:

                    return false;
            }
        }
    }
}
