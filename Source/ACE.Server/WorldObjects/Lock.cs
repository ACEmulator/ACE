using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using System.Diagnostics;

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
        UnlockResults Unlock(string keyCode);
        UnlockResults Unlock(uint playerLockpickSkillLvl);
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
        public static UnlockResults Unlock(WorldObject me, string keyCode)
        {
            string myLockCode = GetLockCode(me);
            if (myLockCode == null) return UnlockResults.IncorrectKey;

            if (me.IsOpen ?? false)
                return UnlockResults.Open;

            if (keyCode == myLockCode)
            {
                if (!me.IsLocked ?? false)
                    return UnlockResults.AlreadyUnlocked;

                me.IsLocked = false;
                me.CurrentLandblock.EnqueueBroadcast(me.Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(me, PropertyBool.Locked, me.IsLocked ?? false));
                me.CurrentLandblock.EnqueueBroadcastSound(me, Sound.LockSuccess);
                return UnlockResults.UnlockSuccess;
            }
            return UnlockResults.IncorrectKey;
        }
        public static UnlockResults Unlock(WorldObject me, uint playerLockpickSkillLvl)
        {
            int? myResistLockpick = GetResistLockpick(me);

            if (!myResistLockpick.HasValue || myResistLockpick < 1)
                return UnlockResults.CannotBePicked;

            if (me.IsOpen ?? false)
                return UnlockResults.Open;

            if (!me.IsLocked ?? false)
                return UnlockResults.AlreadyUnlocked;

            var pickChance = SkillCheck.GetSkillChance((int)playerLockpickSkillLvl, (int)myResistLockpick);

#if DEBUG
            Debug.WriteLine($"{pickChance.FormatChance()} chance of UnlockSuccess");
#endif

            var dice = Physics.Common.Random.RollDice(0.0f, 1.0f);
            if (dice > pickChance)
                return UnlockResults.PickLockFailed;

            me.IsLocked = false;
            me.CurrentLandblock.EnqueueBroadcast(me.Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(me, PropertyBool.Locked, me.IsLocked ?? false));
            me.CurrentLandblock.EnqueueBroadcastSound(me, Sound.LockSuccess);
            return UnlockResults.UnlockSuccess;
        }
    }

}
