using System;
using System.Diagnostics;

using ACE.Common;
using ACE.Common.Extensions;
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
        UnlockResults Unlock(uint unlockerGuid, Key key, string keyCode = null);
        UnlockResults Unlock(uint unlockerGuid, uint playerLockpickSkillLvl, ref int difficulty);
    }
    public class UnlockerHelper
    {
        public static void ConsumeUnlocker(Player player, WorldObject unlocker)
        {
            // is Sonic Screwdriver supposed to be consumed on use?
            // it doesn't have a Structure, and it doesn't have PropertyBool.UnlimitedUse
            if (unlocker.Structure == null || (unlocker.GetProperty(PropertyBool.UnlimitedUse) ?? false))
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
                        result = @lock.Unlock(player.Guid.Full, woKey);
                    }

                    switch (result)
                    {
                        case UnlockResults.UnlockSuccess:

                            if (unlocker.WeenieType == WeenieType.Lockpick)
                            {
                                player.HandleActionApplySoundEffect(Sound.Lockpicking);// Sound.Lockpicking doesn't work via EnqueueBroadcastSound for some reason.

                                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have successfully picked the lock! It is now unlocked.", ChatMessageType.Broadcast));

                                var lockpickSkill = player.GetCreatureSkill(Skill.Lockpick);
                                Proficiency.OnSuccessUse(player, lockpickSkill, difficulty);
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
        /// <summary>
        /// Returns TRUE if wo is a lockable item that can be picked
        /// </summary>
        public static bool IsPickable(WorldObject wo)
        {
            if (!(wo is Lock)) return false;

            var resistLockpick = wo.ResistLockpick;

            // TODO: find out if ResistLockpick >= 9999 is a special 'unpickable' value in acclient,
            // similar to ResistMagic >= 9999 being equivalent to Unenchantable?

            if (resistLockpick == null || resistLockpick >= 9999 )
                return false;

            return true;
        }

        public static int? GetResistLockpick(WorldObject wo)
        {
            if (!(wo is Lock)) return null;

            // if base ResistLockpick without enchantments is unpickable,
            // do not apply enchantments
            var isPickable = IsPickable(wo);

            if (!isPickable)
                return wo.ResistLockpick;

            var resistLockpick = wo.ResistLockpick.Value;
            var enchantmentMod = wo.EnchantmentManager.GetResistLockpick();

            var difficulty = resistLockpick + enchantmentMod;

            // minimum 0 difficulty
            difficulty = Math.Max(0, difficulty);

            return difficulty;
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
        public static UnlockResults Unlock(WorldObject target, Key key, string keyCode = null)
        {
            if (keyCode == null)
                keyCode = key?.KeyCode;

            string myLockCode = GetLockCode(target);
            if (myLockCode == null) return UnlockResults.IncorrectKey;

            if (target.IsOpen)
                return UnlockResults.Open;

            // there is only 1 instance of an 'opens all' key in PY16 data, 'keysonicscrewdriver'
            // which uses keyCode '_bohemund's_magic_key_'

            // when LSD added the rare skeleton key (keyrarevolatileuniversal),
            // they used PropertyBool.OpensAnyLock, which appears to have been used for something else in retail on Writables:

            // https://github.com/ACEmulator/ACE-World-16PY/blob/master/Database/3-Core/9%20WeenieDefaults/SQL/Key/Key/09181%20Sonic%20Screwdriver.sql
            // https://github.com/ACEmulator/ACE-World-16PY/search?q=OpensAnyLock

            if (keyCode != null && (keyCode.Equals(myLockCode, StringComparison.OrdinalIgnoreCase) || keyCode.Equals("_bohemund's_magic_key_")) ||
                    key != null && key.OpensAnyLock)
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
            var isPickable = IsPickable(target);

            if (!isPickable)
                return UnlockResults.CannotBePicked;

            int? myResistLockpick = GetResistLockpick(target);

            difficulty = myResistLockpick.Value;

            if (target.IsOpen)
                return UnlockResults.Open;

            if (!target.IsLocked)
                return UnlockResults.AlreadyUnlocked;

            var pickChance = SkillCheck.GetSkillChance((int)playerLockpickSkillLvl, difficulty);

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
