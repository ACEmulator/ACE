using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Animation;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public partial class EmoteManager
    {
        /// <summary>
        /// Example code for implementing EmoteTypes
        /// For actual implementation, please place working and verified code in EmoteManager.ExecuteEmote()
        /// </summary>
        /// <param name="emote"></param>
        /// <param name="target"></param>
        [Obsolete("Example code only. Please use EmoteManager.ExecuteEmote()")]
        public void Execute(Emote emote, WorldObject target)
        {
            var player = target as Player;
            var creature = target as Creature;

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

                    var text = Replace(emote.Message, WorldObject, target);
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

                    text = Replace(emote.Message, WorldObject, target);
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));
                    break;

                case EmoteType.EraseMyQuest:
                    break;
                case EmoteType.EraseQuest:
                    break;
                case EmoteType.FellowBroadcast:

                    text = Replace(emote.Message, WorldObject, target);
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
                    //InqCategory(EmoteCategory.GotoSet, emote);
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

                    text = Replace(emote.Message, WorldObject, target);
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

                    text = Replace(emote.Message, WorldObject, target);
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

                    text = Replace(emote.Message, WorldObject, target);
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Tell));
                    break;

                case EmoteType.TellFellow:

                    text = Replace(emote.Message, WorldObject, target);
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

                    text = Replace(emote.Message, WorldObject, target);
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

                    text = Replace(emote.Message, WorldObject, target);
                    players = WorldManager.GetAll();
                    foreach (var _player in players)
                        _player.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.WorldBroadcast));
                    break;
            }
        }
    }
}
