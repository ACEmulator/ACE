using log4net;

using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.WorldObjects;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.WorldObjects.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Factories;

using System;
using System.Linq;

namespace ACE.Server.Managers
{
    public class RecipeManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Random _random = new Random();

        public static void UseObjectOnTarget(Player player, WorldObject source, WorldObject target)
        {
            var recipe = DatabaseManager.World.GetCachedCookbook(source.WeenieClassId, target.WeenieClassId);

            if (recipe == null)
            {
                var message = new GameMessageSystemChat($"The {source.Name} cannot be used on the {target.Name}.", ChatMessageType.Craft);
                player.Session.Network.EnqueueSend(message);
                player.SendUseDoneEvent();
                return;
            }

            ActionChain craftChain = new ActionChain();
            CreatureSkill skill = null;
            bool skillSuccess = true; // assume success, unless there's a skill check
            double percentSuccess = 1;

            UniversalMotion motion = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.ClapHands));
            craftChain.AddAction(player, () => player.HandleActionMotion(motion));
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

                    percentSuccess = skill.GetPercentSuccess(recipe.Recipe.Difficulty); //FIXME: Pretty certain this is broken
                }

                if (skill != null)
                {
                    if (skill.Status == SkillStatus.Untrained)
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


                var components = recipe.Recipe.RecipeComponent.ToList();

                if (skillSuccess)
                {
                    var targetSuccess = components[0];
                    var sourceSuccess = components[1];

                    bool destroyTarget = _random.NextDouble() < targetSuccess.DestroyChance;
                    bool destroySource = _random.NextDouble() < sourceSuccess.DestroyChance;

                    if (destroyTarget)
                    {
                        // TODO - Check if the target is wielded and handle appropriately
                        if (target.OwnerId == player.Guid.Full  || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworking(target, (ushort)targetSuccess.DestroyAmount);
                        }
                        else
                        {
                            target.Destroy();
                        }

                        if (targetSuccess.DestroyMessage != "")
                        {
                            var destroyMessage = new GameMessageSystemChat(targetSuccess.DestroyMessage, ChatMessageType.Craft);
                            player.Session.Network.EnqueueSend(destroyMessage);
                        }
                    }

                    if (destroySource)
                    {
                        // TODO - Check if the source is wielded and handle appropriately
                        if (source.OwnerId == player.Guid.Full || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworking(source, (ushort)sourceSuccess.DestroyAmount);
                        }
                        else
                        {
                            source.Destroy();
                        }

                        if (sourceSuccess.DestroyMessage != "")
                        {
                            var destroyMessage = new GameMessageSystemChat(sourceSuccess.DestroyMessage, ChatMessageType.Craft);
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
                    var targetFail = components[2];
                    var sourceFail = components[3];

                    bool destroyTarget = _random.NextDouble() < targetFail.DestroyChance;
                    bool destroySource = _random.NextDouble() < sourceFail.DestroyChance;

                    if (destroyTarget)
                    {
                        // TODO - Check if the target is wielded and handle appropriately
                        if (target.OwnerId == player.Guid.Full || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworking(target, (ushort)targetFail.DestroyAmount);
                        }
                        else
                        {
                            target.Destroy();
                        }

                        if (targetFail.DestroyMessage != "")
                        {
                            var destroyMessage = new GameMessageSystemChat(targetFail.DestroyMessage, ChatMessageType.Craft);
                            player.Session.Network.EnqueueSend(destroyMessage);
                        }
                    }

                    if (destroySource)
                    {
                        // TODO - Check if the source is wielded and handle appropriately
                        if (source.OwnerId == player.Guid.Full || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworking(source, (ushort)sourceFail.DestroyAmount);
                        }
                        else
                        {
                            source.Destroy();
                        }

                        if (sourceFail.DestroyMessage != "")
                        {
                            var destroyMessage = new GameMessageSystemChat(sourceFail.DestroyMessage, ChatMessageType.Craft);
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
    }
}
