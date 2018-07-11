using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Physics.Animation;
using ACE.Server.WorldObjects;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.Managers
{
    public class EmoteManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WorldObject WorldObject;
        public double EndTime;
        public Queue<QueuedEmote> EmoteQueue;

        public EmoteManager(WorldObject worldObject)
        {
            WorldObject = worldObject;
        }

        public void Cancel()
        {
            EmoteQueue.Clear();
        }

        // Thanks to the original pioneers in the emulator community for help decoding these structures!
        public void Execute(Emote emote, WorldObject target)
        {
            var player = target is Player ? (Player)target : null;
            var creature = target is Creature ? (Creature)target : null;

            switch (emote.Type)
            {
                case EmoteType.Act:

                    if (player != null)
                        WorldObject.ActOnUse(player);
                    break;

                case EmoteType.Activate:

                    target.Activate(WorldObject);
                    break;

                case EmoteType.AddCharacterTitle:

                    if (player != null)
                        player.AddTitle((CharacterTitle)emote.Stat);
                    break;

                case EmoteType.AddContract:

                    if (player != null)
                        player.AddContract(emote.Stat);
                    break;

                case EmoteType.AdminSpam:

                    var text = Replace(emote.Message, target, WorldObject);
                    var players = WorldManager.GetAll();
                    foreach (var _player in players)
                        _player.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.AdminTell));
                    break;

                case EmoteType.AwardLevelProportionalSkillXP:

                    if (player != null)
                        player.GrantLevelProportionalSkillXP((Skill)emote.Stat, emote.Percent, emote.Max);
                    break;

                case EmoteType.AwardLevelProportionalXP:

                    if (player != null)
                        player.GrantLevelProportionalXp(emote.Percent, emote.Max);
                    break;

                case EmoteType.AwardLuminance:

                    if (player != null)
                        player.GrantLuminance((long)emote.Amount);
                    break;

                case EmoteType.AwardNoShareXP:

                    if (player != null)
                        player.EarnXP((long)emote.Amount, false);
                    break;

                case EmoteType.AwardSkillPoints:

                    if (player != null)
                        player.AwardSkillPoints((Skill)emote.Stat, (uint)emote.Amount);
                    break;

                case EmoteType.AwardSkillXP:

                    if (player != null)
                        player.RaiseSkillGameAction((Skill)emote.Stat, (uint)emote.Amount, true);
                    break;

                case EmoteType.AwardTrainingCredits:

                    if (player != null)
                        player.AddSkillCredits((int)emote.Amount, true);
                    break;

                case EmoteType.AwardXP:

                    if (player != null)
                        player.EarnXP((long)emote.Amount);
                    break;

                case EmoteType.BLog:
                    break;

                case EmoteType.CastSpell:

                    if (WorldObject is Player)
                        (WorldObject as Player).CreatePlayerSpell(emote.SpellId);

                    else if (WorldObject is Creature)
                        (WorldObject as Creature).CreateCreatureSpell(target.Guid, emote.SpellId);

                    break;

                case EmoteType.CastSpellInstant:

                    if (WorldObject is Player)
                        (WorldObject as Player).CreatePlayerSpell(emote.SpellId);

                    else if (WorldObject is Creature)
                        (WorldObject as Creature).CreateCreatureSpell(target.Guid, emote.SpellId);

                    break;

                case EmoteType.CloseMe:

                    target.Close(WorldObject);
                    break;

                case EmoteType.CreateTreasure:
                    break;

                case EmoteType.DecrementIntStat:

                    var id = (PropertyInt)emote.Stat;
                    var prop = target.GetProperty(id);
                    if (prop != null) target.SetProperty(id, prop.Value - 1);
                    break;

                case EmoteType.DecrementMyQuest:
                    break;
                case EmoteType.DecrementQuest:
                    break;
                case EmoteType.DeleteSelf:

                    WorldObject.CurrentLandblock?.RemoveWorldObject(WorldObject.Guid, false);
                    break;

                case EmoteType.DirectBroadcast:

                    text = Replace(emote.Message, target, WorldObject);
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                    break;

                case EmoteType.EraseMyQuest:
                    break;
                case EmoteType.EraseQuest:
                    break;
                case EmoteType.FellowBroadcast:

                    text = Replace(emote.Message, target, WorldObject);
                    if (player != null)
                    {
                        var fellowship = player.Fellowship;
                        if (fellowship != null)
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                        else
                        {
                            foreach (var fellow in fellowship.FellowshipMembers)
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                        }
                    }
                    break;

                case EmoteType.ForceMotion:

                    WorldObject.PhysicsObj.DoMotion(emote.Motion, new MovementParameters());
                    break;

                case EmoteType.Generate:

                    var wcid = emote.CreateProfile.ClassID;
                    var item = WorldObjectFactory.CreateNewWorldObject(wcid);
                    break;

                case EmoteType.Give:

                    if (player != null)
                    {
                        wcid = emote.CreateProfile.ClassID;
                        item = WorldObjectFactory.CreateNewWorldObject(wcid);
                        if (item == null)
                            break;

                        var success = player.TryAddToInventory(item);
                    }
                    break;

                case EmoteType.Goto:
                    InqCategory(EmoteCategory.GotoSet, emote);
                    break;

                case EmoteType.IncrementIntStat:

                    id = (PropertyInt)emote.Stat;
                    prop = target.GetProperty(id);
                    if (prop != null) target.SetProperty(id, prop.Value + 1);
                    break;

                case EmoteType.IncrementMyQuest:
                    break;
                case EmoteType.IncrementQuest:
                    break;
                case EmoteType.InflictVitaePenalty:

                    if (player != null) player.VitaeCpPool++;
                    break;

                case EmoteType.InqAttributeStat:

                    if (creature != null)
                    {
                        var attr = creature.GetCreatureAttribute((PropertyAttribute)emote.Stat);
                        var success = attr != null && attr.Ranks >= emote.Min && attr.Ranks <= emote.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqBoolStat:
                    InqProperty(target.GetProperty((PropertyBool)emote.Stat), emote);
                    break;

                case EmoteType.InqContractsFull:

                    if (player != null)
                    {
                        var contracts = player.TrackedContracts;
                        InqCategory(contracts.Count != 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqEvent:

                    var started = EventManager.IsEventStarted(emote.Message);
                    InqCategory(started ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    break;

                case EmoteType.InqFellowNum:
                    InqCategory(player != null && player.Fellowship != null ? EmoteCategory.TestSuccess : EmoteCategory.TestNoFellow, emote);
                    break;

                case EmoteType.InqFellowQuest:
                    break;

                case EmoteType.InqFloatStat:
                    InqProperty(target.GetProperty((PropertyFloat)emote.Stat), emote);
                    break;

                case EmoteType.InqInt64Stat:
                    InqProperty(target.GetProperty((PropertyInt64)emote.Stat), emote);
                    break;

                case EmoteType.InqIntStat:
                    InqProperty(target.GetProperty((PropertyInt)emote.Stat), emote);
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

                    if (player != null)
                        InqCategory(player.NumCharacterTitles != 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    break;

                case EmoteType.InqOwnsItems:

                    if (player != null)
                        InqCategory(player.Inventory.Count > 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    break;

                case EmoteType.InqPackSpace:

                    if (player != null)
                    {
                        var freeSpace = player.ContainerCapacity > player.ItemCapacity;
                        InqCategory(freeSpace ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqQuest:
                    break;
                case EmoteType.InqQuestBitsOff:
                    break;
                case EmoteType.InqQuestBitsOn:
                    break;
                case EmoteType.InqQuestSolves:
                    break;
                case EmoteType.InqRawAttributeStat:

                    if (creature != null)
                    {
                        var attr = creature.GetCreatureAttribute((PropertyAttribute)emote.Stat);
                        var success = attr != null && attr.Base >= emote.Min && attr.Base <= emote.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqRawSecondaryAttributeStat:

                    if (creature != null)
                    {
                        var vital = creature.GetCreatureVital((PropertyAttribute2nd)emote.Stat);
                        var success = vital != null && vital.Base >= emote.Min && vital.Base <= emote.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqRawSkillStat:

                    if (creature != null)
                    {
                        var skill = creature.GetCreatureSkill((Skill)emote.Stat);
                        var success = skill != null && skill.Base >= emote.Min && skill.Base <= emote.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqSecondaryAttributeStat:

                    if (creature != null)
                    {
                        var vital = creature.GetCreatureVital((PropertyAttribute2nd)emote.Stat);
                        var success = vital != null && vital.Ranks >= emote.Min && vital.Ranks <= emote.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqSkillSpecialized:

                    if (creature != null)
                    {
                        var skill = creature.GetCreatureSkill((Skill)emote.Stat);
                        InqProperty(skill.Status == SkillStatus.Specialized, emote);
                    }
                    break;

                case EmoteType.InqSkillStat:

                    if (creature != null)
                    {
                        var skill = creature.GetCreatureSkill((Skill)emote.Stat);
                        var success = skill != null && skill.Ranks >= emote.Min && skill.Ranks <= emote.Max;
                        InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqSkillTrained:

                    if (creature != null)
                    {
                        var skill = creature.GetCreatureSkill((Skill)emote.Stat);
                        InqProperty(skill.Status == SkillStatus.Trained || skill.Status == SkillStatus.Specialized, emote);
                    }
                    break;

                case EmoteType.InqStringStat:

                    InqProperty(target.GetProperty((PropertyString)emote.Stat), emote);
                    break;

                case EmoteType.InqYesNo:
                    ConfirmationManager.ProcessConfirmation(emote.Stat, true);
                    break;

                case EmoteType.Invalid:
                    break;
                case EmoteType.KillSelf:

                    if (player != null)
                        player.Smite(WorldObject.Guid);
                    break;

                case EmoteType.LocalBroadcast:

                    text = Replace(emote.Message, target, WorldObject);
                    WorldObject.CurrentLandblock?.EnqueueBroadcastSystemChat(WorldObject, text, ChatMessageType.Broadcast);
                    break;

                case EmoteType.LocalSignal:
                    break;

                case EmoteType.LockFellow:

                    if (player != null && player.Fellowship != null)
                        player.HandleActionFellowshipChangeOpenness(false);
                    break;

                case EmoteType.Motion:

                    WorldObject.PhysicsObj.DoMotion(emote.Motion, new MovementParameters());
                    break;

                case EmoteType.Move:

                    if (creature != null)
                    {
                        var movement = creature.PhysicsObj.MovementManager.MoveToManager;
                        movement.MoveToPosition(new Physics.Common.Position(emote.Position), new MovementParameters());
                    }
                    break;

                case EmoteType.MoveHome:

                    if (creature != null)
                    {
                        var movement = creature.PhysicsObj.MovementManager.MoveToManager;
                        movement.MoveToPosition(new Physics.Common.Position(creature.Home), new MovementParameters());
                    }
                    break;

                case EmoteType.MoveToPos:

                    if (creature != null)
                    {
                        var movement = creature.PhysicsObj.MovementManager.MoveToManager;
                        movement.MoveToPosition(new Physics.Common.Position(emote.Position), new MovementParameters());
                    }
                    break;

                case EmoteType.OpenMe:

                    target.Open(WorldObject);
                    break;

                case EmoteType.PetCastSpellOnOwner:

                    if (WorldObject is Creature)
                        (WorldObject as Creature).CreateCreatureSpell(target.Guid, emote.SpellId);
                    break;

                case EmoteType.PhysScript:

                    target.PhysicsObj.play_script(emote.PScript, 1.0f);
                    break;

                case EmoteType.PopUp:
                    ConfirmationManager.AddConfirmation(new Confirmation((ConfirmationType)emote.Stat, emote.Message, WorldObject.Guid.Full, target.Guid.Full));
                    break;

                case EmoteType.RemoveContract:

                    if (player != null)
                        player.HandleActionAbandonContract(emote.Stat);
                    break;

                case EmoteType.RemoveVitaePenalty:

                    if (player != null) player.VitaeCpPool = 0;
                    break;

                case EmoteType.ResetHomePosition:

                    if (creature != null)
                        target.Home = emote.Position;
                    break;

                case EmoteType.Say:

                    text = Replace(emote.Message, target, WorldObject);
                    if (player != null)
                        player.CurrentLandblock?.EnqueueBroadcastLocalChat(player, text);
                    break;

                case EmoteType.SetAltRacialSkills:
                    break;

                case EmoteType.SetBoolStat:
                    target.SetProperty((PropertyBool)emote.Stat, emote.Amount == 0 ? false : true);
                    break;

                case EmoteType.SetEyePalette:

                    if (creature != null)
                        creature.EyesPaletteDID = (uint)emote.Display;
                    break;

                case EmoteType.SetEyeTexture:

                    if (creature != null)
                        creature.EyesTextureDID = (uint)emote.Display;
                    break;

                case EmoteType.SetFloatStat:
                    target.SetProperty((PropertyFloat)emote.Stat, (float)emote.Amount);
                    break;

                case EmoteType.SetHeadObject:

                    if (creature != null)
                        creature.HeadObjectDID = (uint)emote.Display;
                    break;

                case EmoteType.SetHeadPalette:
                    break;

                case EmoteType.SetInt64Stat:
                    target.SetProperty((PropertyInt)emote.Stat, (int)emote.Amount);
                    break;

                case EmoteType.SetIntStat:
                    target.SetProperty((PropertyInt)emote.Stat, (int)emote.Amount);
                    break;

                case EmoteType.SetMouthPalette:
                    break;

                case EmoteType.SetMouthTexture:

                    if (creature != null)
                        creature.MouthTextureDID = (uint)emote.Display;
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

                    if (creature != null)
                        creature.NoseTextureDID = (uint)emote.Display;
                    break;

                case EmoteType.SetQuestBitsOff:
                    break;
                case EmoteType.SetQuestBitsOn:
                    break;
                case EmoteType.SetQuestCompletions:
                    break;
                case EmoteType.SetSanctuaryPosition:

                    if (player != null)
                        player.Sanctuary = emote.Position;
                    break;

                case EmoteType.Sound:
                    target.CurrentLandblock?.EnqueueBroadcastSound(target, (Sound)emote.Sound);
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
                        wcid = emote.CreateProfile.ClassID;
                        item = WorldObjectFactory.CreateNewWorldObject(wcid);
                        if (item == null) break;

                        var success = player.TryRemoveItemFromInventoryWithNetworking(item, (ushort)emote.Amount);
                    }
                    break;

                case EmoteType.TeachSpell:

                    if (player != null)
                        player.LearnSpellWithNetworking(emote.SpellId);
                    break;

                case EmoteType.TeleportSelf:

                    if (WorldObject is Player)
                        (WorldObject as Player).Teleport(emote.Position);
                    break;

                case EmoteType.TeleportTarget:

                    if (player != null)
                        player.Teleport(emote.Position);
                    break;

                case EmoteType.Tell:

                    text = Replace(emote.Message, target, WorldObject);
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Tell));
                    break;

                case EmoteType.TellFellow:

                    text = Replace(emote.Message, target, WorldObject);
                    if (player != null)
                    {
                        var fellowship = player.Fellowship;
                        if (fellowship != null)
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Tell));
                        else
                        {
                            foreach (var fellow in fellowship.FellowshipMembers)
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Tell));
                        }
                    }
                    break;

                case EmoteType.TextDirect:

                    text = Replace(emote.Message, target, WorldObject);
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.AdminTell));
                    break;

                case EmoteType.Turn:

                    var mvp = new MovementParameters();
                    mvp.DesiredHeading = new AFrame(emote.Frame).get_heading();

                    // increment animation sequence
                    WorldObject.PhysicsObj.LastMoveWasAutonomous = false;
                    WorldObject.PhysicsObj.cancel_moveto();
                    WorldObject.PhysicsObj.TurnToHeading(mvp);
                    break;

                case EmoteType.TurnToTarget:

                    WorldObject.PhysicsObj.TurnToObject(target.Guid.Full, new MovementParameters());
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
                    break;
                case EmoteType.WorldBroadcast:

                    text = Replace(emote.Message, target, WorldObject);
                    players = WorldManager.GetAll();
                    foreach (var _player in players)
                        _player.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.WorldBroadcast));
                    break;
            }
        }

        public void ExecuteSet(EmoteSet emoteSet, WorldObject target)
        {
            if (EndTime < Timer.CurrentTime)
                EndTime = Timer.CurrentTime;

            double queueTime = 0.0;
            foreach (var emote in emoteSet.Emotes)
            {
                queueTime += emote.Delay;
                EmoteQueue.Enqueue(new QueuedEmote(emote, target, Timer.CurrentTime + queueTime));
            }
        }

        public bool InProgress()
        {
            return EmoteQueue.Count > 0;
        }

        public void OnDeath(uint sourceID)
        {
            Cancel();
        }

        public string Replace(string message, WorldObject target, WorldObject source)
        {
            var result = message;

            result = result.Replace("%s", target.Name);
            result = result.Replace("%tn", target.Name);
            result = result.Replace("%n", source.Name);
            result = result.Replace("%mn", source.Name);
            result = result.Replace("%tqt", "some amount of time");

            return result;
        }

        public void InqCategory(EmoteCategory categoryId, Emote emote)
        {
            var category = WorldObject.Biota.GetEmotes((uint)categoryId);
            if (category == null) return;
            foreach (var entry in category)
            {
                if (!emote.Message.Equals(entry.Quest))
                    continue;
                //ExecuteSet(entry, target);
                break;
            }
        }

        public void InqProperty(bool? prop, Emote emote)
        {
            if (prop == null) return;
            InqPropertyInner(emote, true);
        }

        public void InqProperty(long? prop, Emote emote)
        {
            if (prop == null) return;
            var inRange = prop.Value >= (int)emote.Min && prop.Value <= (int)emote.Max;
            InqPropertyInner(emote, inRange);
        }

        public void InqProperty(double? prop, Emote emote)
        {
            if (prop == null) return;
            var inRange = prop.Value >= (int)emote.MinFloat && prop.Value <= (int)emote.MaxFloat;
            InqPropertyInner(emote, inRange);
        }

        public void InqProperty(string prop, Emote emote)
        {
            if (prop == null) return;
            InqPropertyInner(emote, true);
        }

        public void InqPropertyInner(Emote emote, bool inRange)
        {
            var category = WorldObject.Biota.GetEmotes(inRange ? (uint)EmoteCategory.TestSuccess : (uint)EmoteCategory.TestFailure);
            if (category == null) return;
            foreach (var entry in category)
            {
                if (!emote.Message.Equals(entry.Quest))
                    continue;
                //ExecuteSet(entry, target);
                break;
            }
        }

        public void HeartBeat()
        {
            var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);

            var emoteChain = new ActionChain();

            //if (!(WorldObject is Vendor))
            //{
            foreach (var emote in Emotes(EmoteCategory.HeartBeat))
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
            //}
            //else
            //{
            //    foreach (var emote in Emotes(EmoteCategory.Vendor).Where(x => x.VendorType == (int)VendorType.Heartbeat))
            //    {
            //        if (rng < emote.Probability)
            //        {
            //            foreach (var action in EmoteSet(emote.Category, emote.EmoteSetId))
            //            {
            //                ExecuteEmote(emote, action, emoteChain, WorldObject);
            //            }

            //            break;
            //        }
            //    }
            //}

            if (emoteChain != null && emoteChain.FirstElement != null)
            {
                emoteChain.EnqueueChain();
            }
        }

        public void DoVendorEmote(VendorType vendorType, WorldObject targetObject)
        {
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

            if (emoteChain != null && emoteChain.FirstElement != null)
            {
                emoteChain.EnqueueChain();
            }
        }

        public void ExecuteEmote(BiotaPropertiesEmote emote, BiotaPropertiesEmoteAction emoteAction, ActionChain actionChain, WorldObject sourceObject = null, WorldObject targetObject = null)
        {
            var player = targetObject as Player;
            QuestManager qManager = new QuestManager(sourceObject);
            switch ((EmoteType)emoteAction.Type)
            {

                case EmoteType.Activate:
                    var creature = sourceObject is Creature ? (Creature)sourceObject : null;
                    actionChain.AddDelaySeconds(emoteAction.Delay);
                    actionChain.AddAction(sourceObject, () =>
                    {
                        if ((creature.ActivationTarget ?? 0) > 0)
                        {
                            var activationTarget = creature.CurrentLandblock?.GetObject(new ObjectGuid(creature.ActivationTarget ?? 0));
                            activationTarget.ActOnUse(creature);
                        }
                    });
                    break;

                case EmoteType.AddCharacterTitle:

                    if (player != null)
                        player.AddTitle((CharacterTitle)emoteAction.Stat);
                    break;

                case EmoteType.AddContract:

                    //if (player != null)
                    ////Contracts werent in emote table
                    ////player.AddContract(emoteAction.Stat);
                    break;

                case EmoteType.AdminSpam:

                    var text = emoteAction.Message;
                    var players = WorldManager.GetAll();
                    foreach (var _player in players)
                        _player.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.AdminTell));
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
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You've earned " + emoteAction.Amount64 + " experience.", ChatMessageType.System));
                        }
                    });
                    break;

                case EmoteType.AwardSkillPoints:

                    if (player != null)
                        player.AwardSkillPoints((Skill)emoteAction.Stat, (uint)emoteAction.Amount);
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
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat("You've earned " + emoteAction.Amount64 + " experience.", ChatMessageType.System));
                        }
                    });
                    break;

                case EmoteType.BLog:
                    ////Only one test drudge used this emoteAction.
                    break;

                case EmoteType.CastSpell:

                    if (WorldObject is Player)
                        (WorldObject as Player).CreatePlayerSpell((uint)emoteAction.SpellId);

                    else if (WorldObject is Creature)
                        (WorldObject as Creature).CreateCreatureSpell(player.Guid, (uint)emoteAction.SpellId);

                    break;

                case EmoteType.CastSpellInstant:
                    var creature2 = sourceObject is Creature ? (Creature)sourceObject : null;
                    SpellTable spellTable = DatManager.PortalDat.SpellTable;
                    SpellBase spell = spellTable.Spells[(uint)emoteAction.SpellId];
                    actionChain.AddAction(sourceObject, () =>
                    {
                        if (spell.TargetEffect > 0)
                        {
                            Console.WriteLine("Should be casting " + (uint)emoteAction.SpellId + " here. TargetEffect was greater than 0 (This has yet to be added from a magic standpoint");
                            creature2.CreateCreatureSpell(targetObject.Guid, (uint)emoteAction.SpellId);
                        }
                        else
                        {
                            creature2.CreateCreatureSpell((uint)emoteAction.SpellId);
                        }
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
                    ////Used as part of the test drudge for events
                    break;
                case EmoteType.DeleteSelf:
                    sourceObject.CurrentLandblock?.RemoveWorldObject(sourceObject.Guid, false);
                    break;

                case EmoteType.DirectBroadcast:
                    text = Replace(emoteAction.Message, targetObject, WorldObject);
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                    break;

                case EmoteType.EraseMyQuest:
                    break;
                case EmoteType.EraseQuest:
                    qManager.Erase(emoteAction.Message, player);
                    break;
                case EmoteType.FellowBroadcast:

                    text = emoteAction.Message;
                    if (player != null)
                    {
                        var fellowship = player.Fellowship;
                        if (fellowship != null)
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                        else
                        {
                            foreach (var fellow in fellowship.FellowshipMembers)
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                        }
                    }
                    break;

                case EmoteType.ForceMotion:

                    sourceObject.PhysicsObj.DoMotion((uint)emoteAction.Motion, new MovementParameters());
                    break;

                case EmoteType.Generate:

                    uint wcid = (uint)emoteAction.WeenieClassId;
                    var item = WorldObjectFactory.CreateNewWorldObject((wcid));
                    break;

                case EmoteType.Give:
                    actionChain.AddAction(sourceObject, () =>
                    {
                        if (player != null)
                        {
                            uint weenie = (uint)emoteAction.WeenieClassId;
                            WorldObject item2 = WorldObjectFactory.CreateNewWorldObject(weenie);
                            if (emoteAction.WeenieClassId != null)
                            {
                                if (emoteAction.StackSize > 1)
                                {
                                    item2.StackSize = (ushort)emoteAction.StackSize;
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(WorldObject.Name + " gives you " + emoteAction.StackSize + " " + item2.Name + ".", ChatMessageType.System));
                                }
                                else
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(WorldObject.Name + " gives you " + item2.Name + ".", ChatMessageType.System));
                                var success = player.TryCreateInInventoryWithNetworking(item2);
                            }
                        }
                    });
                    break;

                case EmoteType.Goto:
                    var rng2 = Physics.Common.Random.RollDice(0.0f, 1.0f);
                    var result2 = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 32 && rng2 <= emote2.Probability);
                    var actions2 = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result2.FirstOrDefault().EmoteSetId && action.EmoteCategory == result2.FirstOrDefault().Category);
                    foreach (var action in actions2)
                    {
                        actionChain.AddAction(player, () =>
                        {
                            ExecuteEmote(result2.FirstOrDefault(), action, actionChain, sourceObject, player);
                        });
                    }
                    break;

                case EmoteType.IncrementIntStat:

                    id = (PropertyInt)emoteAction.Stat;
                    prop = player.GetProperty(id);
                    if (prop != null) player.SetProperty(id, prop.Value + 1);
                    break;

                case EmoteType.IncrementMyQuest:
                    break;
                case EmoteType.IncrementQuest:
                    qManager.Increment(emoteAction.Message, player);
                    break;
                case EmoteType.InflictVitaePenalty:

                    if (player != null) player.VitaeCpPool++;
                    break;

                case EmoteType.InqAttributeStat:

                    if (player != null)
                    {
                        var attr = player.GetCreatureAttribute((PropertyAttribute)emoteAction.Stat);
                        var success = attr != null && attr.Ranks >= emoteAction.Min && attr.Ranks <= emoteAction.Max;
                        ///InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emoteAction);
                        if (success)
                        {
                            var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 22 && emote2.Quest == emoteAction.Message);
                            if (result.Count() > 0)
                            {
                                var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                                foreach (var action in actions)
                                {
                                    actionChain.AddAction(player, () =>
                                    {
                                        ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                    });
                                }
                            }
                        }
                        else
                        {
                            var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 23 && emote2.Quest == emoteAction.Message);
                            if (result.Count() > 0)
                            {
                                var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                                foreach (var action in actions)
                                {
                                    actionChain.AddAction(player, () =>
                                    {
                                        ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                    });
                                }
                            }
                        }
                    }
                    break;

                case EmoteType.InqBoolStat:
                    ///This only is used with npc's 24944 and 6386, which are dev tester npc's. Not worth the current effort.
                    break;

                case EmoteType.InqContractsFull:
                    ////Not part of the game at PY16?
                    //if (player != null)
                    //{
                    //    var contracts = player.TrackedContracts;
                    //    InqCategory(contracts.Count != 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    //}
                    break;

                case EmoteType.InqEvent:

                    var started = EventManager.IsEventStarted(emoteAction.Message);
                    //InqCategory(started ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    if (started)
                    {
                        var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 27 && emote2.Quest == emoteAction.Message);
                        if (result.Count() > 0)
                        {
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                        }
                    }
                    else
                    {
                        var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 28 && emote2.Quest == emoteAction.Message);
                        if (result.Count() > 0)
                        {
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                        }
                    }
                    break;

                case EmoteType.InqFellowNum:
                    //InqCategory(player != null && player.Fellowship != null ? EmoteCategory.TestSuccess : EmoteCategory.TestNoFellow, emote);
                    break;

                case EmoteType.InqFellowQuest:
                    //focussing on 1 person quests to begin with
                    break;

                case EmoteType.InqFloatStat:
                    //InqProperty(target.GetProperty((PropertyFloat)emote.Stat), emote);
                    break;

                case EmoteType.InqInt64Stat:
                    //InqProperty(target.GetProperty((PropertyInt64)emote.Stat), emote);
                    break;

                case EmoteType.InqIntStat:
                    if (emoteAction.Stat == 25)
                    {

                        if (player.Level >= emoteAction.Min && player.Level <= emoteAction.Max)
                        {
                            ////Test Success Emote Category
                            var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 22 && emote2.Quest == emoteAction.Message);
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                            //actionChain.EnqueueChain();
                        }
                        else
                        {
                            /////Test Failure Emote Category
                            var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);
                            var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 23 && rng <= emote2.Probability);
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);
                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                            //actionChain.EnqueueChain();
                        }
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

                    ////if (player != null)
                    //// InqCategory(player.NumCharacterTitles != 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    break;

                case EmoteType.InqOwnsItems:

                    ////if (player != null)
                    //// InqCategory(player.Inventory.Count > 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    break;

                case EmoteType.InqPackSpace:

                    //if (player != null)
                    //{
                    //    var freeSpace = player.ContainerCapacity > player.ItemCapacity;
                    //    InqCategory(freeSpace ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    //}
                    break;

                case EmoteType.InqQuest:

                    if (qManager.Inquiry(emoteAction.Message, player))
                    {
                        var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 12 && emote2.Quest == emoteAction.Message);
                        if (result.Count() < 1)
                        {
                            result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 12 && emote2.Quest == emoteAction.Message);
                        }
                        if (result.Count() > 0)
                        {
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                        }
                    }
                    else
                    {
                        var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 13 && emote2.Quest == emoteAction.Message);
                        if (result.Count() > 0)
                        {
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                        }
                    }
                    break;
                case EmoteType.InqQuestBitsOff:
                    break;
                case EmoteType.InqQuestBitsOn:
                    break;
                case EmoteType.InqQuestSolves:

                    if (qManager.Inquiry(emoteAction.Message, player))
                    {
                        var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 12 && emote2.Quest == emoteAction.Message);
                        if (result.Count() < 1)
                        {
                            result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 12 && emote2.Quest == emoteAction.Message);
                        }
                        if (result.Count() > 0)
                        {
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                        }
                        //actionChain.EnqueueChain();
                    }
                    else
                    {
                        var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 13 && emote2.Quest == emoteAction.Message);
                        if (result.Count() > 0)
                        {
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                        }
                    }
                    break;
                case EmoteType.InqRawAttributeStat:

                    if (player != null)
                    {
                        var attr = player.GetCreatureAttribute((PropertyAttribute)emoteAction.Stat);
                        var success = attr != null && attr.Base >= emoteAction.Min && attr.Base <= emoteAction.Max;
                        //InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqRawSecondaryAttributeStat:

                    if (player != null)
                    {
                        var vital = player.GetCreatureVital((PropertyAttribute2nd)emoteAction.Stat);
                        var success = vital != null && vital.Base >= emoteAction.Min && vital.Base <= emoteAction.Max;
                        // InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqRawSkillStat:

                    if (player != null)
                    {
                        var skill = player.GetCreatureSkill((Skill)emoteAction.Stat);
                        var success = skill != null && skill.Base >= emoteAction.Min && skill.Base <= emoteAction.Max;
                        //InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqSecondaryAttributeStat:

                    if (player != null)
                    {
                        var vital = player.GetCreatureVital((PropertyAttribute2nd)emoteAction.Stat);
                        var success = vital != null && vital.Ranks >= emoteAction.Min && vital.Ranks <= emoteAction.Max;
                        //InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqSkillSpecialized:

                    if (player != null)
                    {
                        var skill = player.GetCreatureSkill((Skill)emoteAction.Stat);
                        //InqProperty(skill.Status == SkillStatus.Specialized, emote);
                    }
                    break;

                case EmoteType.InqSkillStat:

                    if (player != null)
                    {
                        var skill = player.GetCreatureSkill((Skill)emoteAction.Stat);
                        var success = skill != null && skill.Ranks >= emoteAction.Min && skill.Ranks <= emoteAction.Max;
                        //InqCategory(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    }
                    break;

                case EmoteType.InqSkillTrained:

                    if (player != null)
                    {
                        var skill = player.GetCreatureSkill((Skill)emoteAction.Stat);
                        //InqProperty(skill.Status == SkillStatus.Trained || skill.Status == SkillStatus.Specialized, emote);
                    }
                    break;

                case EmoteType.InqStringStat:

                    ///InqProperty(player.GetProperty((PropertyString)emoteAction.Stat), emote);
                    break;

                case EmoteType.InqYesNo:
                    ConfirmationManager.ProcessConfirmation((uint)emoteAction.Stat, true);
                    break;

                case EmoteType.Invalid:
                    break;
                case EmoteType.KillSelf:

                    if (player != null)
                        player.Smite(player.Guid);
                    break;

                case EmoteType.LocalBroadcast:
                    if (actionChain == null)
                    {
                        sourceObject.CurrentLandblock?.EnqueueBroadcast(sourceObject.Location, new GameMessageCreatureMessage(emoteAction.Message, sourceObject.Name, sourceObject.Guid.Full, ChatMessageType.Broadcast));
                    }
                    ///text = emoteAction.Message;
                    ///WorldObject.CurrentLandblock?.EnqueueBroadcastSystemChat(player, text, ChatMessageType.Broadcast);
                    else
                    {
                        actionChain.AddDelaySeconds(emoteAction.Delay);
                        actionChain.AddAction(sourceObject, () =>
                        {
                            sourceObject.CurrentLandblock?.EnqueueBroadcast(sourceObject.Location, new GameMessageCreatureMessage(emoteAction.Message, sourceObject.Name, sourceObject.Guid.Full, ChatMessageType.Broadcast));
                        });
                    }
                    break;

                case EmoteType.LocalSignal:
                    break;

                case EmoteType.LockFellow:

                    if (player != null && player.Fellowship != null)
                        player.HandleActionFellowshipChangeOpenness(false);

                    break;

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

                    if (sourceObject != null)
                    {
                        //var movement = sourceObject.PhysicsObj.MovementManager.MoveToManager;
                        //movement.MoveToPosition(new Physics.Common.Position(), new MovementParameters());
                        ////This needs wortk.
                    }
                    break;

                case EmoteType.MoveHome:
                    //creature = sourceObject as Creature;
                    //if (creature != null)
                    //{
                    //    var movement = creature.PhysicsObj.MovementManager.MoveToManager;
                    //    movement.MoveToPosition(new Physics.Common.Position(creature.Home), new MovementParameters());
                    //}
                    break;

                case EmoteType.MoveToPos:
                    //creature = sourceObject as Creature;
                    //if (creature != null)
                    //{
                    //    Physics.Common.Position pos2 = new Physics.Common.Position((uint)emoteAction.OriginX, (uint)emoteAction.OriginY, (uint)emoteAction.OriginZ);
                    //    var movement = creature.PhysicsObj.MovementManager.MoveToManager;
                    //    movement.MoveToPosition(new Physics.Common.Position(emoteAction.Position), new MovementParameters());
                    //}
                    break;

                case EmoteType.OpenMe:

                    sourceObject.Open(sourceObject);
                    break;

                case EmoteType.PetCastSpellOnOwner:

                    if (sourceObject is Creature)
                        (sourceObject as Creature).CreateCreatureSpell(targetObject.Guid, (uint)emoteAction.SpellId);
                    break;

                case EmoteType.PhysScript:

                    targetObject.PhysicsObj.play_script((PlayScript)emoteAction.PScript, 1.0f);
                    break;

                case EmoteType.PopUp:
                    ConfirmationManager.AddConfirmation(new Confirmation((ConfirmationType)emoteAction.Stat, emoteAction.Message, sourceObject.Guid.Full, targetObject.Guid.Full));
                    break;

                case EmoteType.RemoveContract:

                    if (player != null)
                        player.HandleActionAbandonContract((uint)emoteAction.Stat);
                    break;

                case EmoteType.RemoveVitaePenalty:

                    if (player != null) player.VitaeCpPool = 0;
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
                    creature = sourceObject as Creature;
                    if (creature != null)
                        creature.EyesPaletteDID = (uint)emoteAction.Display;
                    break;

                case EmoteType.SetEyeTexture:
                    creature = sourceObject as Creature;
                    if (creature != null)
                        creature.EyesTextureDID = (uint)emoteAction.Display;
                    break;

                case EmoteType.SetFloatStat:
                    player.SetProperty((PropertyFloat)emoteAction.Stat, (float)emoteAction.Amount);
                    break;

                case EmoteType.SetHeadObject:
                    creature = sourceObject as Creature;
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
                    ///Work needs to be done here
                    qManager.Add(emoteAction.Message, player);
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

                    if (player != null)
                    {
                        wcid = (uint)emoteAction.WeenieClassId;
                        item = WorldObjectFactory.CreateNewWorldObject(wcid);
                        if (item == null) break;

                        var success = player.TryRemoveItemFromInventoryWithNetworking(item, (ushort)emoteAction.Amount);
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
                    // player.Teleport(emote.Position);
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
                        if (fellowship != null)
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Tell));
                        else
                        {
                            foreach (var fellow in fellowship.FellowshipMembers)
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Tell));
                        }
                    }
                    break;

                case EmoteType.TextDirect:

                    text = emoteAction.Message;
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.AdminTell));
                    break;

                case EmoteType.Turn:
                    actionChain.AddDelaySeconds(emoteAction.Delay);
                    creature = sourceObject is Creature ? (Creature)sourceObject : null;
                    var pos = new ACE.Entity.Position(creature.Location.Cell, creature.Location.PositionX, creature.Location.PositionY, creature.Location.PositionZ, emoteAction.AnglesX ?? 0, emoteAction.AnglesY ?? 0, emoteAction.AnglesZ ?? 0, emoteAction.AnglesW ?? 0);
                    actionChain.AddAction(sourceObject, () =>
                    {
                        creature.TurnTo(pos);
                    });
                    actionChain.AddDelaySeconds(creature.GetTurnToDelay(pos));
                    break;

                case EmoteType.TurnToTarget:
                    actionChain.AddDelaySeconds(emoteAction.Delay);
                    creature = sourceObject is Creature ? (Creature)sourceObject : null;
                    actionChain.AddAction(sourceObject, () =>
                    {
                        creature.Rotate(player);
                    });
                    actionChain.AddDelaySeconds(creature.GetRotateDelay(player));
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
                    qManager.Add(emoteAction.Message, player);
                    if (qManager.Inquiry(emoteAction.Message, player))
                    {
                        var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 12 && emote2.Quest == emoteAction.Message);
                        if (result.Count() < 1)
                        {
                            result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 12 && emote2.Quest == emoteAction.Message);
                        }
                        if (result.Count() > 0)
                        {
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                        }
                    }
                    else
                    {
                        var result = sourceObject.Biota.BiotaPropertiesEmote.Where(emote2 => emote2.Category == 13 && emote2.Quest == emoteAction.Message);
                        if (result.Count() > 0)
                        {
                            var actions = sourceObject.Biota.BiotaPropertiesEmoteAction.Where(action => action.EmoteSetId == result.FirstOrDefault().EmoteSetId && action.EmoteCategory == result.FirstOrDefault().Category);

                            foreach (var action in actions)
                            {
                                actionChain.AddAction(player, () =>
                                {
                                    ExecuteEmote(result.FirstOrDefault(), action, actionChain, sourceObject, player);
                                });
                            }
                        }
                    }
                    break;
                case EmoteType.WorldBroadcast:
                    actionChain.AddDelaySeconds(emoteAction.Delay);
                    actionChain.AddAction(sourceObject, () =>
                    {
                        player.Session.Network.EnqueueSend(new GameMessageHearDirectSpeech(sourceObject, emoteAction.Message, player, ChatMessageType.WorldBroadcast));
                    });
                    break;

                default:
                    log.Debug($"EmoteManager.Execute - Encountered Unhandled EmoteType {(EmoteType)emoteAction.Type} for {sourceObject.Name} ({sourceObject.WeenieClassId})");
                    break;
            }
        }

        public IEnumerable<BiotaPropertiesEmoteAction> EmoteSet(uint emoteCategory, uint emoteSetId)
        {
            return WorldObject.Biota.BiotaPropertiesEmoteAction.Where(x => x.EmoteCategory == emoteCategory && x.EmoteSetId == emoteSetId);
        }

        public IEnumerable<BiotaPropertiesEmote> Emotes(EmoteCategory emoteCategory)
        {
            return WorldObject.Biota.BiotaPropertiesEmote.Where(x => x.Category == (int)emoteCategory);
        }

        /// <summary>
        /// Processes the emote queue
        /// </summary>
        public void ProcessQueue()
        {
            while (EmoteQueue.Count > 0)
            {
                var emote = EmoteQueue.Peek();

                if (emote.ExecuteTime > Timer.CurrentTime || WorldObject.IsBusy || WorldObject.IsMovingTo)
                    break;

                EmoteQueue.Dequeue();

                AddTime();
            }
        }

        /// <summary>
        /// Sets the execute time for the next item in the queue
        /// </summary>
        public void AddTime()
        {
            if (EmoteQueue.Count == 0) return;
            var emote = EmoteQueue.Peek();
            emote.ExecuteTime = Timer.CurrentTime + emote.Data.Delay;
        }
    }
}
