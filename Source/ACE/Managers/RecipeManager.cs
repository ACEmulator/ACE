using ACE.Entity;
using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Managers
{
    public class RecipeManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static RecipeCache _recipeCache = null;

        public static void Initialize()
        {
            // build the cache
            var recipes = Database.DatabaseManager.World.GetAllRecipes();
            _recipeCache = new RecipeCache(recipes);
        }

        public static void UseObjectOnTarget(Player player, WorldObject source, WorldObject target)
        {
            ActionChain craftChain = new ActionChain();
            Recipe recipe = _recipeCache.GetRecipe(source.WeenieClassId, target.WeenieClassId);
            var success = true; // assume success, unless there's a skill check
            
            if (recipe == null)
            {
                var message = new GameMessageSystemChat($"The {source.Name} cannot be used on the {target.Name}.", ChatMessageType.Craft);
                player.Session.Network.EnqueueSend(message);
                player.SendUseDoneEvent();
                return;
            }

            UniversalMotion motion = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.ClapHands));
            craftChain.AddAction(player, () => player.HandleActionMotion(motion));
            craftChain.AddDelaySeconds(0.5);

            craftChain.AddAction(player, () =>
            {
                if (recipe.SkillId != null && recipe.SkillDifficulty != null)
                {
                    // there's a skill associated with this
                    Skill skillId = (Skill)recipe.SkillId.Value;

                    // this shouldn't happen, but sanity check for unexpected nulls
                    if (!player.Skills.ContainsKey(skillId))
                    {
                        log.Info("Unexpectedly missing skill in Recipe usage");
                        player.SendUseDoneEvent();
                        return;
                    }

                    CreatureSkill skill = player.Skills[skillId];

                    success = skill.SkillCheck(recipe.SkillDifficulty.Value);
                }

                switch ((RecipeType)recipe.RecipeType)
                {
                    case RecipeType.CreateItem:
                        if ((recipe.ResultFlags & (uint)RecipeResult.SourceItemDestroyed) > 0)
                            player.DestroyInventoryItem(source);

                        if ((recipe.ResultFlags & (uint)RecipeResult.TargetItemDestroyed) > 0)
                            player.DestroyInventoryItem(target);

                        if ((recipe.ResultFlags & (uint)RecipeResult.SourceItemUsesDecrement) > 0)
                        {
                            if (source.Structure <= 1)
                                player.DestroyInventoryItem(source);
                            else
                                source.Structure--;
                        }

                        if ((recipe.ResultFlags & (uint)RecipeResult.TargetItemUsesDecrement) > 0)
                        {
                            if (target.Structure <= 1)
                                player.DestroyInventoryItem(target);
                            else
                                target.Structure--;
                        }

                        if (success)
                        {
                            WorldObject newObject1 = null;
                            WorldObject newObject2 = null;

                            if ((recipe.ResultFlags & (uint)RecipeResult.CreateNewItem1) > 0 && recipe.Item1Wcid != null)
                            {
                                newObject1 = Factories.WorldObjectFactory.CreateNewWorldObject(recipe.Item1Wcid.Value);
                                newObject1.ContainerId = player.Guid.Full;
                                newObject1.Placement = 0;
                                player.HandleAddToInventoryEx(newObject1);
                            }

                            if ((recipe.ResultFlags & (uint)RecipeResult.CreateNewItem2) > 0 && recipe.Item2Wcid != null)
                            {
                                newObject2 = Factories.WorldObjectFactory.CreateNewWorldObject(recipe.Item2Wcid.Value);
                                newObject2.ContainerId = player.Guid.Full;
                                newObject2.Placement = 0;
                                player.HandleAddToInventoryEx(newObject2);
                            }

                            var text = string.Format(recipe.SuccessMessage, source.Name, target.Name, newObject1?.Name, newObject2?.Name);
                            var message = new GameMessageSystemChat(text, ChatMessageType.Craft);
                            player.Session.Network.EnqueueSend(message);
                        }

                        break;
                    case RecipeType.Dyeing:
                        break;
                    case RecipeType.Healing:
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

                player.SendUseDoneEvent();
            });
            
            // unnecessary?
            // craftChain.AddAction(player, () => player.HandleActionMotion(new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Ready))));

            craftChain.EnqueueChain();
        }
    }
}
