using System;
using System.Collections.Generic;

using log4net;

using ACE.Database;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.WorldObjects;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Managers
{
    public class RecipeManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Random _random = new Random();

        private static readonly List<GenericPropertyId> _updateStructure = new List<GenericPropertyId>() { new GenericPropertyId((uint)PropertyInt.Structure, PropertyType.PropertyInt) };

        public static void UseObjectOnTarget(Player player, WorldObject source, WorldObject target)
        {
            var recipe = DatabaseManager.World.GetCachedRecipe(source.WeenieClassId, target.WeenieClassId);

            if (recipe == null)
            {
                var message = new GameMessageSystemChat($"The {source.Name} cannot be used on the {target.Name}.", ChatMessageType.Craft);
                player.Session.Network.EnqueueSend(message);
                player.SendUseDoneEvent();
                return;
            }

            switch ((RecipeType)recipe.RecipeType)
            {
                case RecipeType.CreateItem:
                    HandleCreateItemRecipe(player, source, target, recipe);
                    break;
                case RecipeType.Dyeing:
                    break;
                case RecipeType.Healing:
                    HandleHealingRecipe(player, source, target, recipe);
                    break;
                case RecipeType.ManaStone:
                    break;
                case RecipeType.Tinkering:
                    break;
                case RecipeType.Unlocking:
                    break;
                case RecipeType.None:
                    return;
            }
        }

        private static void HandleCreateItemRecipe(Player player, WorldObject source, WorldObject target, AceRecipe recipe)
        {
            ActionChain craftChain = new ActionChain();
            CreatureSkill skill = null;
            bool skillSuccess = true; // assume success, unless there's a skill check
            double percentSuccess = 1;

            UniversalMotion motion = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.ClapHands));
            craftChain.AddAction(player, () => player.HandleActionMotion(motion));
            var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>((uint)player.MotionTableId);
            var craftAnimationLength = motionTable.GetAnimationLength(MotionCommand.ClapHands);
            craftChain.AddDelaySeconds(craftAnimationLength);
            // craftChain.AddDelaySeconds(0.5);

            craftChain.AddAction(player, () =>
            {
                if (recipe.SkillId != null && recipe.SkillDifficulty != null)
                {
                    // there's a skill associated with this
                    Skill skillId = (Skill)recipe.SkillId.Value;

                    // this shouldn't happen, but sanity check for unexpected nulls
                    skill = player.GetCreatureSkill(skillId);

                    if (skill == null)
                    {
                        log.Warn("Unexpectedly missing skill in Recipe usage");
                        player.SendUseDoneEvent();
                        return;
                    }

                    percentSuccess = skill.GetPercentSuccess(recipe.SkillDifficulty.Value);
                }

                // straight skill check, if applciable
                if (skill != null)
                    skillSuccess = _random.NextDouble() < percentSuccess;

                if ((recipe.ResultFlags & (uint)RecipeResult.SourceItemDestroyed) > 0)
                    player.DestroyInventoryItem(source);

                if ((recipe.ResultFlags & (uint)RecipeResult.TargetItemDestroyed) > 0)
                    player.DestroyInventoryItem(target);

                if ((recipe.ResultFlags & (uint)RecipeResult.SourceItemUsesDecrement) > 0)
                {
                    if (source.Structure <= 1)
                        player.DestroyInventoryItem(source);
                    else
                    {
                        source.Structure--;
                        source.SendPartialUpdates(player.Session, _updateStructure);
                    }
                }

                if ((recipe.ResultFlags & (uint)RecipeResult.TargetItemUsesDecrement) > 0)
                {
                    if (target.Structure <= 1)
                        player.DestroyInventoryItem(target);
                    else
                    {
                        target.Structure--;
                        target.SendPartialUpdates(player.Session, _updateStructure);
                    }
                }

                if (skillSuccess)
                {
                    WorldObject newObject1 = null;
                    WorldObject newObject2 = null;

                    if ((recipe.ResultFlags & (uint)RecipeResult.SuccessItem1) > 0 && recipe.SuccessItem1Wcid != null)
                        newObject1 = player.AddNewItemToInventory(recipe.SuccessItem1Wcid.Value);

                    if ((recipe.ResultFlags & (uint)RecipeResult.SuccessItem2) > 0 && recipe.SuccessItem2Wcid != null)
                        newObject2 = player.AddNewItemToInventory(recipe.SuccessItem2Wcid.Value);

                    var text = string.Format(recipe.SuccessMessage, source.Name, target.Name, newObject1?.Name, newObject2?.Name);
                    var message = new GameMessageSystemChat(text, ChatMessageType.Craft);
                    player.Session.Network.EnqueueSend(message);
                }
                else
                {
                    WorldObject newObject1 = null;
                    WorldObject newObject2 = null;

                    if ((recipe.ResultFlags & (uint)RecipeResult.FailureItem1) > 0 && recipe.FailureItem1Wcid != null)
                        newObject1 = player.AddNewItemToInventory(recipe.FailureItem1Wcid.Value);

                    if ((recipe.ResultFlags & (uint)RecipeResult.FailureItem2) > 0 && recipe.FailureItem2Wcid != null)
                        newObject2 = player.AddNewItemToInventory(recipe.FailureItem2Wcid.Value);

                    var text = string.Format(recipe.FailMessage, source.Name, target.Name, newObject1?.Name, newObject2?.Name);
                    var message = new GameMessageSystemChat(text, ChatMessageType.Craft);
                    player.Session.Network.EnqueueSend(message);
                }
                
                player.SendUseDoneEvent();
            });

            craftChain.EnqueueChain();
        }

        private static void HandleHealingRecipe(Player player, WorldObject source, WorldObject target, AceRecipe recipe)
        {
            ActionChain chain = new ActionChain();

            // skill will be null since the difficulty is calculated manually
            if (recipe.SkillId == null)
            {
                log.Warn($"healing recipe has null skill id (should almost certainly be healing, but who knows).  recipe id {recipe.RecipeGuid}.");
                player.SendUseDoneEvent();
                return;
            }

            if (!(target is Player))
            {
                var message = new GameMessageSystemChat($"The {source.Name} cannot be used on {target.Name}.", ChatMessageType.Craft);
                player.Session.Network.EnqueueSend(message);
                player.SendUseDoneEvent();
                return;
            }

            Player targetPlayer = (Player)target;
            //Ability vital = (Ability?)recipe.HealingAttribute ?? Ability.Health;

            // there's a skill associated with this
            Skill skillId = (Skill)recipe.SkillId.Value;

            var skill = player.GetCreatureSkill(skillId);

            // this shouldn't happen, but sanity check for unexpected nulls
            if (skill == null)
            {
                log.Warn("Unexpectedly missing skill in Recipe usage");
                player.SendUseDoneEvent();
                return;
            }

            // at this point, we've validated that the target is a player, and the target is below max health

            if (target.Guid != player.Guid)
            {
                // TODO: validate range
            }

            MotionCommand cmd = MotionCommand.SkillHealSelf;

            if (target.Guid != player.Guid)
                cmd = MotionCommand.Woah; // guess?  nothing else stood out

            // everything pre-validatable is validated.  action will be attempted unless cancelled, so
            // queue up the animation and action
            UniversalMotion motion = new UniversalMotion(MotionStance.Standing, new MotionItem(cmd));
            chain.AddAction(player, () => player.HandleActionMotion(motion));
            chain.AddDelaySeconds(0.5);

            //chain.AddAction(player, () =>
            //{
            //    // TODO: revalidate range if other player (they could have moved)

            //    double difficulty = 2 * (targetPlayer.Vitals[vital].MaxValue - targetPlayer.Vitals[vital].Current);

            //    if (difficulty <= 0)
            //    {
            //        // target is at max (or higher?) health, do nothing
            //        var text = "You are already at full health.";

            //        if (target.Guid != player.Guid)
            //            text = $"{target.Name} is already at full health";
                    
            //        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Craft));
            //        player.SendUseDoneEvent();
            //        return;
            //    }

            //    if (player.CombatMode != CombatMode.NonCombat && player.CombatMode != CombatMode.Undef)
            //        difficulty *= 1.1;
                
            //    int boost = source.Boost ?? 0;
            //    double multiplier = source.HealkitMod ?? 1;

            //    double playerSkill = skill.Current + boost;
            //    if (skill.Status == SkillStatus.Trained)
            //        playerSkill *= 1.1;
            //    else if (skill.Status == SkillStatus.Specialized)
            //        playerSkill *= 1.5;

            //    // usage is inevitable at this point, consume the use
            //    if ((recipe.ResultFlags & (uint)RecipeResult.SourceItemUsesDecrement) > 0)
            //    {
            //        if (source.Structure <= 1)
            //            player.DestroyInventoryItem(source);
            //        else
            //        {
            //            source.Structure--;
            //            source.SendPartialUpdates(player.Session, _updateStructure);
            //        }
            //    }

            //    double percentSuccess = CreatureSkill.GetPercentSuccess((uint)playerSkill, (uint)difficulty);

            //    if (_random.NextDouble() <= percentSuccess)
            //    {
            //        string expertly = "";

            //        if (_random.NextDouble() < 0.1d)
            //        {
            //            expertly = "expertly ";
            //            multiplier *= 1.2;
            //        }

            //        // calculate amount restored
            //        uint maxRestore = targetPlayer.Vitals[vital].MaxValue - targetPlayer.Vitals[vital].Current;

            //        // TODO: get actual forumula for healing.  this is COMPLETELY wrong.  this is 60 + random(1-60).
            //        double amountRestored = 60d + _random.Next(1, 61);
            //        amountRestored *= multiplier;

            //        uint actualRestored = (uint)Math.Min(maxRestore, amountRestored);
            //        targetPlayer.Vitals[vital].Current += actualRestored;
                    
            //        var updateVital = new GameMessagePrivateUpdateAttribute2ndLevel(player.Session, vital.GetVital(), targetPlayer.Vitals[vital].Current);
            //        player.Session.Network.EnqueueSend(updateVital);

            //        if (targetPlayer.Guid != player.Guid)
            //        {
            //            // tell the other player they got healed
            //            var updateVitalToTarget = new GameMessagePrivateUpdateAttribute2ndLevel(targetPlayer.Session, vital.GetVital(), targetPlayer.Vitals[vital].Current);
            //            targetPlayer.Session.Network.EnqueueSend(updateVitalToTarget);
            //        }

            //        string name = "yourself";
            //        if (targetPlayer.Guid != player.Guid)
            //            name = targetPlayer.Name;

            //        string vitalName = "Health";

            //        if (vital == Ability.Stamina)
            //            vitalName = "Stamina";
            //        else if (vital == Ability.Mana)
            //            vitalName = "Mana";

            //        string uses = source.Structure == 1 ? "use" : "uses";

            //        var text = string.Format(recipe.SuccessMessage, expertly, name, actualRestored, vitalName, source.Name, source.Structure, uses);
            //        var message = new GameMessageSystemChat(text, ChatMessageType.Craft);
            //        player.Session.Network.EnqueueSend(message);
                    
            //        if (targetPlayer.Guid != player.Guid)
            //        {
            //            // send text to the other player too
            //            text = string.Format(recipe.AlternateMessage, player.Name, expertly, actualRestored, vitalName);
            //            message = new GameMessageSystemChat(text, ChatMessageType.Craft);
            //            targetPlayer.Session.Network.EnqueueSend(message);
            //        }
            //    }

            //    player.SendUseDoneEvent();
            //});

            //chain.EnqueueChain();
        }
    }
}
