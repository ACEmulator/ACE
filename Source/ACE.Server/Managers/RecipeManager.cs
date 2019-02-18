using System;
using System.Collections.Generic;
using log4net;

using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
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
            {
                player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            if (source.ItemType == ItemType.TinkeringMaterial)
            {
                HandleTinkering(player, source, target);
                return;
            }

            ActionChain craftChain = new ActionChain();
            CreatureSkill skill = null;
            bool success = true; // assume success, unless there's a skill check
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

                // perform skill check, if applicable
                if (skill != null)
                    success = ThreadSafeRandom.Next(0.0f, 1.0f) <= percentSuccess;

                CreateDestroyItems(player, recipe.Recipe, source, target, success);

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
            var skill = player.GetCreatureSkill(recipeSkill);

            // thanks to Endy's Tinkering Calculator for this formula!
            var difficulty = (int)Math.Floor(((salvageMod * 5.0f) + (itemWorkmanship * salvageMod * 2.0f) - (toolWorkmanship * workmanshipMod * salvageMod / 5.0f)) * attemptMod);

            var successChance = SkillCheck.GetSkillChance((int)skill.Current, difficulty);

            // imbue: divide success by 3
            if (recipe.Recipe.SalvageType == 2)
            {
                successChance /= 3.0f;

                if (player.AugmentationBonusImbueChance > 0)
                    successChance += player.AugmentationBonusImbueChance * 0.05f;
            }

            // handle rare foolproof material
            if (tool.WeenieClassId >= 30094 && tool.WeenieClassId <= 30106)
                successChance = 1.0f;

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

        public static void DoTinkering(Player player, WorldObject tool, WorldObject target, float chance)
        {
            var success = ThreadSafeRandom.Next(0.0f, 1.0f) <= chance;

            if (success)
                Tinkering_ModifyItem(player, tool, target);

            var recipe = DatabaseManager.World.GetCachedCookbook(tool.WeenieClassId, target.WeenieClassId);
            CreateDestroyItems(player, recipe.Recipe, tool, target, success);

            if (!player.GetCharacterOption(CharacterOption.UseCraftingChanceOfSuccessDialog))
                player.SendUseDoneEvent();
        }

        public static void Tinkering_ModifyItem(Player player, WorldObject tool, WorldObject target)
        {
            var recipe = DatabaseManager.World.GetCachedCookbook(tool.WeenieClassId, target.WeenieClassId);

            var materialType = tool.MaterialType.Value;

            switch (materialType)
            {
                // armor tinkering
                case MaterialType.Steel:
                    target.ArmorLevel += 20;
                    break;
                case MaterialType.Alabaster:
                    target.ArmorModVsPierce = Math.Min((target.ArmorModVsPierce ?? 0) + 0.2f, 2.0f);
                    break;
                case MaterialType.Bronze:
                    target.ArmorModVsSlash = Math.Min((target.ArmorModVsSlash ?? 0) + 0.2f, 2.0f);
                    break;
                case MaterialType.Marble:
                    target.ArmorModVsBludgeon = Math.Min((target.ArmorModVsBludgeon ?? 0) + 0.2f, 2.0f);
                    break;
                case MaterialType.ArmoredilloHide:
                    target.ArmorModVsAcid = Math.Min((target.ArmorModVsAcid ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.Ceramic:
                    target.ArmorModVsFire = Math.Min((target.ArmorModVsFire ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.Wool:
                    target.ArmorModVsCold = Math.Min((target.ArmorModVsCold ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.ReedSharkHide:
                    target.ArmorModVsElectric = Math.Min((target.ArmorModVsElectric ?? 0) + 0.4f, 2.0f);
                    break;
                case MaterialType.Peridot:
                    AddImbuedEffect(player, target, ImbuedEffectType.MeleeDefense);
                    break;
                case MaterialType.YellowTopaz:
                    AddImbuedEffect(player, target, ImbuedEffectType.MissileDefense);
                    break;
                case MaterialType.Zircon:
                    AddImbuedEffect(player, target, ImbuedEffectType.MagicDefense);
                    break;

                // item tinkering
                case MaterialType.Pine:
                    target.Value *= (int)Math.Round((target.Value ?? 1) * 0.75f);
                    break;
                case MaterialType.Gold:
                    target.Value *= (int)Math.Round((target.Value ?? 1) * 1.25f);
                    break;
                case MaterialType.Linen:
                    target.EncumbranceVal = (int)Math.Round((target.EncumbranceVal ?? 1) * 0.75f);
                    break;
                case MaterialType.Ivory:
                    target.SetProperty(PropertyInt.Attuned, 0);
                    break;
                case MaterialType.Leather:
                    target.SetProperty(PropertyBool.Retained, true);
                    break;
                case MaterialType.Moonstone:
                    target.ItemMaxMana += 500;
                    break;
                case MaterialType.Teak:
                    target.HeritageGroup = HeritageGroup.Aluvian;
                    break;
                case MaterialType.Ebony:
                    target.HeritageGroup = HeritageGroup.Gharundim;
                    break;
                case MaterialType.Porcelain:
                    target.HeritageGroup = HeritageGroup.Sho;
                    break;
                case MaterialType.Satin:
                    target.HeritageGroup = HeritageGroup.Viamontian;
                    break;
                case MaterialType.Copper:

                    if ((target.WieldSkillType ?? 0) != (int)Skill.MissileDefense)
                        return;
                    // change wield requirement: missile defense -> melee defense (increased)
                    target.WieldSkillType = (int)Skill.MeleeDefense;
                    if (target.ItemSkillLevelLimit.HasValue)
                        target.ItemSkillLevelLimit = (int)Math.Round(target.ItemSkillLevelLimit.Value * 1.2f);
                    break;

                case MaterialType.Silver:

                    if ((target.WieldSkillType ?? 0) != (int)Skill.MeleeDefense)
                        return;
                    // change wield requirement: melee defense -> missile defense (reduced)
                    target.WieldSkillType = (int)Skill.MissileDefense;
                    if (target.ItemSkillLevelLimit.HasValue)
                        target.ItemSkillLevelLimit = (int)Math.Round(target.ItemSkillLevelLimit.Value * 0.85f);
                    break;

                case MaterialType.Silk:

                    // remove allegiance rank limit, increase item difficulty by spellcraft?
                    target.ItemAllegianceRankLimit = null;
                    target.ItemDifficulty = (target.ItemDifficulty ?? 0) + target.ItemSpellcraft;
                    break;

                case MaterialType.Amber:
                case MaterialType.Diamond:
                case MaterialType.GromnieHide:
                case MaterialType.Pyreal:
                case MaterialType.Ruby:
                case MaterialType.Sapphire:
                    return;

                // magic item tinkering

                case MaterialType.Sunstone:
                    AddImbuedEffect(player, target, ImbuedEffectType.ArmorRending);
                    break;
                case MaterialType.FireOpal:
                    AddImbuedEffect(player, target, ImbuedEffectType.CripplingBlow);
                    break;
                case MaterialType.BlackOpal:
                    AddImbuedEffect(player, target, ImbuedEffectType.CriticalStrike);
                    break;
                case MaterialType.Opal:
                    target.ManaConversionMod += 0.01f;
                    break;
                case MaterialType.GreenGarnet:
                    target.ElementalDamageMod += 0.01f;     // + 1% vs. monsters, + 0.25% vs. players
                    break;

                case MaterialType.SmokeyQuartz:
                    AddSpell(player, target, SpellId.CANTRIPCOORDINATION1);
                    break;
                case MaterialType.RoseQuartz:
                    AddSpell(player, target, SpellId.CANTRIPQUICKNESS1);
                    break;
                case MaterialType.RedJade:
                    AddSpell(player, target, SpellId.CANTRIPHEALTHGAIN1);
                    break;
                case MaterialType.Malachite:
                    AddSpell(player, target, SpellId.WarriorsVigor);
                    break;
                case MaterialType.LavenderJade:
                    AddSpell(player, target, SpellId.CANTRIPMANAGAIN1);
                    break;
                case MaterialType.LapisLazuli:
                    AddSpell(player, target, SpellId.CANTRIPWILLPOWER1);
                    break;
                case MaterialType.Hematite:
                    AddSpell(player, target, SpellId.WarriorsVitality);
                    break;
                case MaterialType.Citrine:
                    AddSpell(player, target, SpellId.CANTRIPSTAMINAGAIN1);
                    break;
                case MaterialType.Carnelian:
                    AddSpell(player, target, SpellId.CANTRIPSTRENGTH1);
                    break;
                case MaterialType.Bloodstone:
                    AddSpell(player, target, SpellId.CANTRIPENDURANCE1);
                    break;
                case MaterialType.Azurite:
                    AddSpell(player, target, SpellId.WizardsIntellect);
                    break;
                case MaterialType.Agate:
                    AddSpell(player, target, SpellId.CANTRIPFOCUS1);
                    break;

                // weapon tinkering

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
                case MaterialType.Brass:
                    target.WeaponDefense += 0.01f;
                    break;
                case MaterialType.Velvet:
                    target.WeaponOffense += 0.01f;
                    break;

                // only 1 imbue can be applied per piece of armor?
                case MaterialType.Emerald:
                    AddImbuedEffect(player, target, ImbuedEffectType.AcidRending);
                    break;
                case MaterialType.WhiteSapphire:
                    AddImbuedEffect(player, target, ImbuedEffectType.BludgeonRending);
                    break;
                case MaterialType.Aquamarine:
                    AddImbuedEffect(player, target, ImbuedEffectType.ColdRending);
                    break;
                case MaterialType.Jet:
                    AddImbuedEffect(player, target, ImbuedEffectType.ElectricRending);
                    break;
                case MaterialType.RedGarnet:
                    AddImbuedEffect(player, target, ImbuedEffectType.FireRending);
                    break;
                case MaterialType.BlackGarnet:
                    AddImbuedEffect(player, target, ImbuedEffectType.PierceRending);
                    break;
                case MaterialType.ImperialTopaz:
                    AddImbuedEffect(player, target, ImbuedEffectType.SlashRending);
                    break;
                default:
                    Console.WriteLine($"Unknown material type: {materialType}");
                    return;
            }
            // increase # of times tinkered
            target.NumTimesTinkered++;
        }

        public static void AddSpell(Player player, WorldObject target, SpellId spell, int difficulty = 25)
        {
            target.Biota.GetOrAddKnownSpell((int)spell, target.BiotaDatabaseLock, out var added);
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

        public static bool AddImbuedEffect(Player player, WorldObject target, ImbuedEffectType effect)
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

            if (IconUnderlay.TryGetValue(effect, out var icon))
            {
                target.SetProperty(PropertyDataId.IconUnderlay, icon);
                player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyDataID(target, PropertyDataId.IconUnderlay, icon));
            }

            return true;
        }

        // derrick's input => output mappings
        public static Dictionary<ImbuedEffectType, uint> IconUnderlay = new Dictionary<ImbuedEffectType, uint>()
        {
            { ImbuedEffectType.ColdRending,     0x06003353 },
            { ImbuedEffectType.ElectricRending, 0x06003354 },
            { ImbuedEffectType.AcidRending,     0x06003355 },
            { ImbuedEffectType.ArmorRending,    0x06003356 },
            { ImbuedEffectType.CriticalStrike,  0x06003357 },
            { ImbuedEffectType.CripplingBlow,   0x06003357 },
            { ImbuedEffectType.FireRending,     0x06003359 },
            { ImbuedEffectType.BludgeonRending, 0x0600335a },
            { ImbuedEffectType.SlashRending,    0x0600335b },
            { ImbuedEffectType.PierceRending,   0x0600335c },
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

        public static void CreateDestroyItems(Player player, Recipe recipe, WorldObject source, WorldObject target, bool success)
        {
            var destroyTargetChance = success ? recipe.SuccessDestroyTargetChance : recipe.FailDestroyTargetChance;
            var destroySourceChance = success ? recipe.SuccessDestroySourceChance : recipe.FailDestroySourceChance;

            var destroyTarget = ThreadSafeRandom.Next(0.0f, 1.0f) <= destroyTargetChance;
            var destroySource = ThreadSafeRandom.Next(0.0f, 1.0f) <= destroySourceChance;

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

            var createItem = success ? recipe.SuccessWCID : recipe.FailWCID;
            var createAmount = success ? recipe.SuccessAmount : recipe.FailAmount;

            WorldObject result = null;

            if (createItem > 0)
                result = CreateItem(player, createItem, createAmount);

            ModifyItem(player, recipe, source, target, result, success);

            var message = success ? recipe.SuccessMessage : recipe.FailMessage;

            player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Craft));
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

        public enum ModifyOp
        {
            None,       // 0
            SetValue,   // 1
            Add,        // 2
            CopyTarget, // 3
            CopyCreate, // 4
            Unknown1,   // 5
            Unknown2,   // 6
            AddSpell    // 7
        }

        public static void ModifyItem(Player player, Recipe recipe, WorldObject source, WorldObject target, WorldObject result, bool success)
        {
            foreach (var mod in recipe.RecipeMod)
            {
                if (mod.ExecutesOnSuccess != success)
                    continue;

                // apply base mod

                // apply type mods
                foreach (var boolMod in mod.RecipeModsBool)
                    ModifyBool(boolMod, source, target);

                foreach (var intMod in mod.RecipeModsInt)
                    ModifyInt(player, intMod, source, target, result);

                foreach (var floatMod in mod.RecipeModsFloat)
                    ModifyFloat(player, floatMod, source, target, result);

                foreach (var stringMod in mod.RecipeModsString)
                    ModifyString(player, stringMod, source, target, result);

                foreach (var iidMod in mod.RecipeModsIID)
                    ModifyInstanceID(player, iidMod, source, target, result);

                foreach (var didMod in mod.RecipeModsDID)
                    ModifyDataID(player, didMod, source, target, result);
            }
        }

        public static void ModifyBool(RecipeModsBool boolMod, WorldObject source, WorldObject target)
        {
            var op = (ModifyOp)boolMod.Enum;
            var prop = (PropertyBool)boolMod.Stat;
            var value = boolMod.Value;

            // always SetValue?
            if (op != ModifyOp.SetValue)
            {
                log.Warn($"RecipeManager.ModifyBool({source.Name}, {target.Name}): unhandled operation {op}");
                return;
            }
            target.SetProperty(prop, value);
        }

        public enum SourceType
        {
            Player  = 0,
            Source  = 1,
            Dye     = 60
        };

        public static WorldObject GetSourceMod(SourceType sourceType, Player player, WorldObject source)
        {
            switch (sourceType)
            {
                case SourceType.Player:
                    return player;
                case SourceType.Source:
                    return source;
            }
            log.Warn($"RecipeManager.GetSourceMod({sourceType}, {player.Name}, {source.Name}) - unknown source type");
            return null;
        }

        public static void ModifyInt(Player player, RecipeModsInt intMod, WorldObject source, WorldObject target, WorldObject result)
        {
            var op = (ModifyOp)intMod.Enum;
            var prop = (PropertyInt)intMod.Stat;
            var value = intMod.Value;

            var sourceMod = GetSourceMod((SourceType)intMod.Source, player, source);

            switch (op)
            {
                case ModifyOp.SetValue:
                    target.SetProperty(prop, value);
                    break;
                case ModifyOp.Add:
                    target.IncProperty(prop, value);
                    break;
                case ModifyOp.CopyTarget:
                    target.SetProperty(prop, sourceMod.GetProperty(prop) ?? 0);
                    break;
                case ModifyOp.CopyCreate:
                    result.SetProperty(prop, sourceMod.GetProperty(prop) ?? 0);
                    break;
                case ModifyOp.AddSpell:
                    if (value != -1)
                    {
                        target.Biota.GetOrAddKnownSpell(value, target.BiotaDatabaseLock, out var added);
                        target.ChangesDetected = true;
                    }
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyInt({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyFloat(Player player, RecipeModsFloat floatMod, WorldObject source, WorldObject target, WorldObject result)
        {
            var op = (ModifyOp)floatMod.Enum;
            var prop = (PropertyFloat)floatMod.Stat;
            var value = floatMod.Value;

            var sourceMod = GetSourceMod((SourceType)floatMod.Source, player, source);

            switch (op)
            {
                case ModifyOp.SetValue:
                    target.SetProperty(prop, value);
                    break;
                case ModifyOp.Add:
                    target.IncProperty(prop, value);
                    break;
                case ModifyOp.CopyTarget:
                    target.SetProperty(prop, sourceMod.GetProperty(prop) ?? 0);
                    break;
                case ModifyOp.CopyCreate:
                    result.SetProperty(prop, sourceMod.GetProperty(prop) ?? 0);
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyFloat({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyString(Player player, RecipeModsString stringMod, WorldObject source, WorldObject target, WorldObject result)
        {
            var op = (ModifyOp)stringMod.Enum;
            var prop = (PropertyString)stringMod.Stat;
            var value = stringMod.Value;

            var sourceMod = GetSourceMod((SourceType)stringMod.Source, player, source);

            switch (op)
            {
                case ModifyOp.SetValue:
                    target.SetProperty(prop, value);
                    break;
                case ModifyOp.CopyTarget:
                    target.SetProperty(prop, sourceMod.GetProperty(prop) ?? sourceMod.Name);
                    break;
                case ModifyOp.CopyCreate:
                    result.SetProperty(prop, sourceMod.GetProperty(prop) ?? sourceMod.Name);
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyString({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyInstanceID(Player player, RecipeModsIID iidMod, WorldObject source, WorldObject target, WorldObject result)
        {
            var op = (ModifyOp)iidMod.Enum;
            var prop = (PropertyInstanceId)iidMod.Stat;
            var value = iidMod.Value;

            var sourceMod = GetSourceMod((SourceType)iidMod.Source, player, source);

            switch (op)
            {
                case ModifyOp.SetValue:
                    target.SetProperty(prop, value);
                    break;
                case ModifyOp.CopyTarget:
                    target.SetProperty(prop, sourceMod.GetProperty(prop) ?? 0);
                    break;
                case ModifyOp.CopyCreate:
                    result.SetProperty(prop, sourceMod.GetProperty(prop) ?? 0);
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyInstanceID({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyDataID(Player player, RecipeModsDID didMod, WorldObject source, WorldObject target, WorldObject result)
        {
            var op = (ModifyOp)didMod.Enum;
            var prop = (PropertyDataId)didMod.Stat;
            var value = didMod.Value;

            var sourceMod = GetSourceMod((SourceType)didMod.Source, player, source);

            switch (op)
            {
                case ModifyOp.SetValue:
                    target.SetProperty(prop, value);
                    break;
                case ModifyOp.CopyTarget:
                    target.SetProperty(prop, sourceMod.GetProperty(prop) ?? 0);
                    break;
                case ModifyOp.CopyCreate:
                    result.SetProperty(prop, sourceMod.GetProperty(prop) ?? 0);
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyDataID({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }
    }
}
