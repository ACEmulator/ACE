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
                    if (skill.AdvancementClass == SkillAdvancementClass.Untrained)
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
                            player.TryRemoveItemFromInventoryWithNetworking(target, (ushort)recipe.Recipe.SuccessDestroyTargetAmount);
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

                        if (!String.IsNullOrEmpty(recipe.Recipe.SuccessDestroyTargetMessage))
                        {
                            var destroyMessage = new GameMessageSystemChat(recipe.Recipe.SuccessDestroyTargetMessage, ChatMessageType.Craft);
                            player.Session.Network.EnqueueSend(destroyMessage);
                        }
                    }

                    if (destroySource)
                    {
                        if (source.OwnerId == player.Guid.Full || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworking(source, (ushort)recipe.Recipe.SuccessDestroySourceAmount);
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

                        if (!String.IsNullOrEmpty(recipe.Recipe.SuccessDestroySourceMessage))
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
                            player.TryRemoveItemFromInventoryWithNetworking(target, (ushort)recipe.Recipe.FailDestroyTargetAmount);
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

                        if (!String.IsNullOrEmpty(recipe.Recipe.FailDestroyTargetMessage))
                        {
                            var destroyMessage = new GameMessageSystemChat(recipe.Recipe.FailDestroyTargetMessage, ChatMessageType.Craft);
                            player.Session.Network.EnqueueSend(destroyMessage);
                        }
                    }

                    if (destroySource)
                    {
                        if (source.OwnerId == player.Guid.Full || player.GetInventoryItem(target.Guid) != null)
                        {
                            player.TryRemoveItemFromInventoryWithNetworking(source, (ushort)recipe.Recipe.FailDestroySourceAmount);
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

                        if (!String.IsNullOrEmpty(recipe.Recipe.FailDestroySourceMessage))
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
    }
}
