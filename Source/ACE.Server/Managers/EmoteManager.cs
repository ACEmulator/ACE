using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.WorldObjects;
using log4net;
using Position = ACE.Entity.Position;

namespace ACE.Server.Managers
{
    public partial class EmoteManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WorldObject WorldObject;
        public double EndTime;
        public Queue<QueuedEmote> EmoteQueue;

        public EmoteManager(WorldObject worldObject)
        {
            WorldObject = worldObject;
        }

        public void ExecuteEmote(BiotaPropertiesEmote emote, BiotaPropertiesEmoteAction emoteAction, ActionChain actionChain, WorldObject sourceObject = null, WorldObject targetObject = null)
        {
            var player = targetObject as Player;
            var creature = sourceObject as Creature;
            var targetCreature = targetObject as Creature;

            var emoteType = (EmoteType)emoteAction.Type;

            //if (emoteType != EmoteType.Motion && emoteType != EmoteType.Turn && emoteType != EmoteType.Move)
                //Console.WriteLine($"ExecuteEmote({emoteType})");

            var text = emoteAction.Message;

            switch ((EmoteType)emoteAction.Type)
            {
                case EmoteType.Act:
                    // short for 'acting' text
                    var message = Replace(text, sourceObject, targetObject);
                    sourceObject.CurrentLandblock?.EnqueueBroadcast(sourceObject.Location, new GameMessageSystemChat(message, ChatMessageType.Broadcast));
                    break;

                case EmoteType.Activate:
                    actionChain.AddDelaySeconds(emoteAction.Delay);
                    actionChain.AddAction(sourceObject, () =>
                    {
                        if (creature != null && (creature.ActivationTarget ?? 0) > 0)
                        {
                            var activationTarget = creature.CurrentLandblock?.GetObject(creature.ActivationTarget ?? 0);
                            activationTarget?.ActOnUse(creature);
                        }
                    });
                    break;

                case EmoteType.AddCharacterTitle:

                    // emoteAction.Stat == null for all EmoteType.AddCharacterTitle entries in current db?
                    if (player != null)
                        player.AddTitle((CharacterTitle)emoteAction.Stat);
                    break;

                case EmoteType.AddContract:

                    //if (player != null)
                        //Contracts werent in emote table
                        //player.AddContract(emoteAction.Stat);
                    break;

                case EmoteType.AdminSpam:

                    var players = WorldManager.GetAll();
                    foreach (var onlinePlayer in players)
                        onlinePlayer.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.AdminTell));
                    break;

                case EmoteType.AwardLevelProportionalSkillXP:

                    if (player != null)
                        player.GrantLevelProportionalSkillXP((Skill)emoteAction.Stat, emoteAction.Percent ?? 0, (ulong)emoteAction.Max);
                    break;

                case EmoteType.AwardLevelProportionalXP:

                    if (player != null)
                        player.GrantLevelProportionalXp(emoteAction.Percent ?? 0, (ulong)emoteAction.Max);
                    break;

                case EmoteType.AwardLuminance:

                    if (player != null)
                        player.GrantLuminance((long)emoteAction.Amount);
                    break;

                case EmoteType.AwardNoShareXP:

                    actionChain.AddDelaySeconds(emoteAction.Delay);
                    actionChain.AddAction(sourceObject, () =>
                    {
                        if (player != null)
                        {
                            player.EarnXP((long)emoteAction.Amount64);
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You've earned " + emoteAction.Amount64 + " experience.", ChatMessageType.Broadcast));
                        }
                    });
                    break;

                case EmoteType.AwardSkillPoints:

                    if (player != null)
                        player.AwardSkillPoints((Skill)emoteAction.Stat, (uint)emoteAction.Amount, true);
                    break;

                case EmoteType.AwardSkillXP:

                    if (player != null)
                        player.RaiseSkillGameAction((Skill)emoteAction.Stat, (uint)emoteAction.Amount, true);
                    break;

                case EmoteType.AwardTrainingCredits:

                    if (player != null)
                        player.AddSkillCredits((int)emoteAction.Amount, true);
                    break;

                case EmoteType.AwardXP:

                    actionChain.AddDelaySeconds(emoteAction.Delay);
                    actionChain.AddAction(sourceObject, () =>
                    {
                        if (player != null)
                        {
                            player.EarnXP((long)emoteAction.Amount64);
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You've earned " + emoteAction.Amount64 + " experience.", ChatMessageType.Broadcast));
                        }
                    });
                    break;

                case EmoteType.BLog:
                    // only one test drudge used this emoteAction.
                    break;

                case EmoteType.CastSpell:

                    if (WorldObject is Player)
                        (WorldObject as Player).CreatePlayerSpell((uint)emoteAction.SpellId);

                    else if (WorldObject is Creature)
                        (WorldObject as Creature).CreateCreatureSpell(player.Guid, (uint)emoteAction.SpellId);

                    break;

                case EmoteType.CastSpellInstant:

                    var spellTable = DatManager.PortalDat.SpellTable;
                    var spell = spellTable.Spells[(uint)emoteAction.SpellId];
                    actionChain.AddAction(sourceObject, () =>
                    {
                        if (spell.TargetEffect > 0)
                            creature.CreateCreatureSpell(targetObject.Guid, (uint)emoteAction.SpellId);
                        else
                            creature.CreateCreatureSpell((uint)emoteAction.SpellId);
                    });
                    break;

                case EmoteType.CloseMe:
                    targetObject.Close(WorldObject);
                    break;

                case EmoteType.CreateTreasure:
                    break;

                case EmoteType.DecrementIntStat:

                    var id = (PropertyInt)emoteAction.Stat;
                    var prop = player.GetProperty(id);
                    if (prop != null) player.SetProperty(id, prop.Value - 1);
                    break;

                case EmoteType.DecrementMyQuest:
                    break;

                case EmoteType.DecrementQuest:
                    // Used as part of the test drudge for events
                    break;

                case EmoteType.DeleteSelf:
                    sourceObject.CurrentLandblock?.RemoveWorldObject(sourceObject.Guid, false);
                    break;

                case EmoteType.DirectBroadcast:
                    text = Replace(emoteAction.Message, WorldObject, targetObject);
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                    break;

                case EmoteType.EraseMyQuest:
                    break;

                case EmoteType.EraseQuest:

                    if (player != null)
                        player.QuestManager.Erase(emoteAction.Message);
                    break;

                case EmoteType.FellowBroadcast:

                    text = emoteAction.Message;
                    if (player != null)
                    {
                        var fellowship = player.Fellowship;
                        if (fellowship == null)
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                        else
                        {
                            foreach (var fellow in fellowship.FellowshipMembers)
                                fellow.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                        }
                    }
                    break;

                case EmoteType.Generate:

                    uint wcid = (uint)emoteAction.WeenieClassId;
                    var item = WorldObjectFactory.CreateNewWorldObject((wcid));
                    break;

                case EmoteType.Give:

                    bool success = false;
                    if (player != null && emoteAction.WeenieClassId != null)
                    {
                        actionChain.AddAction(sourceObject, () =>
                        {
                            item = WorldObjectFactory.CreateNewWorldObject((uint)emoteAction.WeenieClassId);
                            var stackSize = emoteAction.StackSize ?? 1;
                            var stackMsg = "";
                            if (stackSize > 1)
                            {
                                item.StackSize = (ushort)stackSize;
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
                        });
                    }
                    break;

                case EmoteType.Goto:

                    var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);
                    var firstEmote = sourceObject.Biota.BiotaPropertiesEmote.FirstOrDefault(e => e.Category == (uint)EmoteCategory.GotoSet && rng < e.Probability);
                    var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(e => e.EmoteSetId == firstEmote.EmoteSetId && e.EmoteCategory == firstEmote.Category).ToList();
                    foreach (var action in actions)
                    {
                        actionChain.AddAction(player, () =>
                        {
                            ExecuteEmote(firstEmote, action, actionChain, sourceObject, targetObject);
                        });
                    }
                    break;

                case EmoteType.IncrementIntStat:

                    if (player == null || emoteAction.Stat == null) break;

                    id = (PropertyInt)emoteAction.Stat;
                    prop = player.GetProperty(id);
                    if (prop != null) player.SetProperty(id, prop.Value + 1);
                    break;

                case EmoteType.IncrementMyQuest:
                    break;

                case EmoteType.IncrementQuest:

                    if (player != null)
                        player.QuestManager.Increment(emoteAction.Message);
                    break;

                case EmoteType.InflictVitaePenalty:
                    if (player != null) player.VitaeCpPool++;   // TODO: full path
                    break;

                case EmoteType.InqAttributeStat:

                    if (targetCreature != null)
                    {
                        var attr = targetCreature.GetCreatureAttribute((PropertyAttribute)emoteAction.Stat);
                        success = attr != null && attr.Ranks >= emoteAction.Min && attr.Ranks <= emoteAction.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emoteAction, sourceObject, targetObject, actionChain);
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

                    var started = EventManager.IsEventStarted(emoteAction.Message);
                    InqCategory(started ? EmoteCategory.EventSuccess : EmoteCategory.EventFailure, emoteAction, sourceObject, targetObject, actionChain);
                    break;

                case EmoteType.InqFellowNum:
                    InqCategory(player != null && player.Fellowship != null ? EmoteCategory.TestSuccess : EmoteCategory.TestNoFellow, emoteAction, sourceObject, targetObject, actionChain);
                    break;

                case EmoteType.InqFellowQuest:
                    // focusing on 1 person quests to begin with
                    break;

                case EmoteType.InqFloatStat:
                    //InqProperty(target.GetProperty((PropertyFloat)emote.Stat), emote);
                    break;

                case EmoteType.InqInt64Stat:
                    //InqProperty(target.GetProperty((PropertyInt64)emote.Stat), emote);
                    break;

                case EmoteType.InqIntStat:

                    if (emoteAction.Stat != 25) break;  // ??

                    success = player.Level >= emoteAction.Min && player.Level <= emoteAction.Max;

                    // rng for failure case?
                    var useRNG = !success;

                    InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emoteAction, sourceObject, targetObject, actionChain, useRNG);
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

                    //if (player != null)
                        //InqCategory(player.Inventory.Count > 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
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
                        var hasQuest = player.QuestManager.HasQuest(emoteAction.Message);
                        InqCategory(hasQuest ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emoteAction, sourceObject, targetObject, actionChain);
                    }
                    break;

                case EmoteType.InqQuestBitsOff:
                    break;
                case EmoteType.InqQuestBitsOn:
                    break;
                case EmoteType.InqQuestSolves:

                    // should this be different from InqQuest?
                    if (player != null)
                    {
                        var hasQuest = player.QuestManager.HasQuest(emoteAction.Message);
                        InqCategory(hasQuest ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emoteAction, sourceObject, targetObject, actionChain);
                    }
                    break;

                case EmoteType.InqRawAttributeStat:

                    if (targetCreature != null)
                    {
                        var attr = targetCreature.GetCreatureAttribute((PropertyAttribute)emoteAction.Stat);
                        success = attr != null && attr.Base >= emoteAction.Min && attr.Base <= emoteAction.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emoteAction, sourceObject, targetObject, actionChain);
                    }
                    break;

                case EmoteType.InqRawSecondaryAttributeStat:

                    if (targetCreature != null)
                    {
                        var vital = targetCreature.GetCreatureVital((PropertyAttribute2nd)emoteAction.Stat);
                        success = vital != null && vital.Base >= emoteAction.Min && vital.Base <= emoteAction.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emoteAction, sourceObject, targetObject, actionChain);
                    }
                    break;

                case EmoteType.InqRawSkillStat:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emoteAction.Stat);
                        success = skill != null && skill.Base >= emoteAction.Min && skill.Base <= emoteAction.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emoteAction, sourceObject, targetObject, actionChain);
                    }
                    break;

                case EmoteType.InqSecondaryAttributeStat:

                    if (targetCreature != null)
                    {
                        var vital = targetCreature.GetCreatureVital((PropertyAttribute2nd)emoteAction.Stat);
                        success = vital != null && vital.Ranks >= emoteAction.Min && vital.Ranks <= emoteAction.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emoteAction, sourceObject, targetObject, actionChain);
                    }
                    break;

                case EmoteType.InqSkillSpecialized:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emoteAction.Stat);
                        //InqProperty(skill.Status == SkillStatus.Specialized, emoteAction);
                    }
                    break;

                case EmoteType.InqSkillStat:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emoteAction.Stat);
                        success = skill != null && skill.Ranks >= emoteAction.Min && skill.Ranks <= emoteAction.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emoteAction, sourceObject, targetObject, actionChain);
                    }
                    break;

                case EmoteType.InqSkillTrained:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emoteAction.Stat);
                        // TestNoQuality?
                        InqProperty(skill.Status == SkillStatus.Trained || skill.Status == SkillStatus.Specialized, emoteAction, sourceObject, targetObject, actionChain);
                    }
                    break;

                case EmoteType.InqStringStat:

                    //InqProperty(targetCreature.GetProperty((PropertyString)emoteAction.Stat), emote);
                    break;

                case EmoteType.InqYesNo:
                    ConfirmationManager.ProcessConfirmation((uint)emoteAction.Stat, true);
                    break;

                case EmoteType.Invalid:
                    break;

                case EmoteType.KillSelf:

                    if (targetCreature != null)
                        targetCreature.Smite(targetCreature.Guid);
                    break;

                case EmoteType.LocalBroadcast:
                    if (actionChain != null)
                    {
                        actionChain.AddDelaySeconds(emoteAction.Delay);
                        actionChain.AddAction(sourceObject, () =>
                        {
                            sourceObject.CurrentLandblock?.EnqueueBroadcast(sourceObject.Location, new GameMessageCreatureMessage(emoteAction.Message, sourceObject.Name, sourceObject.Guid.Full, ChatMessageType.Broadcast));
                        });
                    }
                    else
                        sourceObject.CurrentLandblock?.EnqueueBroadcast(sourceObject.Location, new GameMessageCreatureMessage(emoteAction.Message, sourceObject.Name, sourceObject.Guid.Full, ChatMessageType.Broadcast));
                    break;

                case EmoteType.LocalSignal:
                    break;

                case EmoteType.LockFellow:

                    if (player != null && player.Fellowship != null)
                        player.HandleActionFellowshipChangeOpenness(false);

                    break;

                case EmoteType.ForceMotion: // TODO: figure out the difference
                case EmoteType.Motion:

                    if (emote.Category != (uint)EmoteCategory.Vendor && emote.Style != null)
                    {
                        var startingMotion = new UniversalMotion((MotionStance)emote.Style, new MotionItem((MotionCommand)emote.Substyle));
                        var motion = new UniversalMotion((MotionStance)emote.Style, new MotionItem((MotionCommand)emoteAction.Motion, emoteAction.Extent));

                        if (sourceObject.CurrentMotionState.Stance != startingMotion.Stance)
                        {
                            if (sourceObject.CurrentMotionState.Stance == MotionStance.Invalid)
                            {
                                actionChain.AddDelaySeconds(emoteAction.Delay);
                                actionChain.AddAction(sourceObject, () =>
                                {
                                    sourceObject.DoMotion(startingMotion);
                                    sourceObject.CurrentMotionState = startingMotion;
                                });
                            }
                        }
                        else
                        {
                            if (sourceObject.CurrentMotionState.Commands.Count > 0 && sourceObject.CurrentMotionState.Commands[0].Motion == startingMotion.Commands[0].Motion)
                            {
                                actionChain.AddDelaySeconds(emoteAction.Delay);
                                actionChain.AddAction(sourceObject, () =>
                                {
                                    sourceObject.DoMotion(motion);
                                    sourceObject.CurrentMotionState = motion;
                                });
                                actionChain.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(sourceObject.MotionTableId).GetAnimationLength((MotionCommand)emoteAction.Motion));
                                if (motion.Commands[0].Motion != MotionCommand.Sleeping && motion.Commands[0].Motion != MotionCommand.Sitting) // this feels like it can be handled better, somehow?
                                {
                                    actionChain.AddAction(sourceObject, () =>
                                    {
                                        sourceObject.DoMotion(startingMotion);
                                        sourceObject.CurrentMotionState = startingMotion;
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        var motion = new UniversalMotion(MotionStance.Standing, new MotionItem((MotionCommand)emoteAction.Motion, emoteAction.Extent));

                        actionChain.AddDelaySeconds(emoteAction.Delay);
                        actionChain.AddAction(sourceObject, () =>
                        {
                            sourceObject.DoMotion(motion);
                            sourceObject.CurrentMotionState = motion;
                        });
                    }

                    break;

                case EmoteType.Move:

                    // what is the difference between this and MoveToPos?
                    // using MoveToPos logic for now...
                    if (targetCreature != null)
                    {
                        var currentPos = targetCreature.Location;

                        var newPos = new Position();
                        newPos.LandblockId = new LandblockId(currentPos.LandblockId.Raw);
                        newPos.Pos = new Vector3(emoteAction.OriginX ?? currentPos.Pos.X, emoteAction.OriginY ?? currentPos.Pos.Y, emoteAction.OriginZ ?? currentPos.Pos.Z);

                        if (emoteAction.AnglesX == null || emoteAction.AnglesY == null || emoteAction.AnglesZ == null || emoteAction.AnglesW == null)
                            newPos.Rotation = new Quaternion(currentPos.Rotation.X, currentPos.Rotation.Y, currentPos.Rotation.Z, currentPos.Rotation.W);
                        else
                            newPos.Rotation = new Quaternion(emoteAction.AnglesX ?? 0, emoteAction.AnglesY ?? 0, emoteAction.AnglesZ ?? 0, emoteAction.AnglesW ?? 1);

                        if (emoteAction.ObjCellId != null)
                            newPos.LandblockId = new LandblockId(emoteAction.ObjCellId.Value);

                        targetCreature.MoveTo(newPos, targetCreature.GetRunRate());
                    }
                    break;

                case EmoteType.MoveHome:

                    // TODO: call MoveToManager on server
                    if (targetCreature != null)
                        targetCreature.MoveTo(targetCreature.Home, targetCreature.GetRunRate());
                    break;

                case EmoteType.MoveToPos:

                    if (targetCreature != null)
                    {
                        var currentPos = targetCreature.Location;

                        var newPos = new Position();
                        newPos.LandblockId = new LandblockId(currentPos.LandblockId.Raw);
                        newPos.Pos = new Vector3(emoteAction.OriginX ?? currentPos.Pos.X, emoteAction.OriginY ?? currentPos.Pos.Y, emoteAction.OriginZ ?? currentPos.Pos.Z);

                        if (emoteAction.AnglesX == null || emoteAction.AnglesY == null || emoteAction.AnglesZ == null || emoteAction.AnglesW == null)
                            newPos.Rotation = new Quaternion(currentPos.Rotation.X, currentPos.Rotation.Y, currentPos.Rotation.Z, currentPos.Rotation.W);
                        else
                            newPos.Rotation = new Quaternion(emoteAction.AnglesX ?? 0, emoteAction.AnglesY ?? 0, emoteAction.AnglesZ ?? 0, emoteAction.AnglesW ?? 1);

                        if (emoteAction.ObjCellId != null)
                            newPos.LandblockId = new LandblockId(emoteAction.ObjCellId.Value);

                        targetCreature.MoveTo(newPos, targetCreature.GetRunRate());
                    }
                    break;

                case EmoteType.OpenMe:

                    sourceObject.Open(sourceObject);
                    break;

                case EmoteType.PetCastSpellOnOwner:

                    if (creature != null)
                        creature.CreateCreatureSpell(targetObject.Guid, (uint)emoteAction.SpellId);
                    break;

                case EmoteType.PhysScript:

                    // TODO: landblock broadcast
                    if (sourceObject != null)
                        sourceObject.PhysicsObj.play_script((PlayScript)emoteAction.PScript, 1.0f);
                    break;

                case EmoteType.PopUp:
                    ConfirmationManager.AddConfirmation(new Confirmation((ConfirmationType)emoteAction.Stat, emoteAction.Message, sourceObject.Guid.Full, targetObject.Guid.Full));
                    break;

                case EmoteType.RemoveContract:

                    if (player != null)
                        player.HandleActionAbandonContract((uint)emoteAction.Stat);
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

                    actionChain.AddDelaySeconds(emoteAction.Delay);
                    actionChain.AddAction(sourceObject, () =>
                    {
                        sourceObject.CurrentLandblock?.EnqueueBroadcast(sourceObject.Location, new GameMessageCreatureMessage(emoteAction.Message, sourceObject.Name, sourceObject.Guid.Full, ChatMessageType.Emote));
                    });
                    break;

                case EmoteType.SetAltRacialSkills:
                    break;

                case EmoteType.SetBoolStat:
                    targetObject.SetProperty((PropertyBool)emoteAction.Stat, emoteAction.Amount == 0 ? false : true);
                    break;

                case EmoteType.SetEyePalette:
                    if (creature != null)
                        creature.EyesPaletteDID = (uint)emoteAction.Display;
                    break;

                case EmoteType.SetEyeTexture:
                    if (creature != null)
                        creature.EyesTextureDID = (uint)emoteAction.Display;
                    break;

                case EmoteType.SetFloatStat:
                    targetObject.SetProperty((PropertyFloat)emoteAction.Stat, (float)emoteAction.Amount);
                    break;

                case EmoteType.SetHeadObject:
                    if (creature != null)
                        creature.HeadObjectDID = (uint)emoteAction.Display;
                    break;

                case EmoteType.SetHeadPalette:
                    break;

                case EmoteType.SetInt64Stat:
                    player.SetProperty((PropertyInt)emoteAction.Stat, (int)emoteAction.Amount);
                    break;

                case EmoteType.SetIntStat:
                    player.SetProperty((PropertyInt)emoteAction.Stat, (int)emoteAction.Amount);
                    break;

                case EmoteType.SetMouthPalette:
                    break;

                case EmoteType.SetMouthTexture:
                    creature = sourceObject as Creature;
                    if (creature != null)
                        creature.MouthTextureDID = (uint)emoteAction.Display;
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
                    creature = sourceObject as Creature;
                    if (creature != null)
                        creature.NoseTextureDID = (uint)emoteAction.Display;
                    break;

                case EmoteType.SetQuestBitsOff:
                    break;
                case EmoteType.SetQuestBitsOn:
                    break;
                case EmoteType.SetQuestCompletions:
                    break;
                case EmoteType.SetSanctuaryPosition:

                    //if (player != null)
                        //player.Sanctuary = emote.Position;
                    break;

                case EmoteType.Sound:
                    targetObject.CurrentLandblock?.EnqueueBroadcastSound(targetObject, (Sound)emoteAction.Sound);
                    break;

                case EmoteType.SpendLuminance:
                    if (player != null)
                        player.SpendLuminance((long)emoteAction.Amount);
                    break;

                case EmoteType.StampFellowQuest:
                    break;
                case EmoteType.StampMyQuest:
                    break;
                case EmoteType.StampQuest:

                    // work needs to be done here
                    if (player != null)
                        player.QuestManager.Add(emoteAction.Message);
                    break;

                case EmoteType.StartBarber:
                    break;

                case EmoteType.StartEvent:

                    EventManager.StartEvent(emoteAction.Message);
                    break;

                case EmoteType.StopEvent:

                    EventManager.StopEvent(emoteAction.Message);
                    break;

                case EmoteType.TakeItems:

                    if (player != null && emoteAction.WeenieClassId != null)
                    {
                        item = WorldObjectFactory.CreateNewWorldObject((uint)emoteAction.WeenieClassId);
                        if (item == null) break;

                        success = player.TryRemoveItemFromInventoryWithNetworking(item, (ushort)emoteAction.Amount);
                    }
                    break;

                case EmoteType.TeachSpell:

                    if (player != null)
                        player.LearnSpellWithNetworking((uint)emoteAction.SpellId);
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
                    actionChain.AddDelaySeconds(emoteAction.Delay);
                    actionChain.AddAction(sourceObject, () =>
                    {
                        player.Session.Network.EnqueueSend(new GameMessageHearDirectSpeech(sourceObject, emoteAction.Message, player, ChatMessageType.Tell));
                    });
                    break;

                case EmoteType.TellFellow:

                    text = emoteAction.Message;
                    if (player != null)
                    {
                        var fellowship = player.Fellowship;
                        if (fellowship == null)
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Tell));
                        else
                        {
                            foreach (var fellow in fellowship.FellowshipMembers)
                                fellow.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Tell));
                        }
                    }
                    break;

                case EmoteType.TextDirect:

                    if (player != null)
                    {
                        // should these delays be moved to 1 place??
                        actionChain.AddDelaySeconds(emoteAction.Delay);
                        text = emoteAction.Message;     // no known instances of replace tokens in current text, but could be added in future
                        actionChain.AddAction(player, () =>
                        {
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                        });
                    }
                    break;

                case EmoteType.Turn:

                    if (creature != null)
                    {
                        actionChain.AddDelaySeconds(emoteAction.Delay);
                        var pos = new Position(creature.Location.Cell, creature.Location.PositionX, creature.Location.PositionY, creature.Location.PositionZ, emoteAction.AnglesX ?? 0, emoteAction.AnglesY ?? 0, emoteAction.AnglesZ ?? 0, emoteAction.AnglesW ?? 0);
                        var rotateTime = 0.0f;
                        actionChain.AddAction(creature, () =>
                        {
                            rotateTime = creature.TurnTo(pos);
                        });
                        actionChain.AddDelaySeconds(rotateTime);
                    }
                    break;

                case EmoteType.TurnToTarget:

                    if (creature != null && targetCreature != null)
                    {
                        actionChain.AddDelaySeconds(emoteAction.Delay);
                        var rotateTime = 0.0f;
                        actionChain.AddAction(creature, () =>
                        {
                            rotateTime = creature.Rotate(targetCreature);
                        });
                        actionChain.AddDelaySeconds(rotateTime);
                    }
                    break;

                case EmoteType.UntrainSkill:

                    if (player != null)
                        player.UntrainSkill((Skill)emoteAction.Stat, 1);
                    break;

                case EmoteType.UpdateFellowQuest:
                    break;
                case EmoteType.UpdateMyQuest:
                    break;
                case EmoteType.UpdateQuest:

                    // only delay seems to be with test NPC here
                    // still, unsafe to use any emotes directly outside of a chain,
                    // as they could be executed out-of-order
                    if (player != null)
                    {
                        var questName = emoteAction.Message;
                        player.QuestManager.Add(questName);
                        var hasQuest = player.QuestManager.HasQuest(questName);
                        InqCategory(hasQuest ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emoteAction, sourceObject, targetObject, actionChain);
                    }
                    break;

                case EmoteType.WorldBroadcast:
                    if (player != null)
                    {
                        actionChain.AddDelaySeconds(emoteAction.Delay);
                        actionChain.AddAction(sourceObject, () =>
                        {
                            player.Session.Network.EnqueueSend(new GameMessageHearDirectSpeech(sourceObject, emoteAction.Message, player, ChatMessageType.WorldBroadcast));
                        });
                    }
                    break;

                default:
                    log.Debug($"EmoteManager.Execute - Encountered Unhandled EmoteType {(EmoteType)emoteAction.Type} for {sourceObject.Name} ({sourceObject.WeenieClassId})");
                    break;
            }
        }

        public void InqCategory(EmoteCategory categoryId, BiotaPropertiesEmoteAction emoteAction, WorldObject sourceObject, WorldObject targetObject, ActionChain actionChain, bool useRNG = false)
        {
            var rng = useRNG ? Physics.Common.Random.RollDice(0.0f, 1.0f) : 0.0f;

            var result = sourceObject.Biota.BiotaPropertiesEmote.FirstOrDefault(e => e.Category == (uint)categoryId && e.Quest == emoteAction.Message && rng <= e.Probability);
            if (result == null) return;
            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(a => a.EmoteSetId == result.EmoteSetId && a.EmoteCategory == result.Category);

            foreach (var action in actions)
            {
                actionChain.AddAction(sourceObject, () =>
                {
                    ExecuteEmote(result, action, actionChain, sourceObject, targetObject);
                });
            }
        }

        public void InqProperty(bool? prop, BiotaPropertiesEmoteAction emoteAction, WorldObject sourceObject, WorldObject targetObject, ActionChain actionChain)
        {
            var inRange = prop != null && prop.Value;
            InqPropertyInner(emoteAction, inRange, sourceObject, targetObject, actionChain);
        }

        public void InqProperty(int? prop, BiotaPropertiesEmoteAction emoteAction, WorldObject sourceObject, WorldObject targetObject, ActionChain actionChain)
        {
            var inRange = prop != null && (prop.Value >= emoteAction.Min && prop.Value <= emoteAction.Max);
            InqPropertyInner(emoteAction, inRange, sourceObject, targetObject, actionChain);
        }

        public void InqProperty(long? prop, BiotaPropertiesEmoteAction emoteAction, WorldObject sourceObject, WorldObject targetObject, ActionChain actionChain)
        {
            var inRange = prop != null && (prop.Value >= emoteAction.Min64 && prop.Value <= emoteAction.Max64);
            InqPropertyInner(emoteAction, inRange, sourceObject, targetObject, actionChain);
        }

        public void InqProperty(double? prop, BiotaPropertiesEmoteAction emoteAction, WorldObject sourceObject, WorldObject targetObject, ActionChain actionChain)
        {
            var inRange = prop != null && (prop.Value >= emoteAction.MinDbl && prop.Value <= emoteAction.MaxDbl);
            InqPropertyInner(emoteAction, inRange, sourceObject, targetObject, actionChain);
        }

        public void InqProperty(string prop, BiotaPropertiesEmoteAction emoteAction, WorldObject sourceObject, WorldObject targetObject, ActionChain actionChain)
        {
            if (prop == null) return;
            InqPropertyInner(emoteAction, true, sourceObject, targetObject, actionChain);
        }

        public void InqPropertyInner(BiotaPropertiesEmoteAction emoteAction, bool inRange, WorldObject sourceObject, WorldObject targetObject, ActionChain actionChain)
        {
            var category = inRange ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure;
            var useRNG = !inRange;  // ??
            InqCategory(category, emoteAction, sourceObject, targetObject, actionChain, useRNG);
        }

        public void DoVendorEmote(VendorType vendorType, WorldObject targetObject)
        {
            //Console.WriteLine("DoVendorEmote");

            var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);

            var emoteChain = new ActionChain();

            foreach (var emote in Emotes(EmoteCategory.Vendor).Where(x => x.VendorType == (int)vendorType))
            {
                if (rng < emote.Probability)
                {
                    foreach (var action in EmoteSet(emote.Category, emote.EmoteSetId))
                    {
                        ExecuteEmote(emote, action, emoteChain, WorldObject, targetObject);
                    }

                    break;
                }
            }

            foreach (var emote in Emotes(EmoteCategory.Vendor).Where(x => x.VendorType == (int)VendorType.Heartbeat))
            {
                if (rng < emote.Probability)
                {
                    foreach (var action in EmoteSet(emote.Category, emote.EmoteSetId))
                    {
                        ExecuteEmote(emote, action, emoteChain, WorldObject);
                    }

                    break;
                }
            }
            emoteChain.EnqueueChain();
        }

        public IEnumerable<BiotaPropertiesEmoteAction> EmoteSet(uint emoteCategory, uint emoteSetId)
        {
            return WorldObject.Biota.BiotaPropertiesEmoteAction.Where(x => x.EmoteCategory == emoteCategory && x.EmoteSetId == emoteSetId);
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

        public void HeartBeat()
        {
            var emoteChain = new ActionChain();

            var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);

            foreach (var emote in Emotes(EmoteCategory.HeartBeat))
            {
                if (rng < emote.Probability)
                {
                    foreach (var action in EmoteSet(emote.Category, emote.EmoteSetId))
                        ExecuteEmote(emote, action, emoteChain, WorldObject);

                    break;
                }
            }
            emoteChain.EnqueueChain();
        }
    }
}
