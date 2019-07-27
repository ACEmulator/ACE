using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

using log4net;

using Position = ACE.Entity.Position;

namespace ACE.Server.Managers
{
    public partial class EmoteManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WorldObject WorldObject => _proxy ?? _worldObject;

        private WorldObject _worldObject;
        private WorldObject _proxy;

        /// <summary>
        /// Returns TRUE if this WorldObject is currently busy processing other emotes
        /// </summary>
        public bool IsBusy;

        public bool Debug = false;

        public EmoteManager(WorldObject worldObject)
        {
            _worldObject = worldObject;
        }

        /// <summary>
        /// Executes an emote
        /// </summary>
        /// <param name="emoteSet">The parent set of this emote</param>
        /// <param name="emote">The emote to execute</param>
        /// <param name="targetObject">A target object, usually player</param>
        /// <param name="actionChain">Only used for passing to further sets</param>
        public float ExecuteEmote(BiotaPropertiesEmote emoteSet, BiotaPropertiesEmoteAction emote, WorldObject targetObject = null)
        {
            var player = targetObject as Player;
            var creature = WorldObject as Creature;
            var targetCreature = targetObject as Creature;

            var delay = 0.0f;
            var emoteType = (EmoteType)emote.Type;

            //if (Debug)
                //Console.WriteLine($"{WorldObject.Name}.ExecuteEmote({emoteType})");

            var text = emote.Message;

            switch ((EmoteType)emote.Type)
            {
                case EmoteType.Act:
                    // short for 'acting' text
                    var message = Replace(text, WorldObject, targetObject);
                    WorldObject.EnqueueBroadcast(new GameMessageSystemChat(message, ChatMessageType.Broadcast), 30.0f);
                    break;

                case EmoteType.Activate:

                    if (WorldObject.ActivationTarget > 0)
                    {
                        // ActOnUse delay?
                        var activationTarget = WorldObject.CurrentLandblock?.GetObject(WorldObject.ActivationTarget);
                        activationTarget?.OnActivate(WorldObject);
                    }
                    else if (WorldObject.GeneratorId.HasValue && WorldObject.GeneratorId > 0) // Fallback to linked generator
                    {
                        var linkedGenerator = WorldObject.CurrentLandblock?.GetObject(WorldObject.GeneratorId ?? 0);
                        linkedGenerator?.OnActivate(WorldObject);
                    }
                    break;

                case EmoteType.AddCharacterTitle:

                    // emoteAction.Stat == null for all EmoteType.AddCharacterTitle entries in current db?
                    if (player != null && emote.Amount != 0)
                        player.AddTitle((CharacterTitle)emote.Amount);
                    break;

                case EmoteType.AddContract:

                    // Contracts werent in emote table for 16py, guessing that Stat was used to hold id for contract.
                    if (player != null && emote.Stat.HasValue && emote.Stat.Value > 0)
                        player.ContractManager.Add(emote.Stat.Value);
                    break;

                case EmoteType.AdminSpam:

                    var players = PlayerManager.GetAllOnline();
                    foreach (var onlinePlayer in players)
                        onlinePlayer.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.AdminTell));
                    break;

                case EmoteType.AwardLevelProportionalSkillXP:

                    if (player != null)
                        player.GrantLevelProportionalSkillXP((Skill)emote.Stat, emote.Percent ?? 0, (ulong)emote.Max64);
                    break;

                case EmoteType.AwardLevelProportionalXP:

                    bool shareXP = emote.Display ?? false;

                    if (player != null)
                        player.GrantLevelProportionalXp(emote.Percent ?? 0, (ulong)emote.Max64, shareXP);
                    break;

                case EmoteType.AwardLuminance:

                    if (player != null)
                        player.GrantLuminance((long)emote.Amount);
                    break;

                case EmoteType.AwardNoShareXP:

                    if (player != null)
                        player.EarnXP((long)emote.Amount64, XpType.Quest, ShareType.None);

                    break;

                case EmoteType.AwardSkillPoints:

                    if (player != null)
                        player.AwardSkillPoints((Skill)emote.Stat, (uint)emote.Amount);
                    break;

                case EmoteType.AwardSkillXP:

                    if (player != null)
                        player.AwardSkillXP((Skill)emote.Stat, (uint)emote.Amount);
                    break;

                case EmoteType.AwardTrainingCredits:

                    if (player != null)
                        player.AddSkillCredits((int)emote.Amount, false);
                    break;

                case EmoteType.AwardXP:

                    if (player != null)
                    {
                        var amt = (long)emote.Amount64;
                        if (amt > 0)
                        {
                            player.EarnXP(amt, XpType.Quest, ShareType.All);
                        }
                        else if (amt < 0)
                        {
                            player.SpendXP(-amt);
                        }
                    }
                    break;

                case EmoteType.BLog:
                    // only one test drudge used this emoteAction.
                    break;

                case EmoteType.CastSpell:

                    if (WorldObject != null && targetObject != null)
                    {
                        var spell = new Spell((uint)emote.SpellId);
                        if (!spell.NotFound)
                        {
                            var preCastTime = creature.PreCastMotion(targetObject);
                            delay = preCastTime * 2.0f;

                            var castChain = new ActionChain();
                            castChain.AddDelaySeconds(preCastTime);
                            castChain.AddAction(creature, () =>
                            {
                                creature.TryCastSpell(spell, targetObject, creature);
                                creature.PostCastMotion();
                            });
                            castChain.EnqueueChain();
                        }
                    }
                    break;

                case EmoteType.CastSpellInstant:

                    if (WorldObject != null)
                    {
                        var spell = new Spell((uint)emote.SpellId);
                        if (!spell.NotFound)
                            WorldObject.TryCastSpell(spell, targetObject, WorldObject);
                    }
                    break;

                case EmoteType.CloseMe:

                    // animation delay?
                    if (WorldObject is Container container)
                        container.Close(null);
                    break;

                case EmoteType.CreateTreasure:

                    if (emote.WealthRating.HasValue)
                    {
                        // todo: make use of emote.TreasureClass and emote.TreasureType fields.
                        // this emote is primarily seen on fishing holes so defaulting with jewelery as the only pcap showed 2:1 amulet to crown pull (not much to go on) for now
                        var treasure = LootGenerationFactory.CreateRandomLootObjects(emote.WealthRating ?? 1, false, LootGenerationFactory.LootBias.Jewelry /* probably treasure type here */);
                        if (treasure != null)
                        {
                            player.TryCreateInInventoryWithNetworking(treasure);
                        }
                    }
                    break;

                /* decrements a PropertyInt stat by some amount */
                case EmoteType.DecrementIntStat:

                    // only used by 1 emote in 16PY - check for lower bounds?
                    if (targetObject != null && emote.Stat != null)
                    {
                        var intProperty = (PropertyInt)emote.Stat;
                        var current = targetObject.GetProperty(intProperty) ?? 0;
                        current -= emote.Amount ?? 0;
                        targetObject.SetProperty(intProperty, current);

                        if (player != null)
                            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, intProperty, current));
                    }
                    break;

                case EmoteType.DecrementMyQuest:
                    break;

                case EmoteType.DecrementQuest:
                    // Used as part of the test drudge for events
                    break;

                case EmoteType.DeleteSelf:

                    var destroyChain = new ActionChain();
                    destroyChain.AddAction(WorldObject, () => WorldObject.ApplyVisualEffects(PlayScript.Destroy));
                    delay = 3.0f;
                    destroyChain.AddDelaySeconds(delay);
                    destroyChain.AddAction(WorldObject, () => WorldObject.Destroy());
                    destroyChain.EnqueueChain();

                    break;

                case EmoteType.DirectBroadcast:

                    text = Replace(emote.Message, WorldObject, targetObject);

                    if (player != null)
                    {
                        if ((emote.Message).EndsWith("@%tqt", StringComparison.Ordinal))
                        {
                            var questName = QuestManager.GetQuestName(emote.Message);
                            var remainStr = player.QuestManager.GetNextSolveTime(questName).GetFriendlyString();
                            text = $"{questName}: {remainStr}";
                        }
                        else if ((emote.Message).Contains("%CDtime"))
                        {
                            var questName = QuestManager.GetQuestName(emote.Message);
                            TimeSpan timeSpan = player.QuestManager.GetNextSolveTime(questName);
                            string buffer = (emote.Message).Split("@")[1];

                            string time = $"{timeSpan.Minutes} minutes";

                            if (timeSpan.Hours > 0)
                                time = time.Insert(0, $"{timeSpan.Hours} hours and ");

                            text = buffer.Replace("%CDtime", time);
                        }
                        else
                            text = Replace(emote.Message, WorldObject, targetObject);

                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));     // CreatureMessage / HearDirectSpeech?
                    }
                    break;

                case EmoteType.EraseMyQuest:
                    break;

                case EmoteType.EraseQuest:

                    if (player != null)
                        player.QuestManager.Erase(emote.Message);
                    break;

                case EmoteType.FellowBroadcast:

                    if (player != null)
                    {
                        var fellowship = player.Fellowship;
                        if (fellowship == null)
                        {
                            text = Replace(emote.Message, WorldObject, player);
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                        }
                        else
                        {
                            var fellowshipMembers = fellowship.GetFellowshipMembers();

                            foreach (var fellow in fellowshipMembers.Values)
                            {
                                text = Replace(emote.Message, WorldObject, fellow);
                                fellow.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                            }
                        }
                    }
                    break;

                case EmoteType.Generate:

                    if (WorldObject.IsGenerator)
                        WorldObject.Generator_Regeneration();
                    break;

                case EmoteType.Give:

                    bool success = false;
                    if (player != null && emote.WeenieClassId != null)
                    {
                        var item = WorldObjectFactory.CreateNewWorldObject((uint)emote.WeenieClassId);

                        var stackMsg = "";
                        if (item != null)
                        {
                            var stackSize = emote.StackSize ?? 1;
                            if (stackSize > 1)
                            {
                                item.SetStackSize(stackSize);
                                stackMsg = stackSize + " ";     // pluralize?
                            }
                        }
                        else
                            item = PlayerFactory.CreateIOU((uint)emote.WeenieClassId);

                        success = player.TryCreateInInventoryWithNetworking(item);

                        // transaction / rollback on failure?
                        if (success)
                        {
                            var msg = new GameMessageSystemChat($"{WorldObject.Name} gives you {stackMsg}{item.Name}.", ChatMessageType.Broadcast);
                            var sound = new GameMessageSound(player.Guid, Sound.ReceiveItem, 1);
                            if (!(WorldObject.GetProperty(PropertyBool.NpcInteractsSilently) ?? false))
                                player.Session.Network.EnqueueSend(msg, sound);
                            else
                                player.Session.Network.EnqueueSend(sound);

                            if (PropertyManager.GetBool("player_receive_immediate_save").Item)
                                player.RushNextPlayerSave(5);
                        }
                    }
                    break;

                /* redirects to the GotoSet category for this action */
                case EmoteType.Goto:

                    // TODO: revisit if nested chains need to back-propagate timers
                    var gotoSet = GetEmoteSet(EmoteCategory.GotoSet, emote.Message);
                    ExecuteEmoteSet(gotoSet, targetObject, true);
                    break;

                /* increments a PropertyInt stat by some amount */
                case EmoteType.IncrementIntStat:

                    if (targetObject != null && emote.Stat != null)
                    {
                        var intProperty = (PropertyInt)emote.Stat;
                        var current = targetObject.GetProperty(intProperty) ?? 0;
                        current += emote.Amount ?? 0;
                        targetObject.SetProperty(intProperty, current);

                        if (player != null)
                            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, intProperty, current));
                    }
                    break;

                case EmoteType.IncrementMyQuest:
                    break;

                case EmoteType.IncrementQuest:

                    if (player != null)
                        player.QuestManager.Increment(emote.Message);     // kill task?
                    break;

                case EmoteType.InflictVitaePenalty:
                    if (player != null)
                        player.InflictVitaePenalty(emote.Amount ?? 5);
                    break;

                case EmoteType.InqAttributeStat:

                    if (targetCreature != null)
                    {
                        var attr = targetCreature.Attributes[(PropertyAttribute)emote.Stat];
                        success = attr != null && attr.Current >= emote.Min && attr.Current <= emote.Max;
                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqBoolStat:

                    // This is only used with NPC's 24944 and 6386, which are dev tester npc's. Not worth the current effort.
                    // Could also be post-ToD
                    break;

                case EmoteType.InqContractsFull:

                    // not part of the game at PY16?
                    ExecuteEmoteSet(player != null && player.ContractManager.IsFull ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    break;

                case EmoteType.InqEvent:

                    var started = EventManager.IsEventStarted(emote.Message);
                    ExecuteEmoteSet(started ? EmoteCategory.EventSuccess : EmoteCategory.EventFailure, emote.Message, targetObject, true);
                    break;

                case EmoteType.InqFellowNum:

                    // unused in PY16 - ensure # of fellows between min-max?
                    ExecuteEmoteSet(player != null && player.Fellowship != null ? EmoteCategory.TestSuccess : EmoteCategory.TestNoFellow, emote.Message, targetObject, true);
                    break;

                case EmoteType.InqFellowQuest:

                    if (player != null)
                    {
                        if (player.Fellowship != null)
                        {
                            var hasQuest = player.Fellowship.QuestManager.HasQuest(emote.Message);
                            var canSolve = player.Fellowship.QuestManager.CanSolve(emote.Message);

                            // verify: QuestSuccess = player has quest, and their last completed time + quest minDelta <= currentTime
                            success = hasQuest && !canSolve;

                            ExecuteEmoteSet(success ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                        }
                        else
                            ExecuteEmoteSet(EmoteCategory.QuestNoFellow, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqFloatStat:

                    if (targetObject != null)
                    {
                        var stat = targetObject.GetProperty((PropertyFloat)emote.Stat) ?? 0.0f;
                        success = stat >= emote.MinDbl && stat <= emote.MaxDbl;

                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);

                    }
                    break;

                case EmoteType.InqInt64Stat:

                    if (targetObject != null)
                    {
                        var stat = targetObject.GetProperty((PropertyInt64)emote.Stat) ?? 0;
                        success = stat >= emote.Min64 && stat <= emote.Max64;

                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqIntStat:

                    if (targetObject != null)
                    {
                        var stat = targetObject.GetProperty((PropertyInt)emote.Stat) ?? 0;
                        success = stat >= emote.Min && stat <= emote.Max;

                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqMyQuest:
                    break;
                case EmoteType.InqMyQuestBitsOff:
                    break;
                case EmoteType.InqMyQuestBitsOn:
                    break;
                case EmoteType.InqMyQuestSolves:
                    break;
                case EmoteType.InqNumCharacterTitles:

                    //if (player != null)
                    //InqCategory(player.NumCharacterTitles != 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    break;

                case EmoteType.InqOwnsItems:

                    if (player != null)
					{
                        var numRequired = emote.StackSize ?? 1;

                        var items = player.GetInventoryItemsOfWCID(emote.WeenieClassId ?? 0);
                        var numItems = items.Sum(i => i.StackSize ?? 1);

                        success = numItems >= numRequired;

                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqPackSpace:

                    //if (player != null)
                    //{
                    //    var freeSpace = player.ContainerCapacity > player.ItemCapacity;
                    //    InqCategory(freeSpace ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    //}
                    break;

                case EmoteType.InqQuest:

                    if (player != null)
                    {
                        var hasQuest = player.QuestManager.HasQuest(emote.Message);
                        var canSolve = player.QuestManager.CanSolve(emote.Message);

                        // verify: QuestSuccess = player has quest, and their last completed time + quest minDelta <= currentTime
                        success = hasQuest && !canSolve;

                        ExecuteEmoteSet(success ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqQuestBitsOff:
                    break;
                case EmoteType.InqQuestBitsOn:
                    break;
                case EmoteType.InqQuestSolves:

                    if (player != null)
                    {
                        var questSolves = player.QuestManager.HasQuestSolves(emote.Message, emote.Min, emote.Max);

                        ExecuteEmoteSet(questSolves ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqRawAttributeStat:

                    if (targetCreature != null)
                    {
                        var attr = targetCreature.Attributes[(PropertyAttribute)emote.Stat];
                        success = attr != null && attr.Base >= emote.Min && attr.Base <= emote.Max;
                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqRawSecondaryAttributeStat:

                    if (targetCreature != null)
                    {
                        var vital = targetCreature.Vitals[(PropertyAttribute2nd)emote.Stat];
                        success = vital != null && vital.Base >= emote.Min && vital.Base <= emote.Max;
                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqRawSkillStat:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emote.Stat);
                        success = skill != null && skill.Base >= emote.Min && skill.Base <= emote.Max;
                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqSecondaryAttributeStat:

                    if (targetCreature != null)
                    {
                        var vital = targetCreature.Vitals[(PropertyAttribute2nd)emote.Stat];
                        success = vital != null && vital.Current >= emote.Min && vital.Current <= emote.Max;
                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqSkillSpecialized:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emote.Stat);
                        success = skill.AdvancementClass == SkillAdvancementClass.Specialized;

                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqSkillStat:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emote.Stat);
                        success = skill != null && skill.Current >= emote.Min && skill.Current <= emote.Max;

                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqSkillTrained:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emote.Stat);
                        success = skill.AdvancementClass >= SkillAdvancementClass.Trained;

                        // TestNoQuality?
                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqStringStat:

                    if (targetCreature != null)
                    {
                        // rarely used, only in test data?
                        if (Enum.TryParse(emote.TestString, true, out PropertyString propStr))
                        {
                            var stat = targetCreature.GetProperty(propStr);
                            success = stat.Equals(emote.Message);

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqYesNo:

                    if (player != null)
                    {
                        player.ConfirmationManager.EnqueueSend(new Confirmation_YesNo(WorldObject.Guid, player.Guid, emote.Message), emote.TestString);
                    }
                    break;

                case EmoteType.Invalid:
                    break;

                case EmoteType.KillSelf:

                    if (targetCreature != null)
                        targetCreature.Smite(targetCreature);
                    break;

                case EmoteType.LocalBroadcast:

                    message = Replace(emote.Message, WorldObject, targetObject);
                    WorldObject.EnqueueBroadcast(new GameMessageSystemChat(message, ChatMessageType.Broadcast));
                    break;

                case EmoteType.LocalSignal:
                    if (player != null)
                    {
                        if (player.CurrentLandblock != null)
                            player.CurrentLandblock.EmitSignal(player, emote.Message);
                    }
                    break;

                case EmoteType.LockFellow:

                    if (player != null && player.Fellowship != null)
                        player.HandleActionFellowshipChangeLock(true);

                    break;

                /* plays an animation on the target object (usually the player) */
                case EmoteType.ForceMotion:

                    var motionCommand = MotionCommandHelper.GetMotion(emote.Motion.Value);
                    var motion = new Motion(targetObject, motionCommand, emote.Extent);
                    targetObject.EnqueueBroadcastMotion(motion);
                    break;

                /* plays an animation on the source object */
                case EmoteType.Motion:

                    var debugMotion = false;

                    if (Debug)
                        Console.Write($".{(MotionCommand)emote.Motion}");

                    // are there players within emote range?
                    if (!WorldObject.PlayersInRange(ClientMaxAnimRange))
                        break;

                    if (WorldObject == null || WorldObject.CurrentMotionState == null) break;

                    // TODO: REFACTOR ME
                    if (emoteSet.Category != (uint)EmoteCategory.Vendor && emoteSet.Style != null)
                    {
                        var startingMotion = new Motion((MotionStance)emoteSet.Style, (MotionCommand)emoteSet.Substyle);
                        motion = new Motion((MotionStance)emoteSet.Style, (MotionCommand)emote.Motion, emote.Extent);

                        if (WorldObject.CurrentMotionState.Stance != startingMotion.Stance)
                        {
                            if (WorldObject.CurrentMotionState.Stance == MotionStance.Invalid)
                            {
                                if (debugMotion)
                                    Console.WriteLine($"{WorldObject.Name} running starting motion {(MotionStance)emoteSet.Style}, {(MotionCommand)emoteSet.Substyle}");

                                delay = WorldObject.ExecuteMotion(startingMotion);
                            }
                        }
                        else
                        {
                            if (WorldObject.CurrentMotionState.MotionState.ForwardCommand == startingMotion.MotionState.ForwardCommand
                                    && startingMotion.Stance == MotionStance.NonCombat)     // enforce non-combat here?
                            {
                                if (debugMotion)
                                    Console.WriteLine($"{WorldObject.Name} running motion {(MotionStance)emoteSet.Style}, {(MotionCommand)emote.Motion}");

                                float? maxRange = ClientMaxAnimRange;
                                if (MotionQueue.Contains((MotionCommand)emote.Motion))
                                    maxRange = null;

                                var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(WorldObject.MotionTableId);
                                var animLength = motionTable.GetAnimationLength(WorldObject.CurrentMotionState.Stance, (MotionCommand)emote.Motion, MotionCommand.Ready);

                                delay = WorldObject.ExecuteMotion(motion, true, maxRange);

                                var motionChain = new ActionChain();
                                motionChain.AddDelaySeconds(animLength);
                                motionChain.AddAction(WorldObject, () =>
                                {
                                    // FIXME: better cycle handling
                                    var cmd = WorldObject.CurrentMotionState.MotionState.ForwardCommand;
                                    if (cmd != MotionCommand.Dead && cmd != MotionCommand.Sleeping && cmd != MotionCommand.Sitting && !cmd.ToString().EndsWith("State"))
                                    {
                                        if (debugMotion)
                                            Console.WriteLine($"{WorldObject.Name} running starting motion again {(MotionStance)emoteSet.Style}, {(MotionCommand)emoteSet.Substyle}");

                                        WorldObject.ExecuteMotion(startingMotion);
                                    }
                                });
                                motionChain.EnqueueChain();

                                if (debugMotion)
                                    Console.WriteLine($"{WorldObject.Name} appending time to existing chain: " + animLength);
                            }
                        }
                    }
                    else
                    {
                        // vendor / other motions
                        var startingMotion = new Motion(MotionStance.NonCombat, MotionCommand.Ready);
                        var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(WorldObject.MotionTableId);
                        var animLength = motionTable.GetAnimationLength(WorldObject.CurrentMotionState.Stance, (MotionCommand)emote.Motion, MotionCommand.Ready);

                        motion = new Motion(MotionStance.NonCombat, (MotionCommand)emote.Motion, emote.Extent);

                        if (debugMotion)
                            Console.WriteLine($"{WorldObject.Name} running motion (block 2) {MotionStance.NonCombat}, {(MotionCommand)(emote.Motion ?? 0)}");

                        delay = WorldObject.ExecuteMotion(motion);

                        var motionChain = new ActionChain();
                        motionChain.AddDelaySeconds(animLength);
                        motionChain.AddAction(WorldObject, () => WorldObject.ExecuteMotion(startingMotion, false));

                        motionChain.EnqueueChain();
                    }

                    break;

                /* move to position relative to home */
                case EmoteType.Move:

                    if (creature != null)
                    {
                        var newPos = new Position(creature.Home);
                        newPos.Pos += new Vector3(emote.OriginX ?? 0, emote.OriginY ?? 0, emote.OriginZ ?? 0);      // uses relative offsets
                        newPos.Rotation *= new Quaternion(emote.AnglesX ?? 0, emote.AnglesY ?? 0, emote.AnglesZ ?? 0, emote.AnglesW ?? 1);  // also relative?

                        if (Debug)
                            Console.WriteLine(newPos.ToLOCString());

                        // get new cell
                        newPos.LandblockId = new LandblockId(PositionExtensions.GetCell(newPos));

                        // TODO: handle delay for this?
                        creature.MoveTo(newPos, creature.GetRunRate());
                    }
                    break;

                case EmoteType.MoveHome:

                    // TODO: call MoveToManager on server, handle delay for this?
                    if (creature != null && creature.Home != null)
                    {
                        // are we already at home origin?
                        if (creature.Location.Pos.Equals(creature.Home.Pos))
                        {
                            // just turnto if required?
                            if (Debug)
                                Console.Write($" - already at home origin, checking rotation");

                            if (!creature.Location.Rotation.Equals(creature.Home.Rotation))
                            {
                                if (Debug)
                                    Console.Write($" - turning to");
                                delay = creature.TurnTo(creature.Home);
                            }
                            else if (Debug)
                                Console.Write($" - already at home rotation, doing nothing");
                        }
                        else
                        {
                            if (Debug)
                                Console.Write($" - {creature.Home.ToLOCString()}");

                            // how to get delay with this, callback required?
                            creature.MoveTo(creature.Home, creature.GetRunRate());
                        }
                    }
                    break;

                case EmoteType.MoveToPos:

                    if (creature != null)
                    {
                        var currentPos = creature.Location;

                        var newPos = new Position();
                        newPos.LandblockId = new LandblockId(currentPos.LandblockId.Raw);
                        newPos.Pos = new Vector3(emote.OriginX ?? currentPos.Pos.X, emote.OriginY ?? currentPos.Pos.Y, emote.OriginZ ?? currentPos.Pos.Z);

                        if (emote.AnglesX == null || emote.AnglesY == null || emote.AnglesZ == null || emote.AnglesW == null)
                            newPos.Rotation = new Quaternion(currentPos.Rotation.X, currentPos.Rotation.Y, currentPos.Rotation.Z, currentPos.Rotation.W);
                        else
                            newPos.Rotation = new Quaternion(emote.AnglesX ?? 0, emote.AnglesY ?? 0, emote.AnglesZ ?? 0, emote.AnglesW ?? 1);

                        if (emote.ObjCellId != null)
                            newPos.LandblockId = new LandblockId(emote.ObjCellId.Value);

                        // TODO: handle delay for this?
                        creature.MoveTo(newPos, creature.GetRunRate());
                    }
                    break;

                case EmoteType.OpenMe:

                    if (WorldObject is Container openContainer)
                        openContainer.Open(null);

                    break;

                case EmoteType.PetCastSpellOnOwner:

                    if (creature != null)
                        creature.CreateCreatureSpell(targetObject.Guid, (uint)emote.SpellId);
                    break;

                case EmoteType.PhysScript:

                    // TODO: landblock broadcast
                    WorldObject.PhysicsObj.play_script((PlayScript)emote.PScript, 1.0f);
                    break;

                case EmoteType.PopUp:

                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameEventPopupString(player.Session, emote.Message));
                    break;

                case EmoteType.RemoveContract:

                    if (player != null && emote.Stat.HasValue && emote.Stat.Value > 0)
                        player.HandleActionAbandonContract((uint)emote.Stat);
                    break;

                case EmoteType.RemoveVitaePenalty:

                    if (player != null) player.VitaeCpPool = 0;     // TODO: call full path
                    break;

                case EmoteType.ResetHomePosition:

                    //creature = sourceObject as Creature;
                    //if (creature != null)
                    //    creature.Home = emoteAction.Position;
                    break;

                case EmoteType.Say:

                    if (Debug)
                        Console.Write($" - {emote.Message}");

                    WorldObject.EnqueueBroadcast(new GameMessageCreatureMessage(emote.Message, WorldObject.Name, WorldObject.Guid.Full, ChatMessageType.Emote), WorldObject.LocalBroadcastRange);
                    break;

                case EmoteType.SetAltRacialSkills:
                    break;

                case EmoteType.SetBoolStat:
                    if (player != null)
                    {
                        player.UpdateProperty(player, (PropertyBool)emote.Stat, emote.Amount == 0 ? false : true);
                        player.EnqueueBroadcast(false, new GameMessagePublicUpdatePropertyBool(player, (PropertyBool)emote.Stat, emote.Amount == 0 ? false : true));
                    }
                    break;

                case EmoteType.SetEyePalette:
                    //if (creature != null)
                    //    creature.EyesPaletteDID = (uint)emote.Display;
                    break;

                case EmoteType.SetEyeTexture:
                    //if (creature != null)
                    //    creature.EyesTextureDID = (uint)emote.Display;
                    break;

                case EmoteType.SetFloatStat:
                    if (player != null)
                    {
                        player.UpdateProperty(player, (PropertyFloat)emote.Stat, emote.Percent);
                        player.EnqueueBroadcast(false, new GameMessagePublicUpdatePropertyFloat(player, (PropertyFloat)emote.Stat, Convert.ToDouble(emote.Percent)));
                    }
                    break;

                case EmoteType.SetHeadObject:
                    //if (creature != null)
                    //    creature.HeadObjectDID = (uint)emote.Display;
                    break;

                case EmoteType.SetHeadPalette:
                    break;

                case EmoteType.SetInt64Stat:
                    if (player != null)
                    {
                        player.UpdateProperty(player, (PropertyInt64)emote.Stat, emote.Amount64);
                        player.EnqueueBroadcast(false, new GameMessagePublicUpdatePropertyInt64(player, (PropertyInt64)emote.Stat, Convert.ToInt64(emote.Amount64)));
                    }
                    break;

                case EmoteType.SetIntStat:
                    if (player != null)
                    {
                        player.UpdateProperty(player, (PropertyInt)emote.Stat, emote.Amount);
                        player.EnqueueBroadcast(false, new GameMessagePublicUpdatePropertyInt(player, (PropertyInt)emote.Stat, Convert.ToInt32(emote.Amount)));
                    }
                    break;

                case EmoteType.SetMouthPalette:
                    break;

                case EmoteType.SetMouthTexture:
                    //if (creature != null)
                    //    creature.MouthTextureDID = (uint)emote.Display;
                    break;

                case EmoteType.SetMyQuestBitsOff:
                    break;
                case EmoteType.SetMyQuestBitsOn:
                    break;
                case EmoteType.SetMyQuestCompletions:
                    break;
                case EmoteType.SetNosePalette:
                    break;

                case EmoteType.SetNoseTexture:
                    //if (creature != null)
                    //    creature.NoseTextureDID = (uint)emote.Display;
                    break;

                case EmoteType.SetQuestBitsOff:
                    break;
                case EmoteType.SetQuestBitsOn:
                    break;
                case EmoteType.SetQuestCompletions:
                    if (player != null)
                    {
                        if (emote.Amount != null)
                            player.QuestManager.SetQuestCompletions(emote.Message, (int)emote.Amount);
                    }
                    break;
                case EmoteType.SetSanctuaryPosition:

                    if (player != null)
                        player.SetPosition(PositionType.Sanctuary, new Position(emote.ObjCellId.Value, emote.OriginX.Value, emote.OriginY.Value, emote.OriginZ.Value, emote.AnglesX.Value, emote.AnglesY.Value, emote.AnglesZ.Value, emote.AnglesW.Value));
                    break;

                case EmoteType.Sound:

                    WorldObject.EnqueueBroadcast(new GameMessageSound(WorldObject.Guid, (Sound)emote.Sound, 1.0f));
                    break;

                case EmoteType.SpendLuminance:
                    if (player != null)
                        player.SpendLuminance((long)emote.Amount);
                    break;

                case EmoteType.StampFellowQuest:
                    if (player != null)
                    {
                        if (player.Fellowship != null)
                        {
                            var questName = emote.Message;

                            // are there fellowship only kill tasks?
                            //if (questName.EndsWith("@#kt", StringComparison.Ordinal))
                            //{
                            //    player.Fellowship.QuestManager.HandleKillTask(questName, WorldObject);
                            //}
                            //else
                                player.Fellowship.QuestManager.Stamp(emote.Message);
                        }
                    }
                    break;
                case EmoteType.StampMyQuest:
                    break;
                case EmoteType.StampQuest:

                    if (player != null)
                    {
                        var questName = emote.Message;

                        if (questName.EndsWith("@#kt", StringComparison.Ordinal))
                        {
                            player.QuestManager.HandleKillTask(questName, WorldObject, player.CurrentRadarRange);
                        }
                        else
                            player.QuestManager.Stamp(emote.Message);
                    }
                    break;

                case EmoteType.StartBarber:
                    if (player != null)
                        player.StartBarber();
                    break;

                case EmoteType.StartEvent:

                    EventManager.StartEvent(emote.Message);
                    break;

                case EmoteType.StopEvent:

                    EventManager.StopEvent(emote.Message);
                    break;

                case EmoteType.TakeItems:

                    if (player != null)
                        if (player.TryConsumeFromInventoryWithNetworking(emote.WeenieClassId ?? 0, emote.StackSize ?? 0))
                        {
                            var itemTaken = WorldObjectFactory.CreateWorldObject(DatabaseManager.World.GetCachedWeenie(emote.WeenieClassId ?? 0), ObjectGuid.Invalid);
                            if (itemTaken != null)
                            {
                                var msg = $"You hand over {emote.StackSize ?? 1} of your {itemTaken.GetPluralName()}.";
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                            }
                        }

                    break;

                case EmoteType.TeachSpell:

                    if (player != null)
                        player.LearnSpellWithNetworking((uint)emote.SpellId);
                    break;

                case EmoteType.TeleportSelf:

                    //if (WorldObject is Player)
                    //(WorldObject as Player).Teleport(emote.Position);
                    break;

                case EmoteType.TeleportTarget:

                    if (player != null)
                    {
                        if (emote.ObjCellId.HasValue && emote.OriginX.HasValue && emote.OriginY.HasValue && emote.OriginZ.HasValue && emote.AnglesX.HasValue && emote.AnglesY.HasValue && emote.AnglesZ.HasValue && emote.AnglesW.HasValue)
                        {
                            var destination = new Position(emote.ObjCellId.Value, emote.OriginX.Value, emote.OriginY.Value, emote.OriginZ.Value, emote.AnglesX.Value, emote.AnglesY.Value, emote.AnglesZ.Value, emote.AnglesW.Value);

                            player.AdjustDungeon(destination);
                            player.Teleport(destination);
                        }
                    }
                    break;

                case EmoteType.Tell:

                    if (player != null)
                    {
                        message = Replace(emote.Message, WorldObject, player);
                        player.Session.Network.EnqueueSend(new GameMessageHearDirectSpeech(WorldObject, message, player, ChatMessageType.Tell));
                    }
                    break;

                case EmoteType.TellFellow:

                    if (player != null)
                    {
                        var fellowship = player.Fellowship;
                        if (fellowship == null)
                        {
                            message = Replace(emote.Message, WorldObject, player);
                            player.Session.Network.EnqueueSend(new GameMessageHearDirectSpeech(WorldObject, message, player, ChatMessageType.Tell));
                        }
                        else
                        {
                            var fellowshipMembers = fellowship.GetFellowshipMembers();

                            foreach (var fellow in fellowshipMembers.Values)
                            {
                                message = Replace(emote.Message, WorldObject, fellow);
                                player.Session.Network.EnqueueSend(new GameMessageHearDirectSpeech(WorldObject, message, fellow, ChatMessageType.Tell));
                            }
                        }
                    }
                    break;

                case EmoteType.TextDirect:

                    if (player != null)
                    {
                        message = Replace(emote.Message, WorldObject, player);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Broadcast));
                    }
                    break;

                case EmoteType.Turn:

                    if (creature != null)
                    {
                        // turn to heading
                        var rotation = new Quaternion(emote.AnglesX ?? 0, emote.AnglesY ?? 0, emote.AnglesZ ?? 0, emote.AnglesW ?? 1);
                        var newPos = new Position(creature.Location);
                        newPos.Rotation = rotation;

                        var rotateTime = creature.TurnTo(newPos);
                        delay = rotateTime;
                    }
                    break;

                case EmoteType.TurnToTarget:

                    if (creature != null && targetCreature != null)
                        delay = creature.Rotate(targetCreature);

                    break;

                case EmoteType.UntrainSkill:

                    if (player != null)
                        player.ResetSkill((Skill)emote.Stat);
                    break;

                case EmoteType.UpdateFellowQuest:
                    if (player != null)
                    {
                        if (player.Fellowship != null)
                        {
                            var questName = emote.Message;

                            var hasQuest = player.Fellowship.QuestManager.HasQuest(questName);

                            if (!hasQuest)
                            {
                                // add new quest
                                player.Fellowship.QuestManager.Update(questName);
                                hasQuest = player.Fellowship.QuestManager.HasQuest(questName);
                                ExecuteEmoteSet(hasQuest ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                            }
                            else
                            {
                                // update existing quest
                                var canSolve = player.Fellowship.QuestManager.CanSolve(questName);
                                ExecuteEmoteSet(canSolve ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                            }
                        }
                        else
                            ExecuteEmoteSet(EmoteCategory.QuestNoFellow, emote.Message, targetObject, true);
                    }
                    break;
                case EmoteType.UpdateMyQuest:
                    break;
                case EmoteType.UpdateQuest:

                    if (player != null)
                    {
                        var questName = emote.Message;

                        var hasQuest = player.QuestManager.HasQuest(questName);

                        if (!hasQuest)
                        {
                            // add new quest
                            player.QuestManager.Update(questName);
                            hasQuest = player.QuestManager.HasQuest(questName);
                            ExecuteEmoteSet(hasQuest ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                        }
                        else
                        {
                            // update existing quest
                            var canSolve = player.QuestManager.CanSolve(questName);
                            ExecuteEmoteSet(canSolve ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.WorldBroadcast:

                    message = Replace(text, WorldObject, targetObject);

                    var onlinePlayers = PlayerManager.GetAllOnline();

                    foreach (var session in onlinePlayers)
                        session.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.WorldBroadcast));

                    break;

                default:
                    log.Debug($"EmoteManager.Execute - Encountered Unhandled EmoteType {(EmoteType)emote.Type} for {WorldObject.Name} ({WorldObject.WeenieClassId})");
                    break;
            }

            return delay;
        }

        /// <summary>
        /// Selects an emote set based on category, and optional: quest, vendor, rng
        /// </summary>
        public BiotaPropertiesEmote GetEmoteSet(EmoteCategory category, string questName = null, VendorType? vendorType = null, uint? wcid = null, bool useRNG = true)
        {
            // always pull emoteSet from _worldObject
            var emoteSet = _worldObject.Biota.BiotaPropertiesEmote.Where(e => e.Category == (uint)category);

            // optional criteria
            if (questName != null)
                emoteSet = emoteSet.Where(e => e.Quest.Equals(questName, StringComparison.OrdinalIgnoreCase));
            if (vendorType != null)
                emoteSet = emoteSet.Where(e => e.VendorType != null && e.VendorType.Value == (uint)vendorType);
            if (wcid != null)
                emoteSet = emoteSet.Where(e => e.WeenieClassId == wcid.Value);

            if (category == EmoteCategory.HeartBeat)
            {
                WorldObject.GetCurrentMotionState(out MotionStance currentStance, out MotionCommand currentMotion);

                emoteSet = emoteSet.Where(e => e.Style == null || e.Style == (uint)currentStance);
                emoteSet = emoteSet.Where(e => e.Substyle == null || e.Substyle == (uint)currentMotion);
            }

            if (useRNG)
            {
                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
                emoteSet = emoteSet.OrderBy(e => e.Probability).Where(e => e.Probability >= rng);
                //emoteSet = emoteSet.Where(e => e.Probability >= rng);
            }

            return emoteSet.FirstOrDefault();
        }

        /// <summary>
        /// Convenience wrapper between GetEmoteSet and ExecututeEmoteSet
        /// </summary>
        public void ExecuteEmoteSet(EmoteCategory category, string quest = null, WorldObject targetObject = null, bool nested = false)
        {
            var emoteSet = GetEmoteSet(category, quest);

            // TODO: revisit if nested chains need to propagate timers
            ExecuteEmoteSet(emoteSet, targetObject, nested);
        }

        /// <summary>
        /// Executes a set of emotes to run with delays
        /// </summary>
        /// <param name="emoteSet">A list of emotes to execute</param>
        /// <param name="targetObject">An optional target, usually player</param>
        /// <param name="actionChain">For adding delays between emotes</param>
        public bool ExecuteEmoteSet(BiotaPropertiesEmote emoteSet, WorldObject targetObject = null, bool nested = false)
        {
            // detect busy state
            // TODO: maybe eventually we should consider having categories that can be queued?
            // there are some categories that shouldn't be queued, like heartbeats...
            if (IsBusy && !nested) return false;

            // start action chain
            Enqueue(emoteSet, targetObject);

            return true;
        }

        public void Enqueue(BiotaPropertiesEmote emoteSet, WorldObject targetObject, int emoteIdx = 0, float delay = 0.0f)
        {
            if (emoteSet == null) return;

            IsBusy = true;
            var emote = emoteSet.BiotaPropertiesEmoteAction.ElementAt(emoteIdx);

            var actionChain = new ActionChain();

            // post-delay from actual time of previous emote
            actionChain.AddDelaySeconds(delay);

            // pre-delay for current emote
            actionChain.AddDelaySeconds(emote.Delay);
            if (Debug)
                Console.Write($"{emote.Delay} - ");

            actionChain.AddAction(WorldObject, () =>
            {
                if (Debug)
                    Console.Write($"{(EmoteType)emote.Type}");

                var nextDelay = ExecuteEmote(emoteSet, emote, targetObject);

                if (Debug)
                    Console.WriteLine($" - { nextDelay}");

                if (emoteIdx < emoteSet.BiotaPropertiesEmoteAction.Count - 1)
                    Enqueue(emoteSet, targetObject, emoteIdx + 1, nextDelay);
                else
                {
                    var delayChain = new ActionChain();
                    delayChain.AddDelaySeconds(nextDelay);
                    delayChain.AddAction(WorldObject, () => IsBusy = false);
                    delayChain.EnqueueChain();
                }
            });
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// The maximum animation range of the client
        /// Motions broadcast outside of this range will be automatically queued by client
        /// </summary>
        public static float ClientMaxAnimRange = 96.0f;     // verify: same indoors?

        /// <summary>
        /// The client automatically queues animations that are broadcast outside of 96.0f range
        /// Normally we exclude these emotes from being broadcast outside this range,
        /// but for certain emotes (like monsters going to sleep) we want to always broadcast / enqueue
        /// </summary>
        public static HashSet<MotionCommand> MotionQueue = new HashSet<MotionCommand>()
        {
            MotionCommand.Sleeping
        };

        public void DoVendorEmote(VendorType vendorType, WorldObject target)
        {
            var vendorSet = GetEmoteSet(EmoteCategory.Vendor, null, vendorType);
            var heartbeatSet = GetEmoteSet(EmoteCategory.Vendor, null, VendorType.Heartbeat);

            ExecuteEmoteSet(vendorSet, target);
            ExecuteEmoteSet(heartbeatSet, target, true);
        }

        public IEnumerable<BiotaPropertiesEmote> Emotes(EmoteCategory emoteCategory)
        {
            return WorldObject.Biota.BiotaPropertiesEmote.Where(x => x.Category == (int)emoteCategory);
        }

        public string Replace(string message, WorldObject source, WorldObject target)
        {
            var result = message;

            var sourceName = source != null ? source.Name : "";
            var targetName = target != null ? target.Name : "";

            result = result.Replace("%n", sourceName);
            result = result.Replace("%mn", sourceName);
            result = result.Replace("%s", targetName);
            result = result.Replace("%tn", targetName);
            result = result.Replace("%tqt", "some amount of time");

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HeartBeat()
        {
            // player didn't do idle emotes in retail?
            if (WorldObject is Player)
                return;

            if (WorldObject is Creature creature && creature.IsAwake)
                return;

            ExecuteEmoteSet(EmoteCategory.HeartBeat);
        }

        public void OnUse(Creature activator)
        {
            ExecuteEmoteSet(EmoteCategory.Use, null, activator);
        }

        public void OnPortal(Creature activator)
        {
            ExecuteEmoteSet(EmoteCategory.Portal, null, activator);
        }

        public void OnActivation(Creature activator)
        {
            ExecuteEmoteSet(EmoteCategory.Activation, null, activator);
        }

        public void OnGeneration()
        {
            ExecuteEmoteSet(EmoteCategory.Generation, null, null);
        }

        public void OnWield(Creature wielder)
        {
            ExecuteEmoteSet(EmoteCategory.Wield, null, wielder);
        }

        public void OnUnwield(Creature wielder)
        {
            ExecuteEmoteSet(EmoteCategory.UnWield, null, wielder);
        }

        public void OnPickup(Creature initiator)
        {
            ExecuteEmoteSet(EmoteCategory.PickUp, null, initiator);
        }

        public void OnDrop(Creature dropper)
        {
            ExecuteEmoteSet(EmoteCategory.Drop, null, dropper);
        }

        public void OnAttack(Creature attacker)
        {
            ExecuteEmoteSet(EmoteCategory.NewEnemy, null, attacker);
        }

        public void OnDamage(Creature attacker)
        {
            // optionally restrict to Min/Max Health %
            ExecuteEmoteSet(EmoteCategory.WoundedTaunt, null, attacker);
        }

        public void OnReceiveCritical(Creature attacker)
        {
            ExecuteEmoteSet(EmoteCategory.ReceiveCritical, null, attacker);
        }

        public void OnDeath(DamageHistory damageHistory)
        {
            IsBusy = false;

            if (damageHistory.Damagers.Count == 0)
                ExecuteEmoteSet(EmoteCategory.Death, null, null);
            else 
                ExecuteEmoteSet(EmoteCategory.Death, null, damageHistory.LastDamager);
        }

        /// <summary>
        /// Called when a monster kills a player
        /// </summary>
        public void OnKill(Player player)
        {
            ExecuteEmoteSet(EmoteCategory.KillTaunt, null, player);
        }

        /// <summary>
        /// Called when this NPC receives a direct text message from a player
        /// </summary>
        public void OnTalkDirect(Player player, string message)
        {
            ExecuteEmoteSet(EmoteCategory.ReceiveTalkDirect, message, player);
        }

        /// <summary>
        /// Called when this NPC receives a local signal from a player
        /// </summary>
        public void OnLocalSignal(Player player, string message)
        {
            ExecuteEmoteSet(EmoteCategory.ReceiveLocalSignal, message, player);
        }

        public bool HasAntennas => WorldObject.Biota.BiotaPropertiesEmote.Count(x => x.Category == (int)EmoteCategory.ReceiveLocalSignal) > 0;

        /// <summary>
        /// Call this function when WorldObject is being used via a proxy object, e.g.: Hooker on a Hook
        /// </summary>
        public void SetProxy(WorldObject worldObject)
        {
            _proxy = worldObject;
        }

        /// <summary>
        /// Called when this object is removed from the proxy object (Hooker is picked up from Hook)
        /// </summary>
        public void ClearProxy()
        {
            _proxy = null;
        }
    }
}
