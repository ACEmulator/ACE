using System;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
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

            var actionChain = new ActionChain();

            // handle switching to peace mode
            if (player.CombatMode != CombatMode.NonCombat)
            {
                var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                actionChain.AddDelaySeconds(stanceTime);
            }

            // perform clapping motion
            player.EnqueueMotion(actionChain, MotionCommand.ClapHands);

            actionChain.AddAction(player, () => DoTailoring(player, source, target));

            actionChain.EnqueueChain();
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

            SetCommonProperties(target, wo);

            wo.ClothingPriority = target.ClothingPriority;

            // ObjDescOverride.Clear()

            Finalize(player, source, target, wo);
        }

        public static void SetCommonProperties(WorldObject source, WorldObject target)
        {
            // a lot of this was probably done with recipes and mutations in the original
            // here a lot is done directly in code..

            target.Name = source.Name;

            target.PaletteTemplate = source.PaletteTemplate;
            target.EncumbranceVal = source.EncumbranceVal;
            target.UiEffects = source.UiEffects;
            target.Value = source.Value;
            target.MaterialType = source.MaterialType;
            target.TargetType = source.ItemType;

            target.Shade = source.Shade;
            target.Shade2 = source.Shade2;
            target.Shade3 = source.Shade3;
            target.Shade4 = source.Shade4;

            target.SetupTableId = source.SetupTableId;
            target.PaletteBaseId = source.PaletteBaseId;
            target.ClothingBase = source.ClothingBase;
            target.IconId = source.IconId;

            // probably missing some fields...
        }

        public static void Finalize(Player player, WorldObject source, WorldObject target, WorldObject result)
        {
            player.TryConsumeFromInventoryWithNetworking(source, 1);
            player.TryConsumeFromInventoryWithNetworking(target, 1);

            player.TryCreateInInventoryWithNetworking(result);

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

            // create intermediate weapon tailoring kit
            var wo = WorldObjectFactory.CreateNewWorldObject(51451);
            SetCommonProperties(target, wo);

            if (target is MeleeWeapon)
            {
                wo.W_WeaponType = target.W_WeaponType;
            }
            else if (target is MissileLauncher)
            {
                wo.DefaultCombatStyle = target.DefaultCombatStyle;
            }

            wo.W_DamageType = target.W_DamageType;
            wo.ObjScale = target.ObjScale;

            Finalize(player, source, target, wo);
        }

        /// <summary>
        /// Reduces the coverage for a piece of armor
        /// </summary>
        public static void TailorReduceArmor(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"TailorReduceArmor({player.Name}, {source.Name}, {target.Name})");

            // verify requirements
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

            player.UpdateProperty(target, PropertyInt.ClothingPriority, (int)clothingPriority);
            player.TryConsumeFromInventoryWithNetworking(source, 1);

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

            // update properties
            // creating a brand new item might be reasonable here,
            // but we have to make sure the list of properties here is completed
            UpdateCommonProps(player, source, target);

            // ObjDescOverride.Clear()

            player.TryConsumeFromInventoryWithNetworking(source, 1);

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

                    if (source.W_WeaponType != target.W_WeaponType || source.W_DamageType != target.W_DamageType)
                    {
                        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                        return;
                    }
                    break;

                case ItemType.MissileWeapon:

                    if (source.DefaultCombatStyle != target.DefaultCombatStyle || source.W_DamageType != 0 && source.W_DamageType != target.W_DamageType)
                    {
                        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                        return;
                    }
                    break;

                case ItemType.Caster:

                    if (source.W_DamageType != 0 && source.W_DamageType != target.W_DamageType)
                    {
                        player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                        return;
                    }
                    break;

                default:
                    player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    return;
            }

            // update properties
            // creating a brand new item might be reasonable here,
            // but we have to make sure the list of properties here is completed
            UpdateCommonProps(player, source, target);

            target.ObjScale = source.ObjScale;

            player.TryConsumeFromInventoryWithNetworking(source, 1);

            player.SendUseDoneEvent();
        }

        public static void UpdateCommonProps(Player player, WorldObject source, WorldObject target)
        {
            player.UpdateProperty(target, PropertyString.Name, source.Name);

            player.UpdateProperty(target, PropertyInt.PaletteTemplate, source.PaletteTemplate);
            player.UpdateProperty(target, PropertyInt.UiEffects, (int?)source.UiEffects);
            player.UpdateProperty(target, PropertyInt.MaterialType, (int?)source.MaterialType);

            player.UpdateProperty(target, PropertyFloat.Shade, source.Shade);
            player.UpdateProperty(target, PropertyFloat.Shade2, source.Shade2);
            player.UpdateProperty(target, PropertyFloat.Shade3, source.Shade3);
            player.UpdateProperty(target, PropertyFloat.Shade4, source.Shade4);

            player.UpdateProperty(target, PropertyDataId.Setup, source.SetupTableId);
            player.UpdateProperty(target, PropertyDataId.PaletteBase, source.PaletteBaseId);
            player.UpdateProperty(target, PropertyDataId.ClothingBase, source.ClothingBase);
            player.UpdateProperty(target, PropertyDataId.Icon, source.IconId);
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
