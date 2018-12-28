using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Factories;


namespace ACE.Server.Managers
{
    public class RecipeManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Random _random = new Random();

        public static void UseObjectOnTarget(Player player, WorldObject source, WorldObject target)
        {
            if (source == target)
            {
                var message = new GameMessageSystemChat($"The {source.Name} cannot be combined with itself.", ChatMessageType.Craft);
                player.Session.Network.EnqueueSend(message);
                player.SendUseDoneEvent();
                return;
            }

            var recipe = DatabaseManager.World.GetCachedCookbook(source.WeenieClassId, target.WeenieClassId);

            if (recipe == null)
            {
                var message = new GameMessageSystemChat($"The {source.Name} cannot be used on the {target.Name}.", ChatMessageType.Craft);
                player.Session.Network.EnqueueSend(message);
                player.SendUseDoneEvent();
                return;
            }

            // verify requirements
            if (!VerifyRequirements(recipe.Recipe, player, source, target))
                return;

            if (source.ItemType == ItemType.TinkeringMaterial)
            {
                HandleTinkering(player, source, target);
                return;
            }

            ActionChain craftChain = new ActionChain();
            CreatureSkill skill = null;
            bool skillSuccess = true; // assume success, unless there's a skill check
            double percentSuccess = 1;

            var motion = new Motion(MotionStance.NonCombat, MotionCommand.ClapHands);
            craftChain.AddAction(player, () => player.EnqueueBroadcastMotion(motion));
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(player.MotionTableId);
            var craftAnimationLength = motionTable.GetAnimationLength(MotionCommand.ClapHands);
            craftChain.AddDelaySeconds(craftAnimationLength);

            craftChain.AddAction(player, () =>
            {
                if (recipe.Recipe.Skill > 0 && recipe.Recipe.Difficulty > 0)
                {
                    // there's a skill associated with this
                    Skill skillId = (Skill)recipe.Recipe.Skill;

                    // this shouldn't happen, but sanity check for unexpected nulls
                    skill = player.GetCreatureSkill(skillId);

                    if (skill == null)
                    {
                        log.Warn("Unexpectedly missing skill in Recipe usage");
                        player.SendUseDoneEvent();
                        return;
                    }

                    //Console.WriteLine("Skill difficulty: " + recipe.Recipe.Difficulty);

                    percentSuccess = skill.GetPercentSuccess(recipe.Recipe.Difficulty); //FIXME: Pretty certain this is broken
                }

                if (skill != null)
                {
                    // check for pre-MoA skill
                    // convert into appropriate post-MoA skill
                    // pre-MoA melee weapons: get highest melee weapons skill
                    var newSkill = player.ConvertToMoASkill(skill.Skill);
                    skill = player.GetCreatureSkill(newSkill);

                    //Console.WriteLine("Required skill: " + skill.Skill);

                    if (skill.AdvancementClass <= SkillAdvancementClass.Untrained)
                    {
                        var message = new GameEventWeenieError(player.Session, WeenieError.YouAreNotTrainedInThatTradeSkill);
                        player.Session.Network.EnqueueSend(message);
                        player.SendUseDoneEvent(WeenieError.YouAreNotTrainedInThatTradeSkill);
                        return;
                    }
                }

                // straight skill check, if applicable
                if (skill != null)
                    skillSuccess = _random.NextDouble() < percentSuccess;


                if (skillSuccess)
                {
                    bool destroyTarget = _random.NextDouble() < recipe.Recipe.SuccessDestroyTargetChance;
                    bool destroySource = _random.NextDouble() < recipe.Recipe.SuccessDestroySourceChance;

                    if (destroyTarget)
                    {
                        if (target.OwnerId == player.Guid.Full  || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworkingWithDestroy(target, (ushort)recipe.Recipe.SuccessDestroyTargetAmount);
                        }
                        else if (target.WielderId == player.Guid.Full)
                        {
                            if (!player.TryRemoveItemWithNetworking(target))
                                throw new Exception($"Failed to remove {target.Name} from player inventory.");
                        }
                        else
                        {
                            target.Destroy();
                        }

                        if (!string.IsNullOrEmpty(recipe.Recipe.SuccessDestroyTargetMessage))
                        {
                            var destroyMessage = new GameMessageSystemChat(recipe.Recipe.SuccessDestroyTargetMessage, ChatMessageType.Craft);
                            player.Session.Network.EnqueueSend(destroyMessage);
                        }
                    }

                    if (destroySource)
                    {
                        if (source.OwnerId == player.Guid.Full || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworkingWithDestroy(source, (ushort)recipe.Recipe.SuccessDestroySourceAmount);
                        }
                        else if (source.WielderId == player.Guid.Full)
                        {
                            if (!player.TryRemoveItemWithNetworking(source))
                                throw new Exception($"Failed to remove {source.Name} from player inventory.");
                        }
                        else
                        {
                            source.Destroy();
                        }

                        if (!string.IsNullOrEmpty(recipe.Recipe.SuccessDestroySourceMessage))
                        {
                            var destroyMessage = new GameMessageSystemChat(recipe.Recipe.SuccessDestroySourceMessage, ChatMessageType.Craft);
                            player.Session.Network.EnqueueSend(destroyMessage);
                        }
                    }

                    if (recipe.Recipe.SuccessWCID > 0)
                    {
                        var wo = WorldObjectFactory.CreateNewWorldObject(recipe.Recipe.SuccessWCID);

                        if (wo != null)
                        {
                            if (recipe.Recipe.SuccessAmount > 1)
                                wo.StackSize = (ushort)recipe.Recipe.SuccessAmount;

                            player.TryCreateInInventoryWithNetworking(wo);
                        }
                    }

                    var message = new GameMessageSystemChat(recipe.Recipe.SuccessMessage, ChatMessageType.Craft);
                    player.Session.Network.EnqueueSend(message);
                }
                else
                {
                    bool destroyTarget = _random.NextDouble() < recipe.Recipe.FailDestroyTargetChance;
                    bool destroySource = _random.NextDouble() < recipe.Recipe.FailDestroySourceChance;

                    if (destroyTarget)
                    {
                        if (target.OwnerId == player.Guid.Full || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworkingWithDestroy(target, (ushort)recipe.Recipe.FailDestroyTargetAmount);
                        }
                        else if (target.WielderId == player.Guid.Full)
                        {
                            if (!player.TryRemoveItemWithNetworking(target))
                                throw new Exception($"Failed to remove {target.Name} from player inventory.");
                        }
                        else
                        {
                            target.Destroy();
                        }

                        if (!string.IsNullOrEmpty(recipe.Recipe.FailDestroyTargetMessage))
                        {
                            var destroyMessage = new GameMessageSystemChat(recipe.Recipe.FailDestroyTargetMessage, ChatMessageType.Craft);
                            player.Session.Network.EnqueueSend(destroyMessage);
                        }
                    }

                    if (destroySource)
                    {
                        if (source.OwnerId == player.Guid.Full || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworkingWithDestroy(source, (ushort)recipe.Recipe.FailDestroySourceAmount);
                        }
                        else if (source.WielderId == player.Guid.Full)
                        {
                            if (!player.TryRemoveItemWithNetworking(source))
                                throw new Exception($"Failed to remove {source.Name} from player inventory.");
                        }
                        else
                        {
                            source.Destroy();
                        }

                        if (!string.IsNullOrEmpty(recipe.Recipe.FailDestroySourceMessage))
                        {
                            var destroyMessage = new GameMessageSystemChat(recipe.Recipe.FailDestroySourceMessage, ChatMessageType.Craft);
                            player.Session.Network.EnqueueSend(destroyMessage);
                        }
                    }

                    if (recipe.Recipe.FailWCID > 0)
                    {
                        var wo = WorldObjectFactory.CreateNewWorldObject(recipe.Recipe.FailWCID);

                        if (wo != null)
                        {
                            if (recipe.Recipe.FailAmount > 1)
                                wo.StackSize = (ushort)recipe.Recipe.FailAmount;

                            player.TryCreateInInventoryWithNetworking(wo);
                        }
                    }

                    var message = new GameMessageSystemChat(recipe.Recipe.FailMessage, ChatMessageType.Craft);
                    player.Session.Network.EnqueueSend(message);
                }

                player.SendUseDoneEvent();
            });

            craftChain.EnqueueChain();
        }

        public static float DoCraftMotion(Player player)
        {
            var motion = new Motion(MotionStance.NonCombat, MotionCommand.ClapHands);
            player.EnqueueBroadcastMotion(motion);

            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(player.MotionTableId);
            var craftAnimationLength = motionTable.GetAnimationLength(MotionCommand.ClapHands);
            return craftAnimationLength;
        }

        public static void HandleTinkering(Player player, WorldObject tool, WorldObject target, bool confirmed = false)
        {
            Console.WriteLine($"{player.Name}.HandleTinkering({tool.Name}, {target.Name})");

            // calculate % success chance

            var toolWorkmanship = tool.Workmanship ?? 0;
            var itemWorkmanship = target.Workmanship ?? 0;

            var tinkeredCount = target.NumTimesTinkered;
            var attemptMod = TinkeringDifficulty[tinkeredCount];

            var materialType = tool.MaterialType.Value;
            var salvageMod = GetMaterialMod(materialType);

            var workmanshipMod = 1.0f;
            if (toolWorkmanship >= itemWorkmanship)
                workmanshipMod = 2.0f;

            var recipe = DatabaseManager.World.GetCachedCookbook(tool.WeenieClassId, target.WeenieClassId);
            var recipeSkill = (Skill)recipe.Recipe.Skill;
            Console.WriteLine($"Recipe skill: {recipeSkill}");

            var skill = player.GetCreatureSkill(recipeSkill);

            // thanks to Endy's Tinkering Calculator for this formula!
            var difficulty = (int)Math.Floor(((salvageMod * 5.0f) + (itemWorkmanship * salvageMod * 2.0f) - (toolWorkmanship * workmanshipMod * salvageMod / 5.0f)) * attemptMod);

            var successChance = SkillCheck.GetSkillChance((int)skill.Current, difficulty);

            // imbue: divide success by 3

            // handle rare foolproof material
            //if (tool.WeenieClassId >= 30094 && tool.WeenieClassId <= 30106)
                //successChance = 1.0f;

            // check for player option: 'Use Crafting Chance of Success Dialog'
            if (player.GetCharacterOption(CharacterOption.UseCraftingChanceOfSuccessDialog) && !confirmed)
            {
                var percent = (float)successChance * 100;
                var decimalPlaces = 2;
                var truncated = percent.Truncate(decimalPlaces);

                var templateMsg = $"You have a % chance of using {tool.Name} on {target.Name}.";
                var floorMsg = templateMsg.Replace("%", (int)percent + "%");
                var truncateMsg = templateMsg.Replace("%", Math.Round(truncated, decimalPlaces) + "%");
                var exactMsg = templateMsg.Replace("%", percent + "%");

                var confirm = new Confirmation(ConfirmationType.CraftInteraction, floorMsg, tool, target, player);
                ConfirmationManager.AddConfirmation(confirm);

                player.Session.Network.EnqueueSend(new GameEventConfirmationRequest(player.Session, ConfirmationType.CraftInteraction, confirm.ConfirmationID, floorMsg));
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(exactMsg, ChatMessageType.Craft));

                player.SendUseDoneEvent();
                return;
            }

            var animLength = DoCraftMotion(player);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(player, () => DoTinkering(player, tool, target, (float)successChance));
            actionChain.EnqueueChain();
        }

        public static void DoTinkering(Player player, WorldObject tool, WorldObject target, float successChance)
        {
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            if (rng <= successChance)
            {
                // tinkering success!
                Tinkering_ModifyItem(player, tool, target);

                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You successfully apply the {tool.Name} to the {target.Name}.", ChatMessageType.Craft));
            }
            else
            {
                // tinkering failure

                // send message
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.UnableToMakeCraftReq));
            }

            if (!player.GetCharacterOption(CharacterOption.UseCraftingChanceOfSuccessDialog))
                player.SendUseDoneEvent();
        }

        public static void Tinkering_ModifyItem(Player player, WorldObject tool, WorldObject target)
        {
            var recipe = DatabaseManager.World.GetCachedCookbook(tool.WeenieClassId, target.WeenieClassId);

            var materialType = tool.MaterialType.Value;

            switch (materialType)
            {
                case MaterialType.Steel:
                    target.ArmorLevel += 20;
                    break;
                case MaterialType.ArmoredilloHide:
                    target.ArmorModVsAcid = Math.Min((target.ArmorModVsAcid ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.Marble:
                    target.ArmorModVsBludgeon = Math.Min((target.ArmorModVsBludgeon ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.Wool:
                    target.ArmorModVsCold = Math.Min((target.ArmorModVsCold ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.ReedSharkHide:
                    target.ArmorModVsElectric = Math.Min((target.ArmorModVsElectric ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.Ceramic:
                    target.ArmorModVsFire = Math.Min((target.ArmorModVsFire ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.Alabaster:
                    target.ArmorModVsPierce = Math.Min((target.ArmorModVsPierce ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.Bronze:
                    target.ArmorModVsSlash = Math.Min((target.ArmorModVsSlash ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.Linen:
                    target.EncumbranceVal = (int)Math.Round((target.EncumbranceVal ?? 1) * 0.85f);
                    break;
                case MaterialType.Iron:
                    target.Damage += 1;
                    break;
                case MaterialType.Mahogany:
                    target.DamageMod += 0.04f;
                    break;
                case MaterialType.Granite:
                    target.DamageVariance *= 0.8f;
                    break;
                case MaterialType.Oak:
                    target.WeaponTime = Math.Max(0, (target.WeaponTime ?? 0) - 50);
                    break;
                case MaterialType.Pine:
                    target.Value *= (int)Math.Round((target.Value ?? 1) * 0.75f);
                    break;
                case MaterialType.Gold:
                    target.Value *= (int)Math.Round((target.Value ?? 1) * 1.25f);
                    break;
                case MaterialType.Brass:
                    target.WeaponDefense += 0.01f;
                    break;
                case MaterialType.Velvet:
                    target.WeaponOffense += 0.01f;
                    break;
                case MaterialType.BlackOpal:
                    AddImbuedEffect(target, ImbuedEffectType.CriticalStrike);
                    break;
                case MaterialType.FireOpal:
                    AddImbuedEffect(target, ImbuedEffectType.CripplingBlow);
                    break;
                case MaterialType.Sunstone:
                    AddImbuedEffect(target, ImbuedEffectType.ArmorRending);
                    break;
                case MaterialType.Opal:
                    target.ManaConversionMod += 0.01f;
                    break;
                case MaterialType.Moonstone:
                    target.ItemMaxMana += 500;
                    break;
                case MaterialType.Silver:

                    if (target.WieldSkillType != Skill.MeleeDefense)
                        return;
                    // change wield requirement: melee defense -> missile defense (reduced)
                    target.WieldSkillType = Skill.MissileDefense;
                    if (target.ItemSkillLevelLimit.HasValue)
                        target.ItemSkillLevelLimit = (int)Math.Round(target.ItemSkillLevelLimit.Value * 0.85f);
                    break;

                case MaterialType.Copper:

                    if (target.WieldSkillType != Skill.MissileDefense)
                        return;
                    // change wield requirement: missile defense -> melee defense (increased)
                    target.WieldSkillType = Skill.MeleeDefense;
                    if (target.ItemSkillLevelLimit.HasValue)
                        target.ItemSkillLevelLimit = (int)Math.Round(target.ItemSkillLevelLimit.Value * 1.2f);
                    break;

                case MaterialType.Silk:

                    // remove allegiance rank limit, increase item difficulty by spellcraft?
                    target.ItemAllegianceRankLimit = null;
                    target.ItemDifficulty = (target.ItemDifficulty ?? 0) + target.ItemSpellcraft;
                    break;

                case MaterialType.Zircon:
                    AddImbuedEffect(target, ImbuedEffectType.MagicDefense);
                    break;
                case MaterialType.Peridot:
                    AddImbuedEffect(target, ImbuedEffectType.MeleeDefense);
                    break;
                case MaterialType.YellowTopaz:
                    AddImbuedEffect(target, ImbuedEffectType.MissileDefense);
                    break;
                case MaterialType.Emerald:
                    AddImbuedEffect(target, ImbuedEffectType.AcidRending);
                    break;
                case MaterialType.WhiteSapphire:
                    AddImbuedEffect(target, ImbuedEffectType.BludgeonRending);
                    break;
                case MaterialType.Aquamarine:
                    AddImbuedEffect(target, ImbuedEffectType.ColdRending);
                    break;
                case MaterialType.Jet:
                    AddImbuedEffect(target, ImbuedEffectType.ElectricRending);
                    break;
                case MaterialType.RedGarnet:
                    AddImbuedEffect(target, ImbuedEffectType.FireRending);
                    break;
                case MaterialType.BlackGarnet:
                    AddImbuedEffect(target, ImbuedEffectType.PierceRending);
                    break;
                case MaterialType.ImperialTopaz:
                    AddImbuedEffect(target, ImbuedEffectType.SlashRending);
                    break;
                case MaterialType.Azurite:
                case MaterialType.Malachite:
                case MaterialType.Citrine:
                case MaterialType.Hematite:
                case MaterialType.LavenderJade:
                case MaterialType.RedJade:
                case MaterialType.Carnelian:
                case MaterialType.LapisLazuli:
                case MaterialType.Agate:
                case MaterialType.RoseQuartz:
                case MaterialType.SmokeyQuartz:
                case MaterialType.Bloodstone:
                    return;  // ??
                case MaterialType.Ebony:
                    target.HeritageGroup = HeritageGroup.Gharundim;
                    break;
                case MaterialType.Porcelain:
                    target.HeritageGroup = HeritageGroup.Sho;
                    break;
                case MaterialType.Teak:
                    target.HeritageGroup = HeritageGroup.Aluvian;
                    break;
                case MaterialType.Leather:
                    target.Retained = true;
                    break;
                case MaterialType.GreenGarnet:
                    target.ElementalDamageMod += 0.01f;
                    break;
                //case MaterialType.DarkIdol:
                //AddImbuedEffect(target, ImbuedEffectType.IgnoreSomeMagicProjectileDamage);
                //break;
                default:
                    Console.WriteLine($"Unknown material type: {materialType}");
                    return;
            }
            // increase # of times tinkered
            target.NumTimesTinkered++;
        }

        public static bool AddImbuedEffect(WorldObject target, ImbuedEffectType effect)
        {
            var imbuedEffects = GetImbuedEffects(target);

            if (imbuedEffects.HasFlag(effect))
                return false;     // already present

            imbuedEffects |= effect;

            if (target.GetProperty(PropertyInt.ImbuedEffect) == null)
                target.SetProperty(PropertyInt.ImbuedEffect, (int)effect);

            else if (target.GetProperty(PropertyInt.ImbuedEffect2) == null)
                target.SetProperty(PropertyInt.ImbuedEffect2, (int)effect);

            else if (target.GetProperty(PropertyInt.ImbuedEffect3) == null)
                target.SetProperty(PropertyInt.ImbuedEffect3, (int)effect);

            else if (target.GetProperty(PropertyInt.ImbuedEffect4) == null)
                target.SetProperty(PropertyInt.ImbuedEffect4, (int)effect);

            else if (target.GetProperty(PropertyInt.ImbuedEffect5) == null)
                target.SetProperty(PropertyInt.ImbuedEffect5, (int)effect);

            else
                return false;

            return true;
        }

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

        // todo: verify
        public enum CompareType
        {
            GreaterThan,        // 0
            LessThanEqual,      // 1
            LessThan,           // 2
            GreaterThanEqual,   // 3
            NotEqual,           // 4
            NotEqualNotExist,   // 5
            Equal,              // 6
            NotExist,           // 7
            Exist               // 8
        };

        public static bool VerifyRequirements(Recipe recipe, Player player, WorldObject source, WorldObject target)
        {
            // as opposed to having a recipe requirements field for the object being compared...
            if (!VerifyRequirements(recipe, player, player)) return false;

            if (!VerifyRequirements(recipe, player, source)) return false;

            if (!VerifyRequirements(recipe, player, target)) return false;

            return true;
        }

        public static bool VerifyRequirements(Recipe recipe, Player player, WorldObject obj)
        {
            foreach (var requirement in recipe.RecipeRequirementsBool)
            {
                bool? value = obj.GetProperty((PropertyBool)requirement.Stat);
                double? normalized = value != null ? (double?)Convert.ToDouble(value.Value) : null;

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, normalized, Convert.ToDouble(requirement.Value), requirement.Message))
                    return false;
            }

            foreach (var requirement in recipe.RecipeRequirementsInt)
            {
                int? value = obj.GetProperty((PropertyInt)requirement.Stat);
                double? normalized = value != null ? (double?)Convert.ToDouble(value.Value) : null;

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, normalized, Convert.ToDouble(requirement.Value), requirement.Message))
                    return false;
            }

            foreach (var requirement in recipe.RecipeRequirementsFloat)
            {
                double? value = obj.GetProperty((PropertyFloat)requirement.Stat);

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, value, requirement.Value, requirement.Message))
                    return false;
            }

            foreach (var requirement in recipe.RecipeRequirementsString)
            {
                string value = obj.GetProperty((PropertyString)requirement.Stat);

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, value, requirement.Value, requirement.Message))
                    return false;
            }

            foreach (var requirement in recipe.RecipeRequirementsIID)
            {
                uint? value = obj.GetProperty((PropertyInstanceId)requirement.Stat);
                double? normalized = value != null ? (double?)Convert.ToDouble(value.Value) : null;

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, normalized, Convert.ToDouble(requirement.Value), requirement.Message))
                    return false;
            }

            foreach (var requirement in recipe.RecipeRequirementsDID)
            {
                uint? value = obj.GetProperty((PropertyDataId)requirement.Stat);
                double? normalized = value != null ? (double?)Convert.ToDouble(value.Value) : null;

                if (!VerifyRequirement(player, (CompareType)requirement.Enum, normalized, Convert.ToDouble(requirement.Value), requirement.Message))
                    return false;
            }
            return true;
        }

        public static bool VerifyRequirement(Player player, CompareType compareType, double? prop, double val, string failMsg)
        {
            var success = true;

            switch (compareType)
            {
                case CompareType.GreaterThan:
                    if (prop != null && prop.Value > val)
                        success = false;
                    break;

                case CompareType.LessThanEqual:
                    if (prop != null && prop.Value <= val)
                        success = false;
                    break;

                case CompareType.LessThan:
                    if (prop != null && prop.Value < val)
                        success = false;
                    break;

                case CompareType.GreaterThanEqual:
                    if (prop != null && prop.Value >= val)
                        success = false;
                    break;

                case CompareType.NotEqual:
                    if (prop != null && prop.Value != val)
                        success = false;
                    break;

                case CompareType.NotEqualNotExist:
                    if (prop == null || prop.Value != val)
                        success = false;
                    break;

                case CompareType.Equal:
                    if (prop != null && prop.Value == val)
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

        public static bool VerifyRequirement(Player player, CompareType compareType, string prop, string val, string failMsg)
        {
            var success = true;

            switch (compareType)
            {
                case CompareType.NotEqual:
                    if (prop != null && !prop.Equals(val))
                        success = false;
                    break;

                case CompareType.NotEqualNotExist:
                    if (prop == null || !prop.Equals(val))
                        success = false;
                    break;

                case CompareType.Equal:
                    if (prop != null && prop.Equals(val))
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
    }
}
