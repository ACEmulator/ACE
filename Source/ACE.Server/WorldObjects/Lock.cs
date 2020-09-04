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
        public static void ConsumeUnlocker(Player player, WorldObject unlocker, WorldObject target, bool success)
        {
            // is Sonic Screwdriver supposed to be consumed on use?
            // it doesn't have a Structure, and it doesn't have PropertyBool.UnlimitedUse

            var unlimitedUses = unlocker.Structure == null || (unlocker.GetProperty(PropertyBool.UnlimitedUse) ?? false);
            var isLockpick = unlocker.WeenieType == WeenieType.Lockpick;

            var msg = "";
            if (isLockpick)
            {
                if (success)
                    msg = "You have successfully picked the lock!  It is now unlocked.\n ";
                else
                    msg = "You have failed to pick the lock.  It is still locked.  ";
            }
            else if (success)
            {
                msg = $"The {target.Name} has been unlocked.\n";
            }
            else
            {
                msg = $"The {target.Name} is still locked.\n";
            }

            if (!unlimitedUses)
            {
                msg += $"Your {(isLockpick ? "lockpicks" : "key")} ";

                unlocker.Structure--;

                if (unlocker.Structure < 1)
                {
                    msg += $"{(isLockpick ? "are" : "is")} used up.";
                }
                else
                {
                    msg += $"{(isLockpick ? "have" : "has")} {unlocker.Structure} use{(unlocker.Structure > 1 ? "s" : "")} left.";
                }
            }

            player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
            if (!unlimitedUses)
            {
                if (unlocker.Structure < 1)
                {
                    player.TryConsumeFromInventoryWithNetworking(unlocker, 1);
                }
                else
                {
                    player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(unlocker, PropertyInt.Structure, (int)unlocker.Structure));
                }
            }
            player.SendUseDoneEvent();
        }
        public static uint GetEffectiveLockpickSkill(Player player, WorldObject unlocker)
        {
            var lockpickSkill = player.GetCreatureSkill(Skill.Lockpick).Current;

            var additiveBonus = unlocker.GetProperty(PropertyInt.LockpickMod) ?? 0;
            var multiplicativeBonus = unlocker.GetProperty(PropertyFloat.LockpickMod) ?? 1.0f;

            // is this really 10x bonus, or +10% bonus?
            if (multiplicativeBonus > 1.0f)
                multiplicativeBonus = 1.0f + multiplicativeBonus * 0.01f;

            var effectiveSkill = (int)Math.Round(lockpickSkill * multiplicativeBonus + additiveBonus);

            effectiveSkill = Math.Max(0, effectiveSkill);

            //Console.WriteLine($"Base skill: {lockpickSkill}");
            //Console.WriteLine($"Effective skill: {effectiveSkill}");

            return (uint)effectiveSkill;
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
                    {
                        var effectiveLockpickSkill = GetEffectiveLockpickSkill(player, unlocker);
                        result = @lock.Unlock(player.Guid.Full, effectiveLockpickSkill, ref difficulty);
                    }
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
                                // the source guid for this sound must be the player, else the sound will not play
                                // which differs from PicklockFail and LockSuccess being in the target sound table
                                player.EnqueueBroadcast(new GameMessageSound(player.Guid, Sound.Lockpicking, 1.0f));

                                var lockpickSkill = player.GetCreatureSkill(Skill.Lockpick);
                                Proficiency.OnSuccessUse(player, lockpickSkill, difficulty);
                            }

                            ConsumeUnlocker(player, unlocker, target, true);
                            break;

                        case UnlockResults.Open:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouCannotLockWhatIsOpen));
                            break;
                        case UnlockResults.AlreadyUnlocked:
                            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.LockAlreadyUnlocked));
                            break;
                        case UnlockResults.PickLockFailed:
                            target.EnqueueBroadcast(new GameMessageSound(target.Guid, Sound.PicklockFail, 1.0f));
                            ConsumeUnlocker(player, unlocker, target, false);
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
            if (dice >= pickChance)
                return UnlockResults.PickLockFailed;

            target.IsLocked = false;
            target.EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(target, PropertyBool.Locked, target.IsLocked));
            //target.CurrentLandblock?.EnqueueBroadcastSound(target, Sound.Lockpicking);
            return UnlockResults.UnlockSuccess;
        }
    }
}
