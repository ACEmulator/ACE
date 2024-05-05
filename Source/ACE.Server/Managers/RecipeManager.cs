using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using log4net;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Entity.Mutations;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ACE.Server.Managers
{
    public partial class RecipeManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Recipe GetRecipe(Player player, WorldObject source, WorldObject target)
        {
            // PY16 recipes
            var cookbook = DatabaseManager.World.GetCachedCookbook(source.WeenieClassId, target.WeenieClassId);
            if (cookbook != null)
                return cookbook.Recipe;

            // if none exists, try finding new recipe
            return GetNewRecipe(player, source, target);
        }

        public static void UseObjectOnTarget(Player player, WorldObject source, WorldObject target, bool confirmed = false)
        {
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

            if (source == target)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {source.NameWithMaterial} cannot be combined with itself.", ChatMessageType.Craft));
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"You can't use the {source.NameWithMaterial} on itself."));
                player.SendUseDoneEvent();
                return;
            }

            var recipe = GetRecipe(player, source, target);

            if (recipe == null)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {source.NameWithMaterial} cannot be used on the {target.NameWithMaterial}.", ChatMessageType.Craft));
                player.SendUseDoneEvent();
                return;
            }

            // verify requirements
            if (!VerifyRequirements(recipe, player, source, target))
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            if (recipe.IsTinkering())
                log.DebugFormat("[TINKERING] {0}.UseObjectOnTarget({1}, {2}) | Status: {3}confirmed", player.Name, source.NameWithMaterial, target.NameWithMaterial, (confirmed ? "" : "un"));

            var percentSuccess = GetRecipeChance(player, source, target, recipe);

            if (percentSuccess == null)
            {
                player.SendUseDoneEvent();
                return;
            }

            var showDialog = HasDifficulty(recipe) && player.GetCharacterOption(CharacterOption.UseCraftingChanceOfSuccessDialog);

            if (!confirmed && player.LumAugSkilledCraft > 0)
                player.SendMessage($"Your Aura of the Craftman augmentation increased your skill by {player.LumAugSkilledCraft}!");

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
            var clapTime = !confirmed ? Physics.Animation.MotionTable.GetAnimationLength(player.MotionTableId, currentStance, motionCommand) : 0.0f;

            if (!confirmed)
            {
                actionChain.AddAction(player, () => player.SendMotionAsCommands(motionCommand, currentStance));
                actionChain.AddDelaySeconds(clapTime);

                nextUseTime += clapTime;
            }

            if (showDialog && !confirmed)
            {
                actionChain.AddAction(player, () => ShowDialog(player, source, target, recipe, percentSuccess.Value));
                actionChain.AddAction(player, () => player.IsBusy = false);
            }
            else
            {
                actionChain.AddAction(player, () => HandleRecipe(player, source, target, recipe, percentSuccess.Value));

                actionChain.AddAction(player, () =>
                {
                    if (!showDialog)
                        player.SendUseDoneEvent();

                    player.IsBusy = false;
                });
            }

            actionChain.EnqueueChain();

            player.NextUseTime = DateTime.UtcNow.AddSeconds(nextUseTime);
        }

        public static bool HasDifficulty(Recipe recipe)
        {
            if (recipe.IsTinkering())
                return true;

            return recipe.Skill > 0 && recipe.Difficulty > 0;
        }

        public static double? GetRecipeChance(Player player, WorldObject source, WorldObject target, Recipe recipe)
        {
            if (recipe.IsTinkering())
                return GetTinkerChance(player, source, target, recipe);

            if (!HasDifficulty(recipe))
                return 1.0;

            var playerSkill = player.GetCreatureSkill((Skill)recipe.Skill);

            if (playerSkill == null)
            {
                // this shouldn't happen, but sanity check for unexpected nulls
                log.Warn($"RecipeManager.GetRecipeChance({player.Name}, {source.Name}, {target.Name}): recipe {recipe.Id} missing skill");
                return null;
            }

            // check for pre-MoA skill
            // convert into appropriate post-MoA skill
            // pre-MoA melee weapons: get highest melee weapons skill
            var newSkill = player.ConvertToMoASkill(playerSkill.Skill);

            playerSkill = player.GetCreatureSkill(newSkill);

            //Console.WriteLine("Required skill: " + skill.Skill);

            if (playerSkill.AdvancementClass < SkillAdvancementClass.Trained)
            {
                player.SendWeenieError(WeenieError.YouAreNotTrainedInThatTradeSkill);
                return null;
            }

            //Console.WriteLine("Skill difficulty: " + recipe.Recipe.Difficulty);

            var playerCurrentPlusLumAugSkilledCraft = playerSkill.Current + (uint)player.LumAugSkilledCraft;

            var successChance = SkillCheck.GetSkillChance(playerCurrentPlusLumAugSkilledCraft, recipe.Difficulty);

            return successChance;
        }

        public static double? GetTinkerChance(Player player, WorldObject tool, WorldObject target, Recipe recipe)
        {
            // calculate % success chance

            var toolWorkmanship = tool.Workmanship ?? 0;
            var itemWorkmanship = target.Workmanship ?? 0;

            var tinkeredCount = target.NumTimesTinkered;

            var materialType = tool.MaterialType ?? MaterialType.Unknown;
            var salvageMod = GetMaterialMod(materialType);

            var workmanshipMod = 1.0f;
            if (toolWorkmanship >= itemWorkmanship)
                workmanshipMod = 2.0f;

            var recipeSkill = (Skill)recipe.Skill;

            var skill = player.GetCreatureSkill(recipeSkill);

            // tinkering skill must be trained
            if (skill.AdvancementClass < SkillAdvancementClass.Trained)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You are not trained in {skill.Skill.ToSentence()}.", ChatMessageType.Broadcast));
                return null;
            }

            // thanks to Endy's Tinkering Calculator for this formula!
            var attemptMod = TinkeringDifficulty[tinkeredCount];

            var difficulty = (int)Math.Floor(((salvageMod * 5.0f) + (itemWorkmanship * salvageMod * 2.0f) - (toolWorkmanship * workmanshipMod * salvageMod / 5.0f)) * attemptMod);

            var playerCurrentPlusLumAugSkilledCraft = skill.Current + (uint)player.LumAugSkilledCraft;

            var successChance = SkillCheck.GetSkillChance((int)playerCurrentPlusLumAugSkilledCraft, difficulty);

            // imbue: divide success by 3
            if (recipe.IsImbuing())
            {
                successChance /= 3.0f;

                if (player.AugmentationBonusImbueChance > 0)
                    successChance += player.AugmentationBonusImbueChance * 0.05f;
            }

            // todo: remove this once foolproof salvage recipes are updated
            if (foolproofTinkers.Contains((WeenieClassName)tool.WeenieClassId))
                successChance = 1.0;

            return successChance;
        }

        /// <summary>
        /// Returns the modifier for a bag of salvaging material
        /// </summary>
        public static float GetMaterialMod(MaterialType material)
        {
            switch (material)
            {
                case MaterialType.Gold:
                case MaterialType.Oak:
                    return 10.0f;

                case MaterialType.Alabaster:
                case MaterialType.ArmoredilloHide:
                case MaterialType.Brass:
                case MaterialType.Bronze:
                case MaterialType.Ceramic:
                case MaterialType.Granite:
                case MaterialType.Linen:
                case MaterialType.Marble:
                case MaterialType.Moonstone:
                case MaterialType.Opal:
                case MaterialType.Pine:
                case MaterialType.ReedSharkHide:
                case MaterialType.Velvet:
                case MaterialType.Wool:
                    return 11.0f;

                case MaterialType.Ebony:
                case MaterialType.GreenGarnet:
                case MaterialType.Iron:
                case MaterialType.Mahogany:
                case MaterialType.Porcelain:
                case MaterialType.Satin:
                case MaterialType.Steel:
                case MaterialType.Teak:
                    return 12.0f;

                case MaterialType.Bloodstone:
                case MaterialType.Carnelian:
                case MaterialType.Citrine:
                case MaterialType.Hematite:
                case MaterialType.LavenderJade:
                case MaterialType.Malachite:
                case MaterialType.RedJade:
                case MaterialType.RoseQuartz:
                    return 25.0f;

                default:
                    return 20.0f;
            }
        }

        /// <summary>
        /// Thanks to Endy's Tinkering Calculator for these values!
        /// </summary>
        public static List<float> TinkeringDifficulty = new List<float>()
        {
            // attempt #
            1.0f,   // 1
            1.1f,   // 2
            1.3f,   // 3
            1.6f,   // 4
            2.0f,   // 5
            2.5f,   // 6
            3.0f,   // 7
            3.5f,   // 8
            4.0f,   // 9
            4.5f    // 10
        };

        public static void ShowDialog(Player player, WorldObject source, WorldObject target, Recipe recipe, double successChance)
        {
            var percent = successChance * 100;

            // retail messages:

            // You determine that you have a 100 percent chance to succeed.
            // You determine that you have a 99 percent chance to succeed.
            // You determine that you have a 38 percent chance to succeed. 5 percent is due to your augmentation.

            var floorMsg = $"You determine that you have a {percent.Round()} percent chance to succeed.";

            var numAugs = recipe.IsImbuing() ? player.AugmentationBonusImbueChance : 0;

            if (numAugs > 0)
                floorMsg += $"\n{numAugs * 5} percent is due to your augmentation.";

            if (!player.ConfirmationManager.EnqueueSend(new Confirmation_CraftInteration(player.Guid, source.Guid, target.Guid), floorMsg))
            {
                player.SendUseDoneEvent(WeenieError.ConfirmationInProgress);
                return;
            }

            if (PropertyManager.GetBool("craft_exact_msg").Item)
            {
                var exactMsg = $"You have a {(float)percent} percent chance of using {source.NameWithMaterial} on {target.NameWithMaterial}.";

                player.Session.Network.EnqueueSend(new GameMessageSystemChat(exactMsg, ChatMessageType.Craft));
            }
            player.SendUseDoneEvent();
        }

        public static void HandleRecipe(Player player, WorldObject source, WorldObject target, Recipe recipe, double successChance)
        {
            // re-verify
            if (!VerifyRequirements(recipe, player, source, target))
            {
                player.SendWeenieError(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            var success = ThreadSafeRandom.Next(0.0f, 1.0f) < successChance;

            if (recipe.IsImbuing())
            {
                player.ImbueAttempts++;
                if (success) player.ImbueSuccesses++;
            }

            var modified = CreateDestroyItems(player, recipe, source, target, successChance, success);

            if (modified != null)
            {
                if (modified.Contains(source.Guid.Full))
                    UpdateObj(player, source);

                if (modified.Contains(target.Guid.Full))
                    UpdateObj(player, target);
            }

            if (success && recipe.Skill > 0 && recipe.Difficulty > 0)
            {
                var skill = player.GetCreatureSkill((Skill)recipe.Skill);
                Proficiency.OnSuccessUse(player, skill, recipe.Difficulty);
            }
        }

        /// <summary>
        /// Sends an UpdateObj to the client for modified sources / targets
        /// </summary>
        private static void UpdateObj(Player player, WorldObject obj)
        {
            if (Debug)
                Console.WriteLine($"{player.Name}.UpdateObj({obj.Name})");

            player.EnqueueBroadcast(new GameMessageUpdateObject(obj));

            if (obj.CurrentWieldedLocation != null)
            {
                // retail possibly required sources / targets to be in the player's inventory,
                // and not equipped. this scenario might already be prevented beforehand in VerifyUse()
                player.EnqueueBroadcast(new GameMessageObjDescEvent(player));
                return;
            }

            // client automatically moves item to first slot in container
            // when an UpdateObject is sent. we must mimic this process on the server for persistance

            // only run this for items in the player's inventory
            // ie. skip for items on landblock, such as chorizite ore

            var invObj = player.FindObject(obj.Guid.Full, Player.SearchLocations.MyInventory);

            if (invObj != null)
                player.MoveItemToFirstContainerSlot(obj);
        }

        public static bool TryMutateNative(Player player, WorldObject source, WorldObject target, Recipe recipe, uint dataId)
        {
            // legacy method, unused by default
            switch (dataId)
            {
                // armor tinkering
                case 0x38000011:    // Steel
                    target.ArmorLevel += 20;
                    break;

                 // mutations apparently didn't cap to 2.0 here, clamps are applied in damage calculations though

                case 0x38000017:    // Alabaster    
                    //target.ArmorModVsPierce = Math.Min((target.ArmorModVsPierce ?? 0) + 0.2f, 2.0f);
                    target.ArmorModVsPierce += 0.2f;
                    break;
                case 0x38000018:    // Bronze
                    //target.ArmorModVsSlash = Math.Min((target.ArmorModVsSlash ?? 0) + 0.2f, 2.0f);
                    target.ArmorModVsSlash += 0.2f;
                    break;
                case 0x38000013:    // Marble
                    //target.ArmorModVsBludgeon = Math.Min((target.ArmorModVsBludgeon ?? 0) + 0.2f, 2.0f);
                    target.ArmorModVsBludgeon += 0.2f;
                    break;
                case 0x38000012:    // ArmoredilloHide
                    //target.ArmorModVsAcid = Math.Min((target.ArmorModVsAcid ?? 0) + 0.4f, 2.0f);
                    target.ArmorModVsAcid += 0.4f;
                    break;
                case 0x38000016:    // Ceramic
                    //target.ArmorModVsFire = Math.Min((target.ArmorModVsFire ?? 0) + 0.4f, 2.0f);
                    target.ArmorModVsFire += 0.4f;
                    break;
                case 0x38000014:    // Wool
                    //target.ArmorModVsCold = Math.Min((target.ArmorModVsCold ?? 0) + 0.4f, 2.0f);
                    target.ArmorModVsCold += 0.4f;
                    break;
                case 0x38000015:    // ReedSharkHide
                    //target.ArmorModVsElectric = Math.Min((target.ArmorModVsElectric ?? 0) + 0.4f, 2.0f);
                    target.ArmorModVsElectric += 0.4f;
                    break;
                case 0x38000038:    // Peridot
                    //AddImbuedEffect(target, ImbuedEffectType.MeleeDefense);
                    target.ImbuedEffect = ImbuedEffectType.MeleeDefense;
                    break;
                case 0x38000039:    // YellowTopaz
                    //AddImbuedEffect(target, ImbuedEffectType.MissileDefense);
                    target.ImbuedEffect = ImbuedEffectType.MissileDefense;
                    break;
                case 0x38000037:    // Zircon
                    //AddImbuedEffect(target, ImbuedEffectType.MagicDefense);
                    target.ImbuedEffect = ImbuedEffectType.MagicDefense;
                    break;

                // item tinkering
                case 0x3800001E:    // Pine
                    //target.Value = (int)Math.Round((target.Value ?? 1) * 0.75f);
                    target.Value = (int?)(target.Value * 0.75f);
                    break;
                case 0x3800001F:    // Gold
                    //target.Value = (int)Math.Round((target.Value ?? 1) * 1.25f);
                    target.Value = (int?)(target.Value * 1.25f);
                    break;
                case 0x38000019:    // Linen
                    //target.EncumbranceVal = (int)Math.Round((target.EncumbranceVal ?? 1) * 0.75f);
                    target.EncumbranceVal = (int?)(target.EncumbranceVal * 0.75f);
                    break;
                // Ivory is handled purely in recipe mod?
                case 0x38000043:    // Leather
                    target.Retained = true;
                    break;
                case 0x3900004E:    // Sandstone: 43 -> 4E
                    target.Retained = false;
                    break;
                case 0x3800002F:    // Moonstone
                    target.ItemMaxMana += 500;
                    break;

                case 0x39000042:
                    // legacy, these are handled in recipe mods
                    //target.SetProperty(PropertyString.ItemHeritageGroupRestriction, "Aluvian");     // Teak
                    //target.SetProperty(PropertyString.ItemHeritageGroupRestriction, "Gharu'ndim");  // Ebony
                    //target.SetProperty(PropertyString.ItemHeritageGroupRestriction, "Sho");         // Porcelain
                    //target.SetProperty(PropertyString.ItemHeritageGroupRestriction, "Viamontian");  // Satin
                    break;

                case 0x38000035:    // Copper

                    // handled in requirements, only here for legacy support?
                    if (target.ItemSkillLimit != Skill.MissileDefense || target.ItemSkillLevelLimit == null)
                        return false;

                    // change activation requirement: missile defense -> melee defense
                    target.ItemSkillLimit = Skill.MeleeDefense;
                    target.ItemSkillLevelLimit = (int)(target.ItemSkillLevelLimit / 0.7f);
                    break;

                case 0x38000034:    // Silver

                    // handled in requirements, only here for legacy support?
                    if (target.ItemSkillLimit != Skill.MeleeDefense || target.ItemSkillLevelLimit == null)
                        return false;

                    // change activation requirement: melee defense -> missile defense
                    target.ItemSkillLimit = Skill.MissileDefense;
                    target.ItemSkillLevelLimit = (int)(target.ItemSkillLevelLimit * 0.7f);
                    break;

                case 0x38000036:    // Silk

                    // remove allegiance rank limit, set difficulty to spellcraft
                    target.ItemAllegianceRankLimit = null;
                    target.ItemDifficulty = target.ItemSpellcraft;
                    break;

                // armatures / trinkets
                // these are handled in recipe mod
                case 0x39000048:    // Amber
                case 0x39000049:    // Diamond
                case 0x39000050:    // GromnieHide
                case 0x39000051:    // Pyreal
                case 0x39000052:    // Ruby
                case 0x39000053:    // Sapphire
                    return false;

                // magic item tinkering

                case 0x38000025: // Sunstone
                    //AddImbuedEffect(target, ImbuedEffectType.ArmorRending);
                    target.ImbuedEffect = ImbuedEffectType.ArmorRending;
                    break;
                case 0x38000024: // FireOpal
                    //AddImbuedEffect(target, ImbuedEffectType.CripplingBlow);
                    target.ImbuedEffect = ImbuedEffectType.CripplingBlow;
                    break;
                case 0x38000023:    // BlackOpal
                    //AddImbuedEffect(target, ImbuedEffectType.CriticalStrike);
                    target.ImbuedEffect = ImbuedEffectType.CriticalStrike;
                    break;
                case 0x3800002E:    // Opal
                    //target.ManaConversionMod += 0.01f;
                    target.ManaConversionMod = (target.ManaConversionMod ?? 0.0f) + 0.01f;
                    break;
                case 0x3800004B:    // GreenGarnet: 44 -> 4B
                    target.ElementalDamageMod = (target.ElementalDamageMod ?? 0.0f) + 0.01f;     // + 1% vs. monsters, + 0.25% vs. players
                    break;

                case 0x38000041:
                    // these are handled in recipe mods already
                    // SmokeyQuartz
                    //AddSpell(player, target, SpellId.CANTRIPCOORDINATION1);
                    // RoseQuartz
                    //AddSpell(player, target, SpellId.CANTRIPQUICKNESS1);
                    // RedJade
                    //AddSpell(player, target, SpellId.CANTRIPHEALTHGAIN1);
                    // Malachite
                    //AddSpell(player, target, SpellId.WarriorsVigor);
                    // LavenderJade
                    //AddSpell(player, target, SpellId.CANTRIPMANAGAIN1);
                    // LapisLazuli
                    //AddSpell(player, target, SpellId.CANTRIPWILLPOWER1);
                    // Hematite
                    //AddSpell(player, target, SpellId.WarriorsVitality);
                    // Citrine
                    //AddSpell(player, target, SpellId.CANTRIPSTAMINAGAIN1);
                    // Carnelian
                    //AddSpell(player, target, SpellId.CANTRIPSTRENGTH1);
                    // Bloodstone
                    //AddSpell(player, target, SpellId.CANTRIPENDURANCE1);
                    // Azurite
                    //AddSpell(player, target, SpellId.WizardsIntellect);
                    // Agate
                    //AddSpell(player, target, SpellId.CANTRIPFOCUS1);

                    target.ImbuedEffect = ImbuedEffectType.Spellbook;
                    break;

                // weapon tinkering

                case 0x3800001A:    // Iron
                    target.Damage += 1;
                    break;
                case 0x3800001B:    // Mahogany
                    target.DamageMod += 0.04f;
                    break;
                case 0x3800001C:    // Granite / Lucky Rabbit's Foot
                    target.DamageVariance *= 0.8f;
                    break;
                case 0x3800001D:    // Oak
                    target.WeaponTime = Math.Max(0, (target.WeaponTime ?? 0) - 50);
                    break;
                case 0x38000020:    // Brass
                    target.WeaponDefense += 0.01f;
                    break;
                case 0x38000021:    // Velvet
                    target.WeaponOffense += 0.01f;
                    break;

                // only 1 imbue can be applied per piece of armor?
                case 0x3800003A:    // Emerald
                    //AddImbuedEffect(target, ImbuedEffectType.AcidRending);
                    target.ImbuedEffect = ImbuedEffectType.AcidRending;
                    break;
                case 0x3800003B:    // WhiteSapphire
                    //AddImbuedEffect(target, ImbuedEffectType.BludgeonRending);
                    target.ImbuedEffect = ImbuedEffectType.BludgeonRending;
                    break;
                case 0x3800003C:    // Aquamarine
                    //AddImbuedEffect(target, ImbuedEffectType.ColdRending);
                    target.ImbuedEffect = ImbuedEffectType.ColdRending;
                    break;
                case 0x3800003D:    // Jet
                    //AddImbuedEffect(target, ImbuedEffectType.ElectricRending);
                    target.ImbuedEffect = ImbuedEffectType.ElectricRending;
                    break;
                case 0x3800003E:    // RedGarnet
                    //AddImbuedEffect(target, ImbuedEffectType.FireRending);
                    target.ImbuedEffect = ImbuedEffectType.FireRending;
                    break;
                case 0x3800003F:    // BlackGarnet
                    //AddImbuedEffect(target, ImbuedEffectType.PierceRending);
                    target.ImbuedEffect = ImbuedEffectType.PierceRending;
                    break;
                case 0x38000040:    // ImperialTopaz
                    //AddImbuedEffect(target, ImbuedEffectType.SlashRending);
                    target.ImbuedEffect = ImbuedEffectType.SlashRending;
                    break;

                // addons

                case 0x3800000F:    // Stamps
                    target.IconOverlayId = target.IconOverlaySecondary;
                    break;

                case 0x38000046:    // Fetish of the Dark Idols

                    // shouldn't exist on player items, but just recreating original script here
                    if (target.ImbuedEffect >= ImbuedEffectType.IgnoreAllArmor)  
                        target.ImbuedEffect = ImbuedEffectType.Undef;

                    target.ImbuedEffect |= ImbuedEffectType.IgnoreSomeMagicProjectileDamage;
                    //target.AbsorbMagicDamage = 0.25f;   // not in original mods / mutation?
                    break;

                case 0x39000000:    // Paragon Weapons
                    target.ItemMaxLevel = (target.ItemMaxLevel ?? 0) + 1;
                    target.ItemBaseXp = 2000000000;
                    target.ItemTotalXp = target.ItemTotalXp ?? 0;
                    break;

                default:
                    log.Error($"{player.Name}.RecipeManager.Tinkering_ModifyItem({source.Name} ({source.Guid}), {target.Name} ({target.Guid})) - unknown mutation id: {dataId:X8}");
                    return false;
            }

            if (incItemTinkered.Contains(dataId))
                HandleTinkerLog(source, target);

            return true;
        }

        // only needed for legacy method
        // ideally this wouldn't even be needed for the legacy method, and recipe.IsTinkering() would suffice
        // however, that would break for rare salvages, which have 0 difficulty and salvage_Type 0
        private static readonly HashSet<uint> incItemTinkered = new HashSet<uint>()
        {
            0x38000011, // Steel
            0x38000012, // Armoredillo Hide
            0x38000013, // Marble
            0x38000014, // Wool
            0x38000015, // Reedshark Hide
            0x38000016, // Ceramic
            0x38000017, // Alabaster
            0x38000018, // Bronze
            0x38000019, // Linen
            0x3800001A, // Iron
            0x3800001B, // Mahogany
            0x3800001C, // Granite
            0x3800001D, // Oak
            0x3800001E, // Pine
            0x3800001F, // Gold
            0x38000020, // Brass
            0x38000021, // Velvet
            0x38000023, // Black Opal
            0x38000024, // Fire Opal
            0x38000025, // Sunstone
            0x3800002E, // Opal
            0x3800002F, // Moonstone
            0x38000034, // Silver
            0x38000035, // Copper
            0x38000036, // Silk
            0x38000037, // Zircon
            0x38000038, // Peridot
            0x38000039, // Yellow Topaz
            0x3800003A, // Emerald
            0x3800003B, // White Sapphire
            0x3800003C, // Aquamarine
            0x3800003D, // Jet
            0x3800003E, // Red Garnet
            0x3800003F, // Black Garnet
            0x38000040, // Imperial Topaz
            0x38000041, // Cantrips
            0x38000042, // Heritage
            0x3800004B, // Green Garnet
        };

        public static void AddSpell(Player player, WorldObject target, SpellId spell, int difficulty = 25)
        {
            target.Biota.GetOrAddKnownSpell((int)spell, target.BiotaDatabaseLock, out _);
            target.ChangesDetected = true;

            if (difficulty != 0)
            {
                target.ItemSpellcraft = (target.ItemSpellcraft ?? 0) + difficulty;
                target.ItemDifficulty = (target.ItemDifficulty ?? 0) + difficulty;
            }
            if (target.UiEffects == null)
            {
                target.UiEffects = UiEffects.Magical;
                player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(target, PropertyInt.UiEffects, (int)target.UiEffects));
            }
        }

        // derrick's input => output mappings
        public static Dictionary<ImbuedEffectType, uint> IconUnderlay = new Dictionary<ImbuedEffectType, uint>()
        {
            { ImbuedEffectType.ColdRending,     0x06003353 },
            { ImbuedEffectType.ElectricRending, 0x06003354 },
            { ImbuedEffectType.AcidRending,     0x06003355 },
            { ImbuedEffectType.ArmorRending,    0x06003356 },
            { ImbuedEffectType.CripplingBlow,   0x06003357 },
            { ImbuedEffectType.CriticalStrike,  0x06003358 },
            { ImbuedEffectType.FireRending,     0x06003359 },
            { ImbuedEffectType.BludgeonRending, 0x0600335a },
            { ImbuedEffectType.PierceRending,   0x0600335b },
            { ImbuedEffectType.SlashRending,    0x0600335c },
        };

        public static ImbuedEffectType GetImbuedEffects(WorldObject target)
        {
            var imbuedEffects = 0;

            imbuedEffects |= target.GetProperty(PropertyInt.ImbuedEffect) ?? 0;
            imbuedEffects |= target.GetProperty(PropertyInt.ImbuedEffect2) ?? 0;
            imbuedEffects |= target.GetProperty(PropertyInt.ImbuedEffect3) ?? 0;
            imbuedEffects |= target.GetProperty(PropertyInt.ImbuedEffect4) ?? 0;
            imbuedEffects |= target.GetProperty(PropertyInt.ImbuedEffect5) ?? 0;

            return (ImbuedEffectType)imbuedEffects;
        }

        public static bool VerifyRequirements(Recipe recipe, Player player, WorldObject source, WorldObject target)
        {
            if (!VerifyUse(player, source, target))
                return false;

            if (!VerifyRequirements(recipe, player, target, RequirementType.Target)) return false;

            if (!VerifyRequirements(recipe, player, source, RequirementType.Source)) return false;

            if (!VerifyRequirements(recipe, player, player, RequirementType.Player)) return false;

            return true;
        }

        public static bool VerifyUse(Player player, WorldObject source, WorldObject target)
        {
            var usable = source.ItemUseable ?? Usable.Undef;

            if (usable == Usable.Undef)
            {
                log.Warn($"{player.Name}.RecipeManager.VerifyUse({source.Name} ({source.Guid}), {target.Name} ({target.Guid})) - source not usable, falling back on defaults");

                // re-verify
                if (player.FindObject(source.Guid.Full, Player.SearchLocations.MyInventory) == null)
                    return false;

                // almost always MyInventory, but sometimes can be applied to equipped
                if (player.FindObject(target.Guid.Full, Player.SearchLocations.MyInventory | Player.SearchLocations.MyEquippedItems) == null)
                    return false;

                return true;
            }

            var sourceUse = usable.GetSourceFlags();
            var targetUse = usable.GetTargetFlags();

            return VerifyUse(player, source, sourceUse) && VerifyUse(player, target, targetUse);
        }

        public static bool VerifyUse(Player player, WorldObject obj, Usable usable)
        {
            var searchLocations = Player.SearchLocations.None;

            // TODO: figure out other Usable flags
            if (usable.HasFlag(Usable.Contained))
                searchLocations |= Player.SearchLocations.MyInventory | Player.SearchLocations.MyEquippedItems;
            if (usable.HasFlag(Usable.Wielded))
                searchLocations |= Player.SearchLocations.MyEquippedItems;
            if (usable.HasFlag(Usable.Remote))
                searchLocations |= Player.SearchLocations.LocationsICanMove;    // TODO: moveto for this type

            return player.FindObject(obj.Guid.Full, searchLocations) != null;
        }

        public static bool Debug = false;

        public static bool VerifyRequirements(Recipe recipe, Player player, WorldObject obj, RequirementType reqType)
        {
            var boolReqs = recipe.RecipeRequirementsBool.Where(i => i.Index == (int)reqType).ToList();
            var intReqs = recipe.RecipeRequirementsInt.Where(i => i.Index == (int)reqType).ToList();
            var floatReqs = recipe.RecipeRequirementsFloat.Where(i => i.Index == (int)reqType).ToList();
            var strReqs = recipe.RecipeRequirementsString.Where(i => i.Index == (int)reqType).ToList();
            var iidReqs = recipe.RecipeRequirementsIID.Where(i => i.Index == (int)reqType).ToList();
            var didReqs = recipe.RecipeRequirementsDID.Where(i => i.Index == (int)reqType).ToList();

            var totalReqs = boolReqs.Count + intReqs.Count + floatReqs.Count + strReqs.Count + iidReqs.Count + didReqs.Count;

            if (Debug && totalReqs > 0)
                Console.WriteLine($"{reqType} Requirements: {totalReqs}");

            foreach (var requirement in boolReqs)
            {
                bool? value = obj.GetProperty((PropertyBool)requirement.Stat);
                double? normalized = value != null ? (double?)Convert.ToDouble(value.Value) : null;

                if (Debug)
                    Console.WriteLine($"PropertyBool.{(PropertyBool)requirement.Stat} {(CompareType)requirement.Enum} {requirement.Value}, current: {value}");

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, normalized, Convert.ToDouble(requirement.Value), requirement.Message))
                    return false;
            }

            foreach (var requirement in intReqs)
            {
                int? value = obj.GetProperty((PropertyInt)requirement.Stat);
                double? normalized = value != null ? (double?)Convert.ToDouble(value.Value) : null;

                if (Debug)
                    Console.WriteLine($"PropertyInt.{(PropertyInt)requirement.Stat} {(CompareType)requirement.Enum} {requirement.Value}, current: {value}");

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, normalized, Convert.ToDouble(requirement.Value), requirement.Message))
                    return false;
            }

            foreach (var requirement in floatReqs)
            {
                double? value = obj.GetProperty((PropertyFloat)requirement.Stat);

                if (Debug)
                    Console.WriteLine($"PropertyFloat.{(PropertyFloat)requirement.Stat} {(CompareType)requirement.Enum} {requirement.Value}, current: {value}");

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, value, requirement.Value, requirement.Message))
                    return false;
            }

            foreach (var requirement in strReqs)
            {
                string value = obj.GetProperty((PropertyString)requirement.Stat);

                if (Debug)
                    Console.WriteLine($"PropertyString.{(PropertyString)requirement.Stat} {(CompareType)requirement.Enum} {requirement.Value}, current: {value}");

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, value, requirement.Value, requirement.Message))
                    return false;
            }

            foreach (var requirement in iidReqs)
            {
                uint? value = obj.GetProperty((PropertyInstanceId)requirement.Stat);
                double? normalized = value != null ? (double?)Convert.ToDouble(value.Value) : null;

                if (Debug)
                    Console.WriteLine($"PropertyInstanceId.{(PropertyInstanceId)requirement.Stat} {(CompareType)requirement.Enum} {requirement.Value}, current: {value}");

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, normalized, Convert.ToDouble(requirement.Value), requirement.Message))
                    return false;
            }

            foreach (var requirement in didReqs)
            {
                uint? value = obj.GetProperty((PropertyDataId)requirement.Stat);
                double? normalized = value != null ? (double?)Convert.ToDouble(value.Value) : null;

                if (Debug)
                    Console.WriteLine($"PropertyDataId.{(PropertyDataId)requirement.Stat} {(CompareType)requirement.Enum} {requirement.Value}, current: {value}");

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, normalized, Convert.ToDouble(requirement.Value), requirement.Message))
                    return false;
            }

            if (Debug && totalReqs > 0)
                Console.WriteLine($"-----");

            return true;
        }

        public static bool VerifyRequirement(Player player, CompareType compareType, double? prop, double val, string failMsg)
        {
            var success = true;

            switch (compareType)
            {
                case CompareType.GreaterThan:
                    if ((prop ?? 0) > val)
                        success = false;
                    break;

                case CompareType.LessThanEqual:
                    if ((prop ?? 0) <= val)
                        success = false;
                    break;

                case CompareType.LessThan:
                    if ((prop ?? 0) < val)
                        success = false;
                    break;

                case CompareType.GreaterThanEqual:
                    if ((prop ?? 0) >= val)
                        success = false;
                    break;

                case CompareType.NotEqual:
                    if ((prop ?? 0) != val)
                        success = false;
                    break;

                case CompareType.NotEqualNotExist:
                    if (prop == null || prop.Value != val)
                        success = false;
                    break;

                case CompareType.Equal:
                    if ((prop ?? 0) == val)
                        success = false;
                    break;

                case CompareType.NotExist:
                    if (prop == null)
                        success = false;
                    break;

                case CompareType.Exist:
                    if (prop != null)
                        success = false;
                    break;

                case CompareType.NotHasBits:
                    if (((int)(prop ?? 0) & (int)val) == 0)
                        success = false;
                    break;

                case CompareType.HasBits:
                    if (((int)(prop ?? 0) & (int)val) == (int)val)
                        success = false;
                    break;
            }

            if (!success)
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(failMsg, ChatMessageType.Craft));

            return success;
        }

        public static bool VerifyRequirement(Player player, CompareType compareType, string prop, string val, string failMsg)
        {
            var success = true;

            switch (compareType)
            {
                case CompareType.NotEqual:
                    if (!(prop ?? "").Equals(val))
                        success = false;
                    break;

                case CompareType.NotEqualNotExist:
                    if (prop == null || !prop.Equals(val))
                        success = false;
                    break;

                case CompareType.Equal:
                    if ((prop ?? "").Equals(val))
                        success = false;
                    break;

                case CompareType.NotExist:
                    if (prop == null)
                        success = false;
                    break;

                case CompareType.Exist:
                    if (prop != null)
                        success = false;
                    break;
            }
            if (!success)
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(failMsg, ChatMessageType.Craft));

            return success;
        }

        /// <summary>
        /// Returns a list of object guids that have been modified
        /// </summary>
        public static HashSet<uint> CreateDestroyItems(Player player, Recipe recipe, WorldObject source, WorldObject target, double successChance, bool success)
        {
            var destroyTargetChance = success ? recipe.SuccessDestroyTargetChance : recipe.FailDestroyTargetChance;
            var destroySourceChance = success ? recipe.SuccessDestroySourceChance : recipe.FailDestroySourceChance;

            var destroyTarget = ThreadSafeRandom.Next(0.0f, 1.0f) < destroyTargetChance;
            var destroySource = ThreadSafeRandom.Next(0.0f, 1.0f) < destroySourceChance;

            var createItem = success ? recipe.SuccessWCID : recipe.FailWCID;
            var createAmount = success ? recipe.SuccessAmount : recipe.FailAmount;

            if (createItem > 0 && DatabaseManager.World.GetCachedWeenie(createItem) == null)
            {
                log.Error($"RecipeManager.CreateDestroyItems: Recipe.Id({recipe.Id}) couldn't find {(success ? "Success" : "Fail")}WCID {createItem} in database.");
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.CraftGeneralErrorUiMsg));
                return null;
            }

            if (destroyTarget)
            {
                var destroyTargetAmount = success ? recipe.SuccessDestroyTargetAmount : recipe.FailDestroyTargetAmount;
                var destroyTargetMessage = success ? recipe.SuccessDestroyTargetMessage : recipe.FailDestroyTargetMessage;

                DestroyItem(player, recipe, target, destroyTargetAmount, destroyTargetMessage);
            }

            if (destroySource)
            {
                var destroySourceAmount = success ? recipe.SuccessDestroySourceAmount : recipe.FailDestroySourceAmount;
                var destroySourceMessage = success ? recipe.SuccessDestroySourceMessage : recipe.FailDestroySourceMessage;

                DestroyItem(player, recipe, source, destroySourceAmount, destroySourceMessage);
            }

            WorldObject result = null;

            if (createItem > 0)
                result = CreateItem(player, createItem, createAmount);

            var modified = ModifyItem(player, recipe, source, target, result, success);

            // broadcast different messages based on recipe type
            if (!recipe.IsTinkering())
            {
                var message = success ? recipe.SuccessMessage : recipe.FailMessage;

                player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Craft));

                if (log.IsDebugEnabled)
                    log.Debug($"[CRAFTING] {player.Name} used {source.NameWithMaterial} on {target.NameWithMaterial} {(success ? "" : "un")}successfully. {(destroySource ? $"| {source.NameWithMaterial} was destroyed " : "")}{(destroyTarget ? $"| {target.NameWithMaterial} was destroyed " : "")}| {message}");
            }
            else
                BroadcastTinkering(player, source, target, successChance, success);

            return modified;
        }

        public static void BroadcastTinkering(Player player, WorldObject tool, WorldObject target, double chance, bool success)
        {
            // retail AC had some inconsistency with respect to the messages broadcast by tinkering.
            //
            // Largely this revolved around the name of the weenies that represented each of the salvage bags.
            // First, there were name changes that were a result of the client side pre-pending of material type which resulted in bags being named "Salvage", "Salvaged"
            // Second, these bags that were generated from loot by players always ended with the number of materials in the bag, such as (100), (88), (1)
            // while non-lootgen bags did not include the number, so the name of the bag when displayed or broadcast varied like "Steel Salvage (100)", "Steel Salvaged (100)", "Steel Salvage"
            // Third, Foolproof bags, again depending on weenie names, also had their own variations which resulted in display names like "Foolproof Black Garnet Gem", "Zircon Foolproof Zircon", "Imperial Topaz Foolproof Imperial Topaz"
            // in many cases, there are multiple weenies of a particular salvage type, used by various systems, which resulted in each tinker operation having varied output even when doing essentially the same thing
            // Finally, items that were inscribed were surprisingly identified in the broadcast like "Reed Shark Hide Studded Leather Sleeves inscribed by Callaway", "Copper Frost Bow inscribed by Mini Bonsai"
            //
            // ACE output, as seen below, has for the most part standardized the message due to weenie name changes, salvage coding and recipe handling differences
            // 
            var sourceName = Regex.Replace(tool.NameWithMaterial, @" \(\d+\)$", "");

            var msg = $"{player.Name} {(success ? "successfully applies" : "fails to apply")} the {sourceName} (workmanship {(tool.Workmanship ?? 0):#.00}) to the {target.NameWithMaterial}{((target.Inscription != null && target.ScribeName != null) ? $" inscribed by {target.ScribeName}" : "")}.{(!success ? " The target is destroyed." : "")}";

            // send local broadcast
            player.EnqueueBroadcast(new GameMessageSystemChat(msg, ChatMessageType.Craft), WorldObject.LocalBroadcastRange, ChatMessageType.Craft);

            if (log.IsDebugEnabled)
                log.Debug($"[TINKERING] {msg} | Chance: {chance}");
        }

        public static WorldObject CreateItem(Player player, uint wcid, uint amount)
        {
            var wo = WorldObjectFactory.CreateNewWorldObject(wcid);

            if (wo == null)
            {
                log.Warn($"RecipeManager.CreateItem({player.Name}, {wcid}, {amount}): failed to create {wcid}");
                return null;
            }

            if (amount > 1)
                wo.SetStackSize((int)amount);

            player.TryCreateInInventoryWithNetworking(wo);
            return wo;
        }

        public static void DestroyItem(Player player, Recipe recipe, WorldObject item, uint amount, string msg)
        {
            if (item.OwnerId == player.Guid.Full || player.GetInventoryItem(item.Guid) != null)
            {
                if (!player.TryConsumeFromInventoryWithNetworking(item, (int)amount))
                    log.Warn($"RecipeManager.DestroyItem({player.Name}, {item.Name}, {amount}, {msg}): failed to remove {item.Name}");
            }
            else if (item.WielderId == player.Guid.Full)
            {
                if (!player.TryDequipObjectWithNetworking(item.Guid, out _, Player.DequipObjectAction.ConsumeItem))
                    log.Warn($"RecipeManager.DestroyItem({player.Name}, {item.Name}, {amount}, {msg}): failed to remove {item.Name}");
            }
            else
            {
                item.Destroy();
            }
            if (!string.IsNullOrEmpty(msg))
            {
                var destroyMessage = new GameMessageSystemChat(msg, ChatMessageType.Craft);
                player.Session.Network.EnqueueSend(destroyMessage);
            }
        }

        public static WorldObject GetSourceMod(RecipeSourceType sourceType, Player player, WorldObject source)
        {
            switch (sourceType)
            {
                case RecipeSourceType.Player:
                    return player;
                case RecipeSourceType.Source:
                    return source;
            }
            log.Warn($"RecipeManager.GetSourceMod({sourceType}, {player.Name}, {source.Name}) - unknown source type");
            return null;
        }

        public static WorldObject GetTargetMod(ModificationType type, WorldObject source, WorldObject target, Player player, WorldObject result)
        {
            switch (type)
            {
                case ModificationType.SuccessSource:
                case ModificationType.FailureSource:
                    return source;

                default:
                    return target;

                case ModificationType.SuccessPlayer:
                case ModificationType.FailurePlayer:
                    return player;

                case ModificationType.SuccessResult:
                case ModificationType.FailureResult:
                    return result ?? target;
            }
        }

        /// <summary>
        /// Returns a list of object guids that have been modified
        /// </summary>
        public static HashSet<uint> ModifyItem(Player player, Recipe recipe, WorldObject source, WorldObject target, WorldObject result, bool success)
        {
            var modified = new HashSet<uint>();

            foreach (var mod in recipe.RecipeMod)
            {
                if (mod.ExecutesOnSuccess != success)
                    continue;

                // adjust vitals
                if (mod.Health != 0)
                    ModifyVital(player, PropertyAttribute2nd.Health, mod.Health);

                if (mod.Stamina != 0)
                    ModifyVital(player, PropertyAttribute2nd.Stamina, mod.Stamina);

                if (mod.Mana != 0)
                    ModifyVital(player, PropertyAttribute2nd.Mana, mod.Mana);

                // apply type mods
                foreach (var boolMod in mod.RecipeModsBool)
                    ModifyBool(player, boolMod, source, target, result, modified);

                foreach (var intMod in mod.RecipeModsInt)
                    ModifyInt(player, intMod, source, target, result, modified);

                foreach (var floatMod in mod.RecipeModsFloat)
                    ModifyFloat(player, floatMod, source, target, result, modified);

                foreach (var stringMod in mod.RecipeModsString)
                    ModifyString(player, stringMod, source, target, result, modified);

                foreach (var iidMod in mod.RecipeModsIID)
                    ModifyInstanceID(player, iidMod, source, target, result, modified);

                foreach (var didMod in mod.RecipeModsDID)
                    ModifyDataID(player, didMod, source, target, result, modified);

                // run mutation script, if applicable
                if (mod.DataId != 0)
                    TryMutate(player, source, target, recipe, (uint)mod.DataId, modified);
            }

            return modified;
        }

        private static void ModifyVital(Player player, PropertyAttribute2nd attribute2nd, int value)
        {
            var vital = player.GetCreatureVital(attribute2nd);

            var vitalChange = (uint)Math.Abs(player.UpdateVitalDelta(vital, value));

            if (attribute2nd == PropertyAttribute2nd.Health)
            {
                if (value >= 0)
                    player.DamageHistory.OnHeal(vitalChange);
                else
                    player.DamageHistory.Add(player, DamageType.Health, vitalChange);

                if (player.Health.Current <= 0)
                {
                    // should this be possible?
                    //var lastDamager = player != null ? new DamageHistoryInfo(player) : null;
                    var lastDamager = new DamageHistoryInfo(player);

                    player.OnDeath(lastDamager, DamageType.Health, false);
                    player.Die();
                }
            }
        }

        public static void ModifyBool(Player player, RecipeModsBool boolMod, WorldObject source, WorldObject target, WorldObject result, HashSet<uint> modified)
        {
            var op = (ModificationOperation)boolMod.Enum;
            var prop = (PropertyBool)boolMod.Stat;
            var value = boolMod.Value;

            var targetMod = GetTargetMod((ModificationType)boolMod.Index, source, target, player, result);

            // always SetValue?
            if (op != ModificationOperation.SetValue)
            {
                log.Warn($"RecipeManager.ModifyBool({source.Name}, {target.Name}): unhandled operation {op}");
                return;
            }
            player.UpdateProperty(targetMod, prop, value);
            modified.Add(targetMod.Guid.Full);

            if (Debug)
                Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
        }

        public static void ModifyInt(Player player, RecipeModsInt intMod, WorldObject source, WorldObject target, WorldObject result, HashSet<uint> modified)
        {
            var op = (ModificationOperation)intMod.Enum;
            var prop = (PropertyInt)intMod.Stat;
            var value = intMod.Value;

            var sourceMod = GetSourceMod((RecipeSourceType)intMod.Source, player, source);
            var targetMod = GetTargetMod((ModificationType)intMod.Index, source, target, player, result);

            switch (op)
            {
                case ModificationOperation.SetValue:
                    player.UpdateProperty(targetMod, prop, value);
                    modified.Add(targetMod.Guid.Full);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.Add:
                    player.UpdateProperty(targetMod, prop, (targetMod.GetProperty(prop) ?? 0) + value);
                    modified.Add(targetMod.Guid.Full);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.IncProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, sourceMod.GetProperty(prop) ?? 0);
                    modified.Add(target.Guid.Full);
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {sourceMod.GetProperty(prop) ?? 0}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, player.GetProperty(prop) ?? 0);     // ??
                    modified.Add(result.Guid.Full);
                    if (Debug) Console.WriteLine($"{result.Name}.SetProperty({prop}, {player.GetProperty(prop) ?? 0}) - {op}");
                    break;
                case ModificationOperation.AddSpell:
                    targetMod.Biota.GetOrAddKnownSpell(intMod.Stat, target.BiotaDatabaseLock, out var added);
                    modified.Add(targetMod.Guid.Full);
                    if (added)
                        targetMod.ChangesDetected = true;
                    if (Debug) Console.WriteLine($"{targetMod.Name}.AddSpell({intMod.Stat}) - {op}");
                    break;
                case ModificationOperation.SetBitsOn:
                    var bits = targetMod.GetProperty(prop) ?? 0;
                    bits |= value;
                    player.UpdateProperty(targetMod, prop, bits);
                    modified.Add(targetMod.Guid.Full);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, 0x{bits:X}) - {op}");
                    break;
                case ModificationOperation.SetBitsOff:
                    bits = targetMod.GetProperty(prop) ?? 0;
                    bits &= ~value;
                    player.UpdateProperty(targetMod, prop, bits);
                    modified.Add(targetMod.Guid.Full);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, 0x{bits:X}) - {op}");
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyInt({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyFloat(Player player, RecipeModsFloat floatMod, WorldObject source, WorldObject target, WorldObject result, HashSet<uint> modified)
        {
            var op = (ModificationOperation)floatMod.Enum;
            var prop = (PropertyFloat)floatMod.Stat;
            var value = floatMod.Value;

            var sourceMod = GetSourceMod((RecipeSourceType)floatMod.Source, player, source);
            var targetMod = GetTargetMod((ModificationType)floatMod.Index, source, target, player, result);

            switch (op)
            {
                case ModificationOperation.SetValue:
                    player.UpdateProperty(targetMod, prop, value);
                    modified.Add(targetMod.Guid.Full);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.Add:
                    player.UpdateProperty(targetMod, prop, (targetMod.GetProperty(prop) ?? 0) + value);
                    modified.Add(targetMod.Guid.Full);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.IncProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, sourceMod.GetProperty(prop) ?? 0);
                    modified.Add(target.Guid.Full);
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {sourceMod.GetProperty(prop) ?? 0}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, player.GetProperty(prop) ?? 0);
                    modified.Add(result.Guid.Full);
                    if (Debug) Console.WriteLine($"{result.Name}.SetProperty({prop}, {player.GetProperty(prop) ?? 0}) - {op}");
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyFloat({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyString(Player player, RecipeModsString stringMod, WorldObject source, WorldObject target, WorldObject result, HashSet<uint> modified)
        {
            var op = (ModificationOperation)stringMod.Enum;
            var prop = (PropertyString)stringMod.Stat;
            var value = stringMod.Value;

            var sourceMod = GetSourceMod((RecipeSourceType)stringMod.Source, player, source);
            var targetMod = GetTargetMod((ModificationType)stringMod.Index, source, target, player, result);

            switch (op)
            {
                case ModificationOperation.SetValue:
                    player.UpdateProperty(targetMod, prop, value);
                    modified.Add(targetMod.Guid.Full);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, sourceMod.GetProperty(prop) ?? sourceMod.Name);
                    modified.Add(target.Guid.Full);
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {sourceMod.GetProperty(prop) ?? sourceMod.Name}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, player.GetProperty(prop) ?? player.Name);
                    modified.Add(result.Guid.Full);
                    if (Debug) Console.WriteLine($"{result.Name}.SetProperty({prop}, {player.GetProperty(prop) ?? player.Name}) - {op}");
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyString({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyInstanceID(Player player, RecipeModsIID iidMod, WorldObject source, WorldObject target, WorldObject result, HashSet<uint> modified)
        {
            var op = (ModificationOperation)iidMod.Enum;
            var prop = (PropertyInstanceId)iidMod.Stat;
            var value = iidMod.Value;

            var sourceMod = GetSourceMod((RecipeSourceType)iidMod.Source, player, source);
            var targetMod = GetTargetMod((ModificationType)iidMod.Index, source, target, player, result);

            switch (op)
            {
                case ModificationOperation.SetValue:
                    player.UpdateProperty(targetMod, prop, value);
                    modified.Add(targetMod.Guid.Full);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, ModifyInstanceIDRuleSet(prop, sourceMod, targetMod));
                    modified.Add(target.Guid.Full);
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {ModifyInstanceIDRuleSet(prop, sourceMod, targetMod)}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, ModifyInstanceIDRuleSet(prop, player, targetMod));     // ??
                    modified.Add(result.Guid.Full);
                    if (Debug) Console.WriteLine($"{result.Name}.SetProperty({prop}, {ModifyInstanceIDRuleSet(prop, player, targetMod)}) - {op}");
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyInstanceID({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        private static uint ModifyInstanceIDRuleSet(PropertyInstanceId property, WorldObject sourceMod, WorldObject targetMod)
        {
            switch (property)
            {
                case PropertyInstanceId.AllowedWielder:
                case PropertyInstanceId.AllowedActivator:
                    return sourceMod.Guid.Full;
                default:
                    break;
            }

            return sourceMod.GetProperty(property) ?? 0;
        }

        public static void ModifyDataID(Player player, RecipeModsDID didMod, WorldObject source, WorldObject target, WorldObject result, HashSet<uint> modified)
        {
            var op = (ModificationOperation)didMod.Enum;
            var prop = (PropertyDataId)didMod.Stat;
            var value = didMod.Value;

            var sourceMod = GetSourceMod((RecipeSourceType)didMod.Source, player, source);
            var targetMod = GetTargetMod((ModificationType)didMod.Index, source, target, player, result);

            switch (op)
            {
                case ModificationOperation.SetValue:
                    player.UpdateProperty(targetMod, prop, value);
                    modified.Add(targetMod.Guid.Full);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, sourceMod.GetProperty(prop) ?? 0);
                    modified.Add(target.Guid.Full);
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {sourceMod.GetProperty(prop) ?? 0}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, player.GetProperty(prop) ?? 0);
                    modified.Add(result.Guid.Full);
                    if (Debug) Console.WriteLine($"{result.Name}.SetProperty({prop}, {player.GetProperty(prop) ?? 0}) - {op}");
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyDataID({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        /// <summary>
        /// flag to use c# logic instead of mutate script logic
        /// </summary>
        private static readonly bool useMutateNative = false;

        public static bool TryMutate(Player player, WorldObject source, WorldObject target, Recipe recipe, uint dataId, HashSet<uint> modified)
        {
            if (useMutateNative)
                return TryMutateNative(player, source, target, recipe, dataId);

            var numTimesTinkered = target.NumTimesTinkered;

            var mutationScript = MutationCache.GetMutation(dataId);

            if (mutationScript == null)
            {
                log.Error($"RecipeManager.TryApplyMutation({dataId:X8}, {target.Name}) - couldn't find mutation script");
                return false;
            }

            var result = mutationScript.TryMutate(target);

            if (numTimesTinkered != target.NumTimesTinkered)
                HandleTinkerLog(source, target);

            modified.Add(target.Guid.Full);

            return result;
        }

        private static void HandleTinkerLog(WorldObject source, WorldObject target)
        {
            if (target.TinkerLog != null)
                target.TinkerLog += ",";

            target.TinkerLog += (uint?)source.MaterialType ?? source.WeenieClassId;
        }

        public static uint MaterialDualDID = 0x27000000;

        public static string GetMaterialName(MaterialType materialType)
        {
            var dualDIDs = DatManager.PortalDat.ReadFromDat<DualDidMapper>(MaterialDualDID);

            if (!dualDIDs.ClientEnumToName.TryGetValue((uint)materialType, out var materialName))
            {
                log.Error($"RecipeManager.GetMaterialName({materialType}): couldn't find material name");
                return materialType.ToString();
            }
            return materialName.Replace("_", " ");
        }

        // todo: remove this once foolproof salvage recipes are updated
        private static readonly HashSet<WeenieClassName> foolproofTinkers = new HashSet<WeenieClassName>()
        {
            // rare foolproof
            WeenieClassName.W_MATERIALRAREFOOLPROOFAQUAMARINE_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKGARNET_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFBLACKOPAL_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFEMERALD_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFFIREOPAL_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFIMPERIALTOPAZ_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFJET_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFPERIDOT_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFREDGARNET_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFSUNSTONE_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFWHITESAPPHIRE_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFYELLOWTOPAZ_CLASS,
            WeenieClassName.W_MATERIALRAREFOOLPROOFZIRCON_CLASS,

            // regular foolproof
            WeenieClassName.W_MATERIALACE36619FOOLPROOFAQUAMARINE,
            WeenieClassName.W_MATERIALACE36620FOOLPROOFBLACKGARNET,
            WeenieClassName.W_MATERIALACE36621FOOLPROOFBLACKOPAL,
            WeenieClassName.W_MATERIALACE36622FOOLPROOFEMERALD,
            WeenieClassName.W_MATERIALACE36623FOOLPROOFFIREOPAL,
            WeenieClassName.W_MATERIALACE36624FOOLPROOFIMPERIALTOPAZ,
            WeenieClassName.W_MATERIALACE36625FOOLPROOFJET,
            WeenieClassName.W_MATERIALACE36626FOOLPROOFREDGARNET,
            WeenieClassName.W_MATERIALACE36627FOOLPROOFSUNSTONE,
            WeenieClassName.W_MATERIALACE36628FOOLPROOFWHITESAPPHIRE,
            WeenieClassName.W_MATERIALACE36634FOOLPROOFPERIDOT,
            WeenieClassName.W_MATERIALACE36635FOOLPROOFYELLOWTOPAZ,
            WeenieClassName.W_MATERIALACE36636FOOLPROOFZIRCON,
        };
    }

    public static class RecipeExtensions
    {
        public static bool IsTinkering(this Recipe recipe)
        {
            return recipe.SalvageType > 0;
        }

        public static bool IsImbuing(this Recipe recipe)
        {
            return recipe.SalvageType == 2;
        }
    }
}
