using System;
using System.Diagnostics;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public enum UnlockResults : ushort
    {
        UnlockSuccess = 0,
        PickLockFailed = 1,
        IncorrectKey = 2,
        AlreadyUnlocked = 3,
        CannotBePicked = 4,
        Open = 5
    }
    public interface Lock
    {
        UnlockResults Unlock(uint unlockerGuid, string keyCode);
        UnlockResults Unlock(uint unlockerGuid, uint playerLockpickSkillLvl, ref int difficulty);
    }
    public class UnlockerHelper
    {
        public static void ConsumeUnlocker(Player player, WorldObject unlocker)
        {
            if ((unlocker.GetProperty(PropertyBool.UnlimitedUse) ?? false))
            {
                player.SendUseDoneEvent();
                return;
            }
            unlocker.Structure--;
            if (unlocker.Structure < 1)
                player.TryConsumeFromInventoryWithNetworking(unlocker, 1);

            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
            player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(unlocker, PropertyInt.Structure, (int)unlocker.Structure));

            var unlockerType = unlocker is Lockpick ? "lockpick" : "key";
            if (unlocker.Structure < 1)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {unlockerType} is used up.", ChatMessageType.Broadcast));
            }
            else
            {
                var usePlural = unlocker.Structure == 1 ? "use" : "uses";
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {unlockerType} has {unlocker.Structure} {usePlural} left.", ChatMessageType.Broadcast));
            }
        }
        public static void UseUnlocker(Player player, WorldObject unlocker, WorldObject target)
        {
            ActionChain chain = new ActionChain();

            chain.AddAction(player, () =>
            {
                if (unlocker.WeenieType == WeenieType.Lockpick &&
                    player.Skills[Skill.Lockpick].AdvancementClass != SkillAdvancementClass.Trained &&
                    player.Skills[Skill.Lockpick].AdvancementClass != SkillAdvancementClass.Specialized)
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouArentTrainedInLockpicking));
                    return;
                }
                if (target is Lock @lock)
                {
                    UnlockResults result = UnlockResults.IncorrectKey;
                    var difficulty = 0;
                    if (unlocker.WeenieType == WeenieType.Lockpick)
                        result = @lock.Unlock(player.Guid.Full, player.Skills[Skill.Lockpick].Current, ref difficulty);
                    else if (unlocker is Key woKey)
                    {
                        if (target is Door woDoor)
                        {
                            if (woDoor.LockCode == "") // the door isn't to be opened with keys
                            {
                                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouCannotLockOrUnlockThat));
                                return;
                            }
                        }
                        result = @lock.Unlock(player.Guid.Full, woKey.KeyCode);
                    }

                    switch (result)
                    {
                        case UnlockResults.UnlockSuccess:

                            if (unlocker.WeenieType == WeenieType.Lockpick)
                            {
                                player.HandleActionApplySoundEffect(Sound.Lockpicking);// Sound.Lockpicking doesn't work via EnqueueBroadcastSound for some reason.

                                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have successfully picked the lock! It is now unlocked.", ChatMessageType.Broadcast));

                                var lockpickSkill = player.GetCreatureSkill(Skill.Lockpick);
                                Proficiency.OnSuccessUse(player, lockpickSkill, (uint)difficulty);
                            }
                            else
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{target.Name} has been unlocked.", ChatMessageType.Broadcast));

                            ConsumeUnlocker(player, unlocker);
                            break;

                        case UnlockResults.Open:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouCannotLockWhatIsOpen));
                            break;
                        case UnlockResults.AlreadyUnlocked:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.LockAlreadyUnlocked));
                            break;
                        case UnlockResults.PickLockFailed:
                            target.EnqueueBroadcast(new GameMessageSound(target.Guid, Sound.PicklockFail, 1.0f));
                            ConsumeUnlocker(player, unlocker);
                            break;
                        case UnlockResults.CannotBePicked:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouCannotLockOrUnlockThat));
                            break;
                        case UnlockResults.IncorrectKey:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.KeyDoesntFitThisLock));
                            break;
                    }
                }
                else
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouCannotLockOrUnlockThat));
                }
            });

            chain.EnqueueChain();
        }
    }
    public class LockHelper
    {
        public static int? GetResistLockpick(WorldObject me)
        {
            int? myResistLockpick = null;
            if (me is Door woDoor)
                myResistLockpick = woDoor.ResistLockpick;
            else if (me is Chest woChest)
                myResistLockpick = woChest.ResistLockpick;
            return myResistLockpick;
        }
        public static string GetLockCode(WorldObject me)
        {
            string myLockCode = null;
            if (me is Door woDoor)
                myLockCode = woDoor.LockCode;
            else if (me is Chest woChest)
                myLockCode = woChest.LockCode;
            return myLockCode;
        }
        public static UnlockResults Unlock(WorldObject target, string keyCode)
        {
            string myLockCode = GetLockCode(target);
            if (myLockCode == null) return UnlockResults.IncorrectKey;

            if (target.IsOpen)
                return UnlockResults.Open;

            if (keyCode.Equals(myLockCode, StringComparison.OrdinalIgnoreCase))
            {
                if (!target.IsLocked)
                    return UnlockResults.AlreadyUnlocked;

                target.IsLocked = false;
                var updateProperty = new GameMessagePublicUpdatePropertyBool(target, PropertyBool.Locked, target.IsLocked);
                var sound = new GameMessageSound(target.Guid, Sound.LockSuccess, 1.0f);
                target.EnqueueBroadcast(updateProperty, sound);
                return UnlockResults.UnlockSuccess;
            }
            return UnlockResults.IncorrectKey;
        }
        public static UnlockResults Unlock(WorldObject target, uint playerLockpickSkillLvl, ref int difficulty)
        {
            int? myResistLockpick = GetResistLockpick(target);

            if (!myResistLockpick.HasValue || myResistLockpick < 1)
                return UnlockResults.CannotBePicked;

            difficulty = myResistLockpick.Value;

            if (target.IsOpen)
                return UnlockResults.Open;

            if (!target.IsLocked)
                return UnlockResults.AlreadyUnlocked;

            var pickChance = SkillCheck.GetSkillChance((int)playerLockpickSkillLvl, (int)myResistLockpick);

#if DEBUG
            Debug.WriteLine($"{pickChance.FormatChance()} chance of UnlockSuccess");
#endif

            var dice = ThreadSafeRandom.Next(0.0f, 1.0f);
            if (dice > pickChance)
                return UnlockResults.PickLockFailed;

            target.IsLocked = false;
            target.EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(target, PropertyBool.Locked, target.IsLocked));
            //target.CurrentLandblock?.EnqueueBroadcastSound(target, Sound.Lockpicking);
            return UnlockResults.UnlockSuccess;
        }
    }

}
