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

        public WorldObject WorldObject;

        /// <summary>
        /// Returns TRUE if this WorldObject is currently busy processing other emotes
        /// </summary>
        public bool IsBusy;

        public bool Debug = false;

        public EmoteManager(WorldObject worldObject)
        {
            WorldObject = worldObject;
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
                        activationTarget?.ActOnUse(WorldObject);
                    }
                    break;

                case EmoteType.AddCharacterTitle:

                    // emoteAction.Stat == null for all EmoteType.AddCharacterTitle entries in current db?
                    if (player != null && emote.Amount != 0)
                        player.AddTitle((CharacterTitle)emote.Amount);
                    break;

                case EmoteType.AddContract:

                    //if (player != null)
                        //Contracts werent in emote table
                        //player.AddContract(emote.Stat);
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

                    if (player != null)
                        player.GrantLevelProportionalXp(emote.Percent ?? 0, (ulong)emote.Max64);
                    break;

                case EmoteType.AwardLuminance:

                    if (player != null)
                        player.GrantLuminance((long)emote.Amount);
                    break;

                case EmoteType.AwardNoShareXP:

                    if (player != null)
                    {
                        player.EarnXP((long)emote.Amount64);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat("You've earned " + emote.Amount64.Value.ToString("N0") + " experience.", ChatMessageType.Broadcast));
                    }
                    break;

                case EmoteType.AwardSkillPoints:

                    if (player != null)
                        player.AwardSkillPoints((Skill)emote.Stat, (uint)emote.Amount, true);
                    break;

                case EmoteType.AwardSkillXP:

                    if (player != null)
                        player.RaiseSkillGameAction((Skill)emote.Stat, (uint)emote.Amount, true);
                    break;

                case EmoteType.AwardTrainingCredits:

                    if (player != null)
                        player.AddSkillCredits((int)emote.Amount, false);
                    break;

                case EmoteType.AwardXP:

                    if (player != null)
                    {
                        player.EarnXP((long)emote.Amount64);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat("You've earned " + emote.Amount64.Value.ToString("N0") + " experience.", ChatMessageType.Broadcast));
                    }
                    break;

                case EmoteType.BLog:
                    // only one test drudge used this emoteAction.
                    break;

                case EmoteType.CastSpell:

                    if (WorldObject != null && targetObject != null)
                    {
                        var spell = new Spell((uint)emote.SpellId);
                        if (spell != null)
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
                        if (spell != null)
                            WorldObject.TryCastSpell(spell, targetObject, WorldObject);
                    }
                    break;

                case EmoteType.CloseMe:

                    // animation delay?
                    if (targetObject != null)
                        targetObject.Close(WorldObject);
                    break;

                case EmoteType.CreateTreasure:
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
                            foreach (var fellow in fellowship.FellowshipMembers)
                            {
                                text = Replace(emote.Message, WorldObject, fellow);
                                fellow.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                            }
                        }
                    }
                    break;

                case EmoteType.Generate:

                    if (WorldObject.IsGenerator)
                        WorldObject.Generator_HeartBeat();
                    break;

                case EmoteType.Give:

                    bool success = false;
                    if (player != null && emote.WeenieClassId != null)
                    {
                        var item = WorldObjectFactory.CreateNewWorldObject((uint)emote.WeenieClassId);
                        var stackSize = emote.StackSize ?? 1;
                        var stackMsg = "";
                        if (stackSize > 1)
                        {
                            item.SetStackSize(stackSize);
                            stackMsg = stackSize + " ";     // pluralize?
                        }
                        success = player.TryCreateInInventoryWithNetworking(item);

                        // transaction / rollback on failure?
                        if (success)
                        {
                            var msg = new GameMessageSystemChat($"{WorldObject.Name} gives you {stackMsg}{item.Name}.", ChatMessageType.Broadcast);
                            var sound = new GameMessageSound(player.Guid, Sound.ReceiveItem, 1);
                            player.Session.Network.EnqueueSend(msg, sound);
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
                        success = attr != null && attr.Ranks >= emote.Min && attr.Ranks <= emote.Max;
                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqBoolStat:

                    // This is only used with NPC's 24944 and 6386, which are dev tester npc's. Not worth the current effort.
                    // Could also be post-ToD
                    break;

                case EmoteType.InqContractsFull:

                    // not part of the game at PY16?
                    //if (player != null)
                    //{
                    //    var contracts = player.TrackedContracts;
                    //    InqCategory(contracts.Count != 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    //}
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

                    // focusing on 1 person quests to begin with
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
                        success = stat >= emote.Min && stat <= emote.Max;

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
                        success = vital != null && vital.Ranks >= emote.Min && vital.Ranks <= emote.Max;
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
                        success = skill != null && skill.Ranks >= emote.Min && skill.Ranks <= emote.Max;

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
                        var confirm = new Confirmation(ConfirmationType.Yes_No, emote.TestString, WorldObject, null, player, emote.Message);
                        ConfirmationManager.AddConfirmation(confirm);

                        player.Session.Network.EnqueueSend(new GameEventConfirmationRequest(player.Session, ConfirmationType.Yes_No, confirm.ConfirmationID, emote.TestString));
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
                    break;

                case EmoteType.LockFellow:

                    if (player != null && player.Fellowship != null)
                        player.HandleActionFellowshipChangeOpenness(false);

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

                    // TODO: refactor me!
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
                            if (WorldObject.CurrentMotionState.MotionState.ForwardCommand == startingMotion.MotionState.ForwardCommand)
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
                                    if (cmd != MotionCommand.Sleeping && cmd != MotionCommand.Sitting && !cmd.ToString().EndsWith("State"))
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

                    WorldObject.Open(WorldObject);
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
                    {
                        if ((emote.Stat == null) || ((ConfirmationType)emote.Stat == ConfirmationType.Undefined))
                            player.Session.Network.EnqueueSend(new GameEventPopupString(player.Session, emote.Message));
                        else
                        {
                            Confirmation confirm = new Confirmation((ConfirmationType)emote.Stat, emote.Message, WorldObject, targetObject);
                            ConfirmationManager.AddConfirmation(confirm);
                            player.Session.Network.EnqueueSend(new GameEventConfirmationRequest(player.Session, (ConfirmationType)emote.Stat, confirm.ConfirmationID, confirm.Message));
                        }
                    }
                    break;

                case EmoteType.RemoveContract:

                    if (player != null)
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
                    targetObject.SetProperty((PropertyBool)emote.Stat, emote.Amount == 0 ? false : true);
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
                    targetObject.SetProperty((PropertyFloat)emote.Stat, (float)emote.Amount);
                    break;

                case EmoteType.SetHeadObject:
                    //if (creature != null)
                    //    creature.HeadObjectDID = (uint)emote.Display;
                    break;

                case EmoteType.SetHeadPalette:
                    break;

                case EmoteType.SetInt64Stat:
                    player.SetProperty((PropertyInt64)emote.Stat, (int)emote.Amount);
                    break;

                case EmoteType.SetIntStat:
                    player.SetProperty((PropertyInt)emote.Stat, (int)emote.Amount);
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
                    break;
                case EmoteType.StampMyQuest:
                    break;
                case EmoteType.StampQuest:

                    // work needs to be done here
                    if (player != null)
                    {
                        if ((emote.Message).EndsWith("@#kt", StringComparison.Ordinal))
                        {
                            var hasQuest = player.QuestManager.HasQuest(emote.Message);
                            if (hasQuest)
                            {
                                player.QuestManager.Stamp(emote.Message);

                                var questName = QuestManager.GetQuestName(emote.Message);
                                var quest = DatabaseManager.World.GetCachedQuest(questName);

                                var playerQuest = player.QuestManager.Quests.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));

                                if (playerQuest != null)
                                {
                                    var isMaxSolves = player.QuestManager.IsMaxSolves(questName);
                                    if (isMaxSolves)
                                        text = $"You have killed {quest.MaxSolves} {WorldObject.Name}s. Your task is complete!";
                                    else
                                        text = $"You have killed {playerQuest.NumTimesCompleted} {WorldObject.Name}s. You must kill {quest.MaxSolves} to complete your task!";
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                                }
                            }
                        }
                        else
                            player.QuestManager.Stamp(emote.Message);
                    }
                    break;

                case EmoteType.StartBarber:
                    break;

                case EmoteType.StartEvent:

                    EventManager.StartEvent(emote.Message);
                    break;

                case EmoteType.StopEvent:

                    EventManager.StopEvent(emote.Message);
                    break;

                case EmoteType.TakeItems:

                    if (player != null)
                    {
                        var items = player.GetInventoryItemsOfWCID(emote.WeenieClassId ?? 0);

                        var leftReq = emote.StackSize ?? 0;
                        foreach (var item in items)
                        {
                            var removeNum = Math.Min(leftReq, item.StackSize ?? 1);
                            player.TryConsumeFromInventoryWithNetworking(item, removeNum);

                            leftReq -= removeNum;
                            if (leftReq <= 0)
                                break;
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

                    //if (player != null)
                    //player.Teleport(emote.Position);
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
                            foreach (var fellow in fellowship.FellowshipMembers)
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
                        player.UntrainSkill((Skill)emote.Stat, 1);
                    break;

                case EmoteType.UpdateFellowQuest:
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
            var emoteSet = WorldObject.Biota.BiotaPropertiesEmote.Where(e => e.Category == (uint)category);

            // optional criteria
            if (questName != null)
                emoteSet = emoteSet.Where(e => e.Quest.Equals(questName, StringComparison.OrdinalIgnoreCase));
            if (vendorType != null)
                emoteSet = emoteSet.Where(e => e.VendorType != null && e.VendorType.Value == (uint)vendorType);
            if (wcid != null)
                emoteSet = emoteSet.Where(e => e.WeenieClassId == wcid.Value);
            if (useRNG)
                emoteSet = emoteSet.Where(e => e.Probability >= ThreadSafeRandom.Next(0.0f, 1.0f));

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

        public void OnReceiveCritical(Creature attacker)
        {
            ExecuteEmoteSet(EmoteCategory.ReceiveCritical, null, attacker);
        }

        public void OnDeath(DamageHistory damageHistory)
        {
            foreach (var damager in damageHistory.Damagers)
                ExecuteEmoteSet(EmoteCategory.Death, null, damager);

            if (damageHistory.Damagers.Count == 0)
                ExecuteEmoteSet(EmoteCategory.Death, null, null);
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
    }
}
