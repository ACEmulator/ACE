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
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

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

            if (source.ItemType == ItemType.TinkeringMaterial)
            {
                HandleTinkering(player, source, target);
                return;
            }

            var percentSuccess = GetRecipeChance(player, source, target, recipe);

            if (percentSuccess == null)
            {
                player.SendUseDoneEvent();
                return;
            }

            var showDialog = HasDifficulty(recipe) && player.GetCharacterOption(CharacterOption.UseCraftingChanceOfSuccessDialog);

            if (showDialog && !confirmed)
            {
                ShowDialog(player, source, target, (float)percentSuccess);
                return;
            }

            ActionChain craftChain = new ActionChain();

            var animTime = 0.0f;

            player.IsBusy = true;

            if (player.CombatMode != CombatMode.NonCombat)
            {
                var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                craftChain.AddDelaySeconds(stanceTime);

                animTime += stanceTime;
            }

            animTime += player.EnqueueMotion(craftChain, MotionCommand.ClapHands);

            craftChain.AddAction(player, () => HandleRecipe(player, source, target, recipe, (float)percentSuccess));

            player.EnqueueMotion(craftChain, MotionCommand.Ready);

            craftChain.AddAction(player, () =>
            {
                if (!showDialog)
                    player.SendUseDoneEvent();

                player.IsBusy = false;
            });

            craftChain.EnqueueChain();

            player.NextUseTime = DateTime.UtcNow.AddSeconds(animTime);
        }

        public static bool HasDifficulty(Recipe recipe)
        {
            return recipe.Skill > 0 && recipe.Difficulty > 0;
        }

        public static float? GetRecipeChance(Player player, WorldObject source, WorldObject target, Recipe recipe)
        {
            // only for regular recipes atm, tinkering / imbues handled separately
            // todo: refactor this more
            if (!HasDifficulty(recipe))
                return 1.0f;

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

            var chance = (float)SkillCheck.GetSkillChance(playerSkill.Current, recipe.Difficulty);

            return chance;
        }

        public static void HandleRecipe(Player player, WorldObject source, WorldObject target, Recipe recipe, float successChance)
        {
            // re-verify
            if (!VerifyRequirements(recipe, player, source, target))
            {
                player.SendWeenieError(WeenieError.YouDoNotPassCraftingRequirements);
                return;
            }

            var success = ThreadSafeRandom.Next(0.0f, 1.0f) <= successChance;

            CreateDestroyItems(player, recipe, source, target, success);

            // this code was intended for dyes, but UpdateObj seems to remove crafting components
            // from shortcut bar, if they are hotkeyed
            // more specifity for this, only if relevant properties are modified?
            var shortcuts = player.GetShortcuts();

            if (!shortcuts.Select(i => i.ObjectId).Contains(target.Guid.Full))
            {
                var updateObj = new GameMessageUpdateObject(target);
                var updateDesc = new GameMessageObjDescEvent(player);

                if (target.CurrentWieldedLocation != null)
                    player.EnqueueBroadcast(updateObj, updateDesc);
                else
                    player.Session.Network.EnqueueSend(updateObj);
            }
        }

        public static float DoMotion(Player player, MotionCommand motionCommand)
        {
            var motion = new Motion(MotionStance.NonCombat, motionCommand);
            player.EnqueueBroadcastMotion(motion);

            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(player.MotionTableId);
            var craftAnimationLength = motionTable.GetAnimationLength(motionCommand);
            return craftAnimationLength;
        }

        public static void ShowDialog(Player player, WorldObject source, WorldObject target, float successChance, bool tinkering = false, int numAugs = 0)
        {
            var percent = successChance * 100;

            // retail messages:

            // You determine that you have a 100 percent chance to succeed.
            // You determine that you have a 99 percent chance to succeed.
            // You determine that you have a 38 percent chance to succeed. 5 percent is due to your augmentation.

            var floorMsg = $"You determine that you have a {percent.Round()}% chance to succeed.";
            if (numAugs > 0)
                floorMsg += $"\n{numAugs * 5}% is due to your augmentation.";

            player.ConfirmationManager.EnqueueSend(new Confirmation_CraftInteration(player.Guid, source.Guid, target.Guid, tinkering), floorMsg);

            if (PropertyManager.GetBool("craft_exact_msg").Item)
            {
                var exactMsg = $"You have a {percent}% chance of using {source.NameWithMaterial} on {target.NameWithMaterial}.";

                player.Session.Network.EnqueueSend(new GameMessageSystemChat(exactMsg, ChatMessageType.Craft));
            }
            player.SendUseDoneEvent();
        }

        public static void HandleTinkering(Player player, WorldObject tool, WorldObject target, bool confirmed = false)
        {
            double successChance;
            bool incItemTinkered = true;

            Console.WriteLine($"{player.Name}.HandleTinkering({tool.NameWithMaterial}, {target.NameWithMaterial})");

            // calculate % success chance

            var toolWorkmanship = tool.Workmanship ?? 0;
            var itemWorkmanship = target.Workmanship ?? 0;

            var tinkeredCount = target.NumTimesTinkered;

            var materialType = tool.MaterialType ?? MaterialType.Unknown;
            var salvageMod = GetMaterialMod(materialType);

            var workmanshipMod = 1.0f;
            if (toolWorkmanship >= itemWorkmanship)
                workmanshipMod = 2.0f;

            var recipe = GetRecipe(player, tool, target);
            var recipeSkill = (Skill)recipe.Skill;
            var skill = player.GetCreatureSkill(recipeSkill);

            // require skill check for everything except ivory / leather / sandstone
            if (UseSkillCheck(materialType))
            {
                // tinkering skill must be trained
                if (skill.AdvancementClass < SkillAdvancementClass.Trained)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You are not trained in {skill.Skill.ToSentence()}.", ChatMessageType.Broadcast));
                    player.SendUseDoneEvent();
                    return;
                }

                // thanks to Endy's Tinkering Calculator for this formula!
                var attemptMod = TinkeringDifficulty[tinkeredCount];

                var difficulty = (int)Math.Floor(((salvageMod * 5.0f) + (itemWorkmanship * salvageMod * 2.0f) - (toolWorkmanship * workmanshipMod * salvageMod / 5.0f)) * attemptMod);

                successChance = SkillCheck.GetSkillChance((int)skill.Current, difficulty);

                // imbue: divide success by 3
                if (recipe.SalvageType == 2)
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
                    var numAugs = recipe.SalvageType == 2 ? player.AugmentationBonusImbueChance : 0;

                    ShowDialog(player, tool, target, (float)successChance, true, numAugs);
                    return;
                }
            }
            else
            {
                // ivory / leather / sandstone always succeeds, and doesn't consume one of the ten tinking slots
                successChance = 1.0f;
                incItemTinkered = false;
            }

            player.IsBusy = true;

            var animLength = DoMotion(player, MotionCommand.ClapHands);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(player, () =>
            {
                DoTinkering(player, tool, target, recipe, (float)successChance, incItemTinkered);
                DoMotion(player, MotionCommand.Ready);
                player.IsBusy = false;
            });
            actionChain.EnqueueChain();

            player.NextUseTime = DateTime.UtcNow.AddSeconds(animLength);
        }

        public static void DoTinkering(Player player, WorldObject tool, WorldObject target, Recipe recipe, float chance, bool incItemTinkered)
        {
            var success = ThreadSafeRandom.Next(0.0f, 1.0f) <= chance;

            var sourceName = Regex.Replace(tool.NameWithMaterial, @" \(\d+\)$", "");

            if (success)
            {
                Tinkering_ModifyItem(player, tool, target, incItemTinkered);

                // send local broadcast
                if (incItemTinkered)
                    player.EnqueueBroadcast(new GameMessageSystemChat($"{player.Name} successfully applies the {sourceName} (workmanship {(tool.Workmanship ?? 0):#.00}) to the {target.NameWithMaterial}.", ChatMessageType.Craft), WorldObject.LocalBroadcastRange, ChatMessageType.Craft);
            }
            else if (incItemTinkered)
                player.EnqueueBroadcast(new GameMessageSystemChat($"{player.Name} fails to apply the {sourceName} Salvage (workmanship {(tool.Workmanship ?? 0):#.00}) to the {target.NameWithMaterial}. The target is destroyed.", ChatMessageType.Craft), WorldObject.LocalBroadcastRange, ChatMessageType.Craft);

            CreateDestroyItems(player, recipe, tool, target, success, !incItemTinkered);

            if (!player.GetCharacterOption(CharacterOption.UseCraftingChanceOfSuccessDialog) || !UseSkillCheck(tool.MaterialType ?? 0))
                player.SendUseDoneEvent();
        }

        public static void Tinkering_ModifyItem(Player player, WorldObject tool, WorldObject target, bool incItemTinkered = true)
        {
            var recipe = GetRecipe(player, tool, target);

            if (tool.MaterialType == null) return;

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
                    target.Value = (int)Math.Round((target.Value ?? 1) * 0.75f);
                    break;
                case MaterialType.Gold:
                    target.Value = (int)Math.Round((target.Value ?? 1) * 1.25f);
                    break;
                case MaterialType.Linen:
                    target.EncumbranceVal = (int)Math.Round((target.EncumbranceVal ?? 1) * 0.75f);
                    break;
                case MaterialType.Ivory:
                    // Recipe already handles this correctly
                    //target.SetProperty(PropertyInt.Attuned, 0);
                    break;
                case MaterialType.Leather:
                    target.Retained = true;
                    break;
                case MaterialType.Sandstone:
                    target.Retained = false;
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

                    if (target.WieldSkillType != (int)Skill.MissileDefense)
                        return;

                    // change wield requirement: missile defense -> melee defense
                    target.WieldSkillType = (int)Skill.MeleeDefense;
                    target.ItemSkillLimit = (int)Skill.MeleeDefense;      // recipe requirements check for this field

                    // increase the wield difficulty
                    if (target.WieldDifficulty != null)
                    {
                        target.WieldDifficulty = target.WieldDifficulty switch
                        {
                            // todo: figure out the exact formula for this conversion
                            160 => 200,
                            205 => 250,
                            245 => 300,
                            270 => 325,
                            290 => 350,
                            305 => 370,
                            330 => 400,
                            340 => 410,
                            _ => (int)Math.Round(target.WieldDifficulty.Value * 1.25f)
                        };
                    }
                    break;

                case MaterialType.Silver:

                    if (target.WieldSkillType != (int)Skill.MeleeDefense)
                        return;

                    // change wield requirement: melee defense -> missile defense
                    target.WieldSkillType = (int)Skill.MissileDefense;
                    target.ItemSkillLimit = (int)Skill.MissileDefense;      // recipe requirements check for this field

                    // decrease the wield difficulty
                    if (target.WieldDifficulty != null)
                    {
                        target.WieldDifficulty = target.WieldDifficulty switch
                        {
                            // todo: figure out the exact formula for this conversion
                            200 => 160,
                            250 => 205,
                            300 => 245,
                            325 => 270,
                            350 => 290,
                            370 => 305,
                            400 => 330,
                            410 => 340,
                            _ => (int)Math.Round(target.WieldDifficulty.Value * 0.8f)
                        };
                    }
                    break;

                case MaterialType.Silk:

                    // remove allegiance rank limit, increase item difficulty by spellcraft?
                    target.ItemAllegianceRankLimit = null;
                    target.ItemDifficulty = (target.ItemDifficulty ?? 0) + target.ItemSpellcraft;
                    break;

                // armatures / trinkets
                // these are handled in recipe mod
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

                // these are handled in recipe mods already

                case MaterialType.SmokeyQuartz:
                    //AddSpell(player, target, SpellId.CANTRIPCOORDINATION1);
                    break;
                case MaterialType.RoseQuartz:
                    //AddSpell(player, target, SpellId.CANTRIPQUICKNESS1);
                    break;
                case MaterialType.RedJade:
                    //AddSpell(player, target, SpellId.CANTRIPHEALTHGAIN1);
                    break;
                case MaterialType.Malachite:
                    //AddSpell(player, target, SpellId.WarriorsVigor);
                    break;
                case MaterialType.LavenderJade:
                    //AddSpell(player, target, SpellId.CANTRIPMANAGAIN1);
                    break;
                case MaterialType.LapisLazuli:
                    //AddSpell(player, target, SpellId.CANTRIPWILLPOWER1);
                    break;
                case MaterialType.Hematite:
                    //AddSpell(player, target, SpellId.WarriorsVitality);
                    break;
                case MaterialType.Citrine:
                    //AddSpell(player, target, SpellId.CANTRIPSTAMINAGAIN1);
                    break;
                case MaterialType.Carnelian:
                    //AddSpell(player, target, SpellId.CANTRIPSTRENGTH1);
                    break;
                case MaterialType.Bloodstone:
                    //AddSpell(player, target, SpellId.CANTRIPENDURANCE1);
                    break;
                case MaterialType.Azurite:
                    //AddSpell(player, target, SpellId.WizardsIntellect);
                    break;
                case MaterialType.Agate:
                    //AddSpell(player, target, SpellId.CANTRIPFOCUS1);
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
                    log.Error($"{player.Name}.RecipeManager.Tinkering_ModifyItem({tool.Name} ({tool.Guid}), {target.Name} ({target.Guid})) - Unknown material type: {materialType}");
                    return;
            }

            // increase # of times tinkered, if appropriate
            if (incItemTinkered)
            {
                target.NumTimesTinkered++;

                if (target.TinkerLog != null)
                    target.TinkerLog += ",";
                target.TinkerLog += (int)materialType;
            }
        }

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

            /*if (IconUnderlay.TryGetValue(effect, out var icon))
            {
                target.IconUnderlayId = icon;
                player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyDataID(target, PropertyDataId.IconUnderlay, target.IconUnderlayId.Value));
            }*/

            return true;
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
            var usable = source.Usable ?? Usable.Undef;

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
                searchLocations |= Player.SearchLocations.MyInventory;
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

        public static void CreateDestroyItems(Player player, Recipe recipe, WorldObject source, WorldObject target, bool success, bool sendMsg = true)
        {
            var destroyTargetChance = success ? recipe.SuccessDestroyTargetChance : recipe.FailDestroyTargetChance;
            var destroySourceChance = success ? recipe.SuccessDestroySourceChance : recipe.FailDestroySourceChance;

            var destroyTarget = ThreadSafeRandom.Next(0.0f, 1.0f) <= destroyTargetChance;
            var destroySource = ThreadSafeRandom.Next(0.0f, 1.0f) <= destroySourceChance;

            var createItem = success ? recipe.SuccessWCID : recipe.FailWCID;
            var createAmount = success ? recipe.SuccessAmount : recipe.FailAmount;

            if (createItem > 0 && DatabaseManager.World.GetWeenie(createItem) == null)
            {
                log.Error($"RecipeManager.CreateDestroyItems: Recipe.Id({recipe.Id}) couldn't find {(success ? "Success" : "Fail")}WCID {createItem} in database.");
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.CraftGeneralErrorUiMsg));
                return;
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

            ModifyItem(player, recipe, source, target, result, success);

            if (sendMsg)
            {
                // TODO: remove this in data for imbues

                // suppress message for imbues w/ a chance of failure here,
                // handled previously in local broadcast

                var message = success ? recipe.SuccessMessage : recipe.FailMessage;

                player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Craft));
            }
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

        public static void ModifyItem(Player player, Recipe recipe, WorldObject source, WorldObject target, WorldObject result, bool success)
        {
            foreach (var mod in recipe.RecipeMod)
            {
                if (mod.ExecutesOnSuccess != success)
                    continue;

                // apply base mod
                switch (mod.DataId)
                {
                    // 	Fetish of the Dark Idols
                    case 0x38000046:
                        AddImbuedEffect(player, target, ImbuedEffectType.IgnoreSomeMagicProjectileDamage);
                        target.SetProperty(PropertyFloat.AbsorbMagicDamage, 0.25f);
                        break;
                }

                // adjust vitals, but all appear to be 0 in current database?

                // apply type mods
                foreach (var boolMod in mod.RecipeModsBool)
                    ModifyBool(player, boolMod, source, target, result);

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

        public static void ModifyBool(Player player, RecipeModsBool boolMod, WorldObject source, WorldObject target, WorldObject result)
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

            if (Debug)
                Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
        }

        public static void ModifyInt(Player player, RecipeModsInt intMod, WorldObject source, WorldObject target, WorldObject result)
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
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.Add:
                    player.UpdateProperty(targetMod, prop, (targetMod.GetProperty(prop) ?? 0) + value);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.IncProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, sourceMod.GetProperty(prop) ?? 0);
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {sourceMod.GetProperty(prop) ?? 0}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, player.GetProperty(prop) ?? 0);     // ??
                    if (Debug) Console.WriteLine($"{result.Name}.SetProperty({prop}, {player.GetProperty(prop) ?? 0}) - {op}");
                    break;
                case ModificationOperation.AddSpell:
                    targetMod.Biota.GetOrAddKnownSpell(intMod.Stat, target.BiotaDatabaseLock, out var added);
                    if (added)
                        targetMod.ChangesDetected = true;
                    if (Debug) Console.WriteLine($"{targetMod.Name}.AddSpell({intMod.Stat}) - {op}");
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyInt({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyFloat(Player player, RecipeModsFloat floatMod, WorldObject source, WorldObject target, WorldObject result)
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
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.Add:
                    player.UpdateProperty(targetMod, prop, (targetMod.GetProperty(prop) ?? 0) + value);
                    if (Debug) Console.WriteLine($"{targetMod.Name}.IncProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, sourceMod.GetProperty(prop) ?? 0);
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {sourceMod.GetProperty(prop) ?? 0}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, player.GetProperty(prop) ?? 0);
                    if (Debug) Console.WriteLine($"{result.Name}.SetProperty({prop}, {player.GetProperty(prop) ?? 0}) - {op}");
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyFloat({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyString(Player player, RecipeModsString stringMod, WorldObject source, WorldObject target, WorldObject result)
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
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, sourceMod.GetProperty(prop) ?? sourceMod.Name);
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {sourceMod.GetProperty(prop) ?? sourceMod.Name}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, player.GetProperty(prop) ?? player.Name);
                    if (Debug) Console.WriteLine($"{result.Name}.SetProperty({prop}, {player.GetProperty(prop) ?? player.Name}) - {op}");
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyString({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
        }

        public static void ModifyInstanceID(Player player, RecipeModsIID iidMod, WorldObject source, WorldObject target, WorldObject result)
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
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, ModifyInstanceIDRuleSet(prop, sourceMod, targetMod));
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {ModifyInstanceIDRuleSet(prop, sourceMod, targetMod)}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, ModifyInstanceIDRuleSet(prop, player, targetMod));     // ??
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

        public static void ModifyDataID(Player player, RecipeModsDID didMod, WorldObject source, WorldObject target, WorldObject result)
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
                    if (Debug) Console.WriteLine($"{targetMod.Name}.SetProperty({prop}, {value}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToTarget:
                    player.UpdateProperty(target, prop, sourceMod.GetProperty(prop) ?? 0);
                    if (Debug) Console.WriteLine($"{target.Name}.SetProperty({prop}, {sourceMod.GetProperty(prop) ?? 0}) - {op}");
                    break;
                case ModificationOperation.CopyFromSourceToResult:
                    player.UpdateProperty(result, prop, player.GetProperty(prop) ?? 0);
                    if (Debug) Console.WriteLine($"{result.Name}.SetProperty({prop}, {player.GetProperty(prop) ?? 0}) - {op}");
                    break;
                default:
                    log.Warn($"RecipeManager.ModifyDataID({source.Name}, {target.Name}): unhandled operation {op}");
                    break;
            }
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

        /// <summary>
        /// Returns TRUE if this material requies a skill check
        /// </summary>
        public static bool UseSkillCheck(MaterialType material)
        {
            return material != MaterialType.Ivory && material != MaterialType.Leather && material != MaterialType.Sandstone;
        }
    }
}
